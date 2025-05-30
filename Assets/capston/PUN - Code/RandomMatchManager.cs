using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;

public class RandomMatchManager : MonoBehaviourPunCallbacks
{
    public GameObject mainMenuPanel;
    public GameObject randomMatchPanel;
    public TMP_Text matchStatusText; // TextMeshPro status message
    public string gameSceneName = "MutiPlayScene";

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {


        if (matchStatusText == null)
        {
            // TMP_Text를 자동으로 찾아 연결 시도
            matchStatusText = GameObject.Find("matching People")?.GetComponent<TMP_Text>();
            Debug.LogWarning("matchStatusText was not assigned. Auto-connected: " + (matchStatusText != null));
        }


        if (!PhotonNetwork.IsConnected)
        {

            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public void OnClickRandomMatchButton()
    {
        mainMenuPanel.SetActive(false);
        randomMatchPanel.SetActive(true);
        matchStatusText.text = "Looking for an opponent...";
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("Room_" + Random.Range(1000, 9999), new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        matchStatusText.text = "Waiting for opponent to join...";
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("New player joined");
        if (matchStatusText != null)
        {
            matchStatusText.text = "Opponent found!\nGame will start in 5 seconds.";
        }

        Debug.Log("New player joined: " + newPlayer);
        Debug.Log("Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            if (photonView == null)
            {
                Debug.LogError("photonView is NULL. Did you forget to add PhotonView component?");
                return;
            }

            photonView.RPC("StartCountdown", RpcTarget.All);
        }
    }

    [PunRPC]
    void StartCountdown()
    {
        if (matchStatusText == null)
        {
            Debug.LogWarning("StartCountdown: matchStatusText is null");
            return;
        }

        StartCoroutine(CountdownToStart());
    }


    IEnumerator CountdownToStart()
    {
        if (matchStatusText != null)
        {
            matchStatusText.text = "Opponent found!\nGame will start in 5 seconds.";
        }

        yield return new WaitForSeconds(5f);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Master is loading scene: " + gameSceneName);
            PhotonNetwork.LoadLevel(gameSceneName);
        }
    }

}
