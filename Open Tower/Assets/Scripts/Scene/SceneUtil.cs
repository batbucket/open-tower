using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneUtil {
    public const int MAIN_MENU_INDEX = 0;
    public const int LEVEL_SELECT_INDEX = 6;
    public const int BROWSER_INDEX = 5;
    public const int EDITOR_INDEX = 3;
    public const int LEVEL_START_INDEX = 7;
    public static readonly int LEVEL_END_INDEX = 30;
    public static readonly int NUMBER_OF_LEVELS = LEVEL_END_INDEX - LEVEL_START_INDEX + 1;

    public static PlayType Play;

    public static int LevelIndex {
        get {
            int index = SceneManager.GetActiveScene().buildIndex;
            Util.Assert(IsLevelIndex, "Scene is not an official level!");
            return index - LEVEL_START_INDEX;
        }
    }

    public static bool IsLevelIndex {
        get {
            int index = SceneManager.GetActiveScene().buildIndex;
            Debug.Log(index);
            return
                index >= LEVEL_START_INDEX
                && index <= LEVEL_END_INDEX;
        }
    }

    private static readonly DungeonSet[] dungeonSets = new DungeonSet[] { // sets to use for each world
        SpriteList.GetDungeonSet(PathType.TUTOR),
        SpriteList.GetDungeonSet(PathType.GRASS),
        SpriteList.GetDungeonSet(PathType.TOWER),
        SpriteList.GetDungeonSet(PathType.DEATH),
        SpriteList.GetDungeonSet(PathType.VOID),
    };

    private static readonly LevelParams[] levels = new LevelParams[] {
        new LevelParams(0, "Gestation"), // story
        new LevelParams(0, 0, 337514, "Baby Steps"), // tute begin
        new LevelParams(0, 1, 337515, "Keys I"),
        new LevelParams(0, 2, 337516, "Keys II"),
        new LevelParams(0, 3, 337517, "Combat I"),
        new LevelParams(0, 4, 337518, "Combat II"),
        new LevelParams(1, "Friendship"), // story
        new LevelParams(1, 0, 337759, "The Armory"), // life begin
        new LevelParams(1, 1, 337760, "Change"),
        new LevelParams(1, 2, 337761, "Positivity"),
        new LevelParams(1, 3, 337762, "Overflow"),
        new LevelParams(2, 0, 338158, "Bare Necessities"), // limbo begin
        new LevelParams(2, 1, 338159, "Nine Lives"),
        new LevelParams(2, 2, 338160, "Turning Tower"),
        new LevelParams(2, 3, 337763, "Maze Monolith"),
        new LevelParams(3, 0, 337764, "Decide I"), // death begin
        new LevelParams(3, 1, 337765, "Decide II"),
        new LevelParams(3, 2, 337766, "Magic Tower"),
        new LevelParams(3, 3, 337767, "Skeletower"),
        new LevelParams(4, 0, 338161, "The Morgue"), // void begin
        new LevelParams(4, 1, 337768, "The Horde"),
        new LevelParams(4, 2, 338162, "Seal Chamber I"),
        new LevelParams(4, 3, 338163, "Seal Chamber II"),
        new LevelParams(4, 4, 337769, "Floored")
    };

    public static DungeonSet GetSet(int levelIndex) {
        return dungeonSets[GetParams(levelIndex).WorldIndex];
    }

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