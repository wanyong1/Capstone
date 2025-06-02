using Photon.Pun;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject multiplayerMenu;
    public GameObject shopPanel; //  Shop �г� ����

    public void OpenMultiplayerMenu()
    {
        GameModeManager.IsMultiplayer = true;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // ���� 1ȸ�� ���� �õ�
        }
        mainMenu.SetActive(false);
        multiplayerMenu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        multiplayerMenu.SetActive(false);
        shopPanel.SetActive(false);      //  Shop �гε� ����
        mainMenu.SetActive(true);
    }

    // Shop ��ư ������ ��
    public void OpenShop()
    {
        mainMenu.SetActive(false);
        shopPanel.SetActive(true);
    }

    // Shop �� Back ��ư ������ ��
    public void BackFromShop()
    {
        shopPanel.SetActive(false);
        mainMenu.SetActive(true);
    }
}
