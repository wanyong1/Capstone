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
            PhotonNetwork.ConnectUsingSettings(); // 최초 1회만 연결 시도
        }
        mainMenu.SetActive(false);         // 메인 메뉴 비활성화
        multiplayerMenu.SetActive(true);   // 멀티 메뉴 활성화
    }
    public void BackToMainMenu()
    {
        multiplayerMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}

