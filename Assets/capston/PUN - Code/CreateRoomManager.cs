using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class CreateRoomManager : MonoBehaviourPunCallbacks
{
    public TMP_Text roomCodeText;
    public TMP_Text opponentStatusText;

    public Button readyButton;
    public Button startGameButton;
    public Button cancelButton;

    private bool isReady = false;
    private int readyCount = 0;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void OnEnable()
    {
        startGameButton.interactable = false;
        opponentStatusText.text = "Opponent: Not Ready";

        if (PhotonNetwork.InRoom)
        {
            roomCodeText.text = "Room Code: " + PhotonNetwork.CurrentRoom.Name;
        }

        readyButton.onClick.AddListener(OnClickReady);
        startGameButton.onClick.AddListener(OnClickStartGame);
        cancelButton.onClick.AddListener(OnClickCancel);
    }

    public void OnClickReady()
    {
        if (isReady) return;  // 중복 방지
        isReady = true;
        readyButton.interactable = false;
        photonView.RPC("PlayerReady", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void PlayerReady()
    {
        readyCount++;

        if (!PhotonNetwork.IsMasterClient)
        {
            opponentStatusText.text = "Opponent: Ready";
        }

        if (readyCount >= 2 && PhotonNetwork.IsMasterClient)
        {
            startGameButton.interactable = true;
        }
    }

    public void OnClickStartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("MutiPlayScene");
        }
    }

    public void OnClickCancel()
    {
        PhotonNetwork.LeaveRoom();
        this.gameObject.SetActive(false);
        // 메뉴로 복귀 처리 (필요시 추가)
    }


}
