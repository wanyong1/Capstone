using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static bool IsMultiplayer = false;

    void Awake()
    {
        IsMultiplayer = false; // �̱� �÷��� �⺻��
    }
}
