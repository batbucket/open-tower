using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneUtil {
    public const int MAIN_MENU_INDEX = 0;
    public const int LEVEL_SELECT_INDEX = 6;
    public const int BROWSER_INDEX = 5;
    public const int EDITOR_INDEX = 3;
    public const int LEVEL_START_INDEX = 7;
    public static readonly int LEVEL_END_INDEX = SceneManager.sceneCountInBuildSettings - 1;
    public static int NUMBER_OF_LEVELS = LEVEL_END_INDEX - LEVEL_START_INDEX + 1;

    public static PlayType Play;

    public static int GetNextSceneIndex() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Playtype: " + Play.ToString());
        int nextSceneIndex = -1;
        if (Play == PlayType.LEVEL_SELECT) {
            nextSceneIndex = LEVEL_SELECT_INDEX;
        } else {
            if (currentSceneIndex < LEVEL_END_INDEX) {
                nextSceneIndex = currentSceneIndex + 1;
            } else {
                nextSceneIndex = MAIN_MENU_INDEX;
            }
        }
        Debug.Log("next index is: " + nextSceneIndex);
        return nextSceneIndex;
    }
}