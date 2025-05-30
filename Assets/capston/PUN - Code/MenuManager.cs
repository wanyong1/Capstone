using Photon.Pun;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject multiplayerMenu;

    public void OpenMultiplayerMenu()
    {
        GameModeManager.IsMultiplayer = true;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // ���� 1ȸ�� ���� �õ�
        }
        mainMenu.SetActive(false);         // ���� �޴� ��Ȱ��ȭ
        multiplayerMenu.SetActive(true);   // ��Ƽ �޴� Ȱ��ȭ
    }
    public void BackToMainMenu()
    {
        multiplayerMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}

