﻿using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneUtil {
    public const int MAIN_MENU_INDEX = 0;
    public const int LEVEL_SELECT_INDEX = 6;
    public const int BROWSER_INDEX = 5;
    public const int EDITOR_INDEX = 3;
    public const int LEVEL_START_INDEX = 7;
    public static readonly int LEVEL_END_INDEX = SceneManager.sceneCountInBuildSettings - 1;
    public static readonly int NUMBER_OF_LEVELS = LEVEL_END_INDEX - LEVEL_START_INDEX + 1;

    public static PlayType Play;

    private static readonly LevelParams[] levels = new LevelParams[] {
        new LevelParams(0, "Gestation"), // story
        new LevelParams(0, 0, "Movement"), // tute begin
        new LevelParams(0, 1, "Keys I"),
        new LevelParams(0, 2, "Keys II"),
        new LevelParams(0, 3, "Combat I"),
        new LevelParams(0, 4, "Combat II"),
        new LevelParams(0, "Friendship"), // story
        new LevelParams(1, 0, "Imitation"), // life begin
        new LevelParams(1, 1, "Enterprise"),
        new LevelParams(1, 2, "Dedication"),
        new LevelParams(1, 3, "Renunciation"),
        new LevelParams(2, 0, "Event Horizon"), // maze tower
        new LevelParams(3, 0, "Shock"), // death begin
        new LevelParams(3, 1, "Denial"),
        new LevelParams(3, 2, "Anger"),
        new LevelParams(3, 3, "Bargaining"),
        new LevelParams(4, 0, "Grief"), // finale begin
        new LevelParams(4, 1, "Acceptance")
    };

    public static LevelParams GetParams(int levelIndex) {
        return levels[levelIndex];
    }

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