using UnityEngine;
public class Constant : MonoBehaviour
{
    // Class that holds static constants/Game Behavior Flags for ease of playtesting
    public enum SceneName
    {
        Main_Menu,
        Forest_Cave,
        Forest_1
    }
    public static SceneName STARTING_SCENE = SceneName.Forest_Cave;

    public static int STARTING_HEARTS = 3;
    public static float STARTING_STAMINA = 3f;
    public static int NUMBER_OF_AREAS = 5;
    public static int PPU = 8;

    // Game Feature Flags
    public static bool HAS_TIMED_COMBO = false;
    public static bool HAS_STAMINA = false;
    public static bool STOP_WHEN_ATTACK = true;
    public static bool ATTACK_STUNS_ENEMY = false;
}