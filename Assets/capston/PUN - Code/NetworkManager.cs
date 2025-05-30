using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log(" Photon 서버 접속 시도 중...");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(" Photon 서버 연결 성공!");
        PhotonNetwork.JoinLobby();  // 로비 접속
    }

    public override void OnJoinedLobby()
    {
        Debug.Log(" 로비 입장 완료!");
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogWarning($" 서버 연결 끊김: {cause}");
    }
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
