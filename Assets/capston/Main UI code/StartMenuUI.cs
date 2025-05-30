using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuUI : MonoBehaviour
{
    public void StartGame()
    {
        GameModeManager.IsMultiplayer = false;
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("게임 종료"); // 에디터에서는 이거만 찍힘
    }
}
