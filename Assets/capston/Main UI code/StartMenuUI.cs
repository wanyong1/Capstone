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
        Debug.Log("���� ����"); // �����Ϳ����� �̰Ÿ� ����
    }
}
