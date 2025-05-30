using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log(" Photon ���� ���� �õ� ��...");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(" Photon ���� ���� ����!");
        PhotonNetwork.JoinLobby();  // �κ� ����
    }

    public override void OnJoinedLobby()
    {
        Debug.Log(" �κ� ���� �Ϸ�!");
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogWarning($" ���� ���� ����: {cause}");
    }
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
