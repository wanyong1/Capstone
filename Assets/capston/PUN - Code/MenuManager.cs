using Photon.Pun;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject multiplayerMenu;
    public GameObject shopPanel; //  Shop 패널 연결

    public void OpenMultiplayerMenu()
    {
        GameModeManager.IsMultiplayer = true;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // 최초 1회만 연결 시도
        }
        mainMenu.SetActive(false);
        multiplayerMenu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        multiplayerMenu.SetActive(false);
        shopPanel.SetActive(false);      //  Shop 패널도 닫음
        mainMenu.SetActive(true);
    }

    // Shop 버튼 눌렀을 때
    public void OpenShop()
    {
        mainMenu.SetActive(false);
        shopPanel.SetActive(true);
    }

    // Shop 내 Back 버튼 눌렀을 때
    public void BackFromShop()
    {
        shopPanel.SetActive(false);
        mainMenu.SetActive(true);
    }
}
