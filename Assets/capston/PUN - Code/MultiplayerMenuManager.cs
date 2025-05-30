using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class MultiplayerMenuManager : MonoBehaviourPunCallbacks
{
    public GameObject multiplayerMenu;

    public GameObject nameInputPanel;        // �г��� �Է� �г� (Create & Join ����)
    public TMP_InputField nameInputField;

    public GameObject roomCodeInputPanel;    // Join�� �� �ڵ� �Է� �г�
    public TMP_InputField roomCodeInputField;

    public GameObject createRoomPanel;       // ��� UI (Create & Join ����)
    public TMP_Text myNameText;              // PlayerAreaLeft (�����)
    public TMP_Text opponentNameText;        // PlayerAreaRight (�����ڿ�)
    public TMP_Text roomCodeText;            // �� �ڵ� ǥ��
    public GameObject opponentReadyButton;   // ������ Ready ��ư

    private string roomName;

    private enum Mode { None, Create, Join }
    private Mode currentMode = Mode.None;

    // �� Create Room ��ư Ŭ��
    public void OnClickCreateRoomButton()
    {
        currentMode = Mode.Create;
        multiplayerMenu.SetActive(false);
        nameInputPanel.SetActive(true);
    }

    // �� Join Room ��ư Ŭ��
    public void OnClickJoinRoomButton()
    {
        currentMode = Mode.Join;
        multiplayerMenu.SetActive(false);
        nameInputPanel.SetActive(true);
    }

    // �� �г��� Confirm
    public void OnClickConfirmName()
    {
        string playerName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(playerName))
            playerName = "Player_" + Random.Range(1000, 9999);

        PhotonNetwork.NickName = playerName;
        nameInputPanel.SetActive(false);

        if (currentMode == Mode.Create)
        {
            createRoomPanel.SetActive(true);

            roomName = Random.Range(1000, 9999).ToString();
            RoomOptions options = new RoomOptions { MaxPlayers = 2 };
            PhotonNetwork.CreateRoom(roomName, options);

            myNameText.text = "Name: " + playerName;
            roomCodeText.text = "Room Code: " + roomName;
        }
        else if (currentMode == Mode.Join)
        {
            roomCodeInputPanel.SetActive(true);
        }
    }

    // �� �� �ڵ� �Է� �� Join
    public void OnClickJoinRoomByCode()
    {
        string code = roomCodeInputField.text.Trim();

        if (!string.IsNullOrEmpty(code))
        {
            PhotonNetwork.JoinRoom(code);
        }
        else
        {
            Debug.LogWarning("Enter a valid Room Code!");
        }
    }

    // �� �� ���� ���� ��
    public override void OnJoinedRoom()
    {
        if (currentMode == Mode.Join)
        {
            roomCodeInputPanel.SetActive(false);
            createRoomPanel.SetActive(true);

            opponentNameText.text = "Name: " + PhotonNetwork.NickName;
            opponentReadyButton.SetActive(true);
        }
    }



    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Room creation failed: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Join room failed: " + message);
    }

    public void OnClickBackToMenu()
    {
        roomCodeInputPanel.SetActive(false);
        nameInputPanel.SetActive(false);
        createRoomPanel.SetActive(false);
        multiplayerMenu.SetActive(true);

        currentMode = Mode.None;
        roomCodeInputField.text = "";
        nameInputField.text = "";
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log(" Opponent Joined: " + newPlayer.NickName);
            opponentNameText.text = "Name: " + newPlayer.NickName;
            opponentReadyButton.SetActive(true);
        }
    }

}
