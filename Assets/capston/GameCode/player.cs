using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 6f;
    private float speedMultiplier = 1f;
    float hAxis;
    float vAxis;
    Vector3 moveVec;
    //애니메이션
    Animator animator;
    //회전 오브젝트
    public GameObject orbitingPrefab;
    public int orbitingDamage = 5;
    //총알 관련 
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2.0f;
    private float nextFireTime = 0f;

    private bool isMouseShooting = false;
    //총알 업그레이드 관련
    public float plusBulletDamage = 5f;
    public float plusBulletCount = 1f;

    // 업그레이드
    private float baseBulletDamage = 5;
    private float inGameBulletUpgrade = 0;



    private PhotonView photonView;

    private void Start()
    {
        if (!GameModeManager.IsMultiplayer)
        {
            UpgradeStatsManager.Instance.LoadUpgradeLevels();  // 최신 업그레이드 정보 로드
        }

        //속도
        if (!GameModeManager.IsMultiplayer)
        {
            speedMultiplier += UpgradeStatsManager.Instance?.GetMoveSpeedBonus() ?? 0f;
        }


        int storeBonus = 0;

        // 싱글 모드일 경우에만 상점 업그레이드 적용 총알 데미지
        if (!GameModeManager.IsMultiplayer && UpgradeStatsManager.Instance != null)
        {
            storeBonus = (int)UpgradeStatsManager.Instance.GetBulletDamageBonus();
        }

        plusBulletDamage = baseBulletDamage + storeBonus + inGameBulletUpgrade;

        photonView = GetComponent<PhotonView>();// 초기화

        if (GameModeManager.IsMultiplayer && !photonView.IsMine)
        {
            this.enabled = false;
            return;
        }
        // 내 플레이어일 경우 Main Camera의 Follow.target 설정
        Camera cam = Camera.main;
        if (cam != null)
        {
            var follow = cam.GetComponent<CameraFollow>();
            if (follow != null)
            {
                follow.target = this.transform;
            }
            else
            {
                Debug.LogWarning("MainCamera에 CameraFollow 스크립트가 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("MainCamera가 씬에 존재하지 않습니다.");
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        HandleShootingMode();
        Shoot();
    }

    void Move()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        transform.position += moveVec * speed * speedMultiplier * Time.deltaTime;

        animator.SetBool("isRunLeft", hAxis < 0);
        animator.SetBool("isRunRight", hAxis > 0);
        animator.SetBool("isRunFWD", vAxis > 0);
        animator.SetBool("isRunBWD", vAxis < 0);

        if (isMouseShooting) RotateToMouse();
        else if (moveVec != Vector3.zero) transform.LookAt(transform.position + moveVec);
    }

    void HandleShootingMode()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMouseShooting = !isMouseShooting;
            Debug.Log("총알 모드 변경: " + (isMouseShooting ? "마우스 조준" : "키보드 방향"));
        }
    }

    void Shoot()
    {

        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            Vector3 shootDirection;
            Quaternion baseRotation;

            if (isMouseShooting)
            {
                Vector3 mousePosition = GetMouseWorldPosition();
                shootDirection = (mousePosition - firePoint.position).normalized;
                baseRotation = Quaternion.LookRotation(shootDirection);
            }
            else
            {
                shootDirection = moveVec != Vector3.zero ? moveVec : transform.forward;
                baseRotation = Quaternion.LookRotation(shootDirection);
            }

            float spreadAngle = 10f;
            float mid = plusBulletCount / 2;

            for (int i = 0; i < plusBulletCount; i++)
            {
                float offsetIndex = i - mid;
                if (plusBulletCount % 2 == 0) offsetIndex += 1;

                Quaternion rotation = baseRotation * Quaternion.Euler(0, spreadAngle * offsetIndex, 0);
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);

                Bullet bulletScript = bullet.GetComponent<Bullet>();
                if (bulletScript != null) bulletScript.damage = plusBulletDamage;
                Debug.Log($"총알 {i + 1} 생성됨 - 각도 오프셋: {spreadAngle * offsetIndex}");
            }

            animator.SetBool("isShooting", true);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("MainCamera가 null입니다! 마우스 조준 불가");
            return transform.position;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            return ray.GetPoint(rayDistance);
        }

        return transform.position;
    }

    void RotateToMouse()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 direction = (mousePosition - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(direction);
    }

    public void ApplyItem(Item.Type type, float value)
    {
        switch (type)
        {
            case Item.Type.BulletDamageUp:
                inGameBulletUpgrade += value;
                int storeBonus = 0;
                if (UpgradeStatsManager.Instance != null)
                {
                    storeBonus = (int)UpgradeStatsManager.Instance.GetBulletDamageBonus();
                }
                plusBulletDamage = baseBulletDamage + storeBonus + inGameBulletUpgrade;
                break;
            case Item.Type.FireRateUp:
                fireRate = Mathf.Max(0.1f, fireRate - 0.1f * value);
                break;
            case Item.Type.BulletCountUp:
                plusBulletCount += value;
                break;
        }
    }

    public void AddOrbitingObject()
    {
        if (orbitingPrefab == null)
        {
            Debug.LogError("⚠️ OrbitingPrefab이 연결되지 않았습니다!");
            return;
        }

        GameObject orbit = Instantiate(orbitingPrefab);
        orbit.transform.localScale = Vector3.one * 1.5f;

        OrbitingObject orbitScript = orbit.GetComponent<OrbitingObject>();
        if (orbitScript != null)
        {
            orbitScript.target = this.transform;
            orbitScript.orbitDamage = orbitingDamage;
        }
    }
    
  
}
