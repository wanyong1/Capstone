using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static bool IsMultiplayer = false;

    void Awake()
    {
        IsMultiplayer = false; // 싱글 플레이 기본값
    }
}
