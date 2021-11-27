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

    public static int startingHearts = 3;
    public static float startingStamina = 3f;
    public static int numOfAreas = 5;
    public static int ppu = 8;

    // Game Feature Flags
    public static bool hasTimedCombo = false;
    public static bool hasStamina = false;
    public static bool stopWhenAttack = true;
    public static bool stunEnemyAfterAttack = true;
}