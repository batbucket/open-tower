using Prime31.TransitionKit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class SceneUtil {
    public const int MAIN_MENU_INDEX = 0;
    public const int LEVEL_SELECT_INDEX = 6;
    public const int CUSTOM_INDEX = 4;
    public const int BROWSER_INDEX = 5;
    public const int EDITOR_INDEX = 3;
    public const int LEVEL_START_INDEX = 7;
    public const int LEVEL_BROWSER_INDEX = 5;
    public static readonly int LEVEL_END_INDEX = 34;
    public static readonly int NUMBER_OF_LEVELS = LEVEL_END_INDEX - LEVEL_START_INDEX + 1;
    private static AudioClip transitionSound = Resources.Load<AudioClip>("Sounds/steam hiss");
    private static TitleDrop titleDrop;

    public static PlayType Play;

    public static int LevelIndex {
        get {
            int index = SceneManager.GetActiveScene().buildIndex;
            Util.Assert(IsLevelIndex(index), "Scene is not an official level!");
            return index - LEVEL_START_INDEX;
        }
    }

    public static bool IsCurrentLevelIndex {
        get {
            int index = SceneManager.GetActiveScene().buildIndex;
            return
                index >= LEVEL_START_INDEX
                && index <= LEVEL_END_INDEX;
        }
    }

    public static bool IsLevelIndex(int index) {
        return
            index >= LEVEL_START_INDEX
            && index <= LEVEL_END_INDEX;
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
        new LevelParams(1, "Gravity"), // story
        new LevelParams(1, 0, 337759, "The Armory"), // life begin
        new LevelParams(1, 1, 337760, "Change"),
        new LevelParams(1, 2, 337761, 91600, "Positivity"),
        new LevelParams(1, 3, 337762, 91601, "Shift"),
        new LevelParams(2, "Safety"), // limbo begin
        new LevelParams(2, 0, 338158, "Bare Necessities"),
        new LevelParams(2, 1, 338159, "Nine Lives"),
        new LevelParams(2, 2, 338160, "Turning Tower"),
        new LevelParams(2, 3, 337763, 91599, "Maze Monolith"),
        new LevelParams(3, "Eyes of Flame"), // death begin
        new LevelParams(3, 0, 337764, "Decide I"),
        new LevelParams(3, 1, 337765, "Decide II"),
        new LevelParams(3, 2, 337766, "Magic Tower"),
        new LevelParams(3, 3, 337767, "Skeletower"),
        new LevelParams(4, "Quiet"), // void
        new LevelParams(4, 0, 338161, "The Morgue"),
        new LevelParams(4, 1, 337768, 91596, "The Horde"),
        new LevelParams(4, 2, 338162, 91597, "Seal Chamber I"),
        new LevelParams(4, 3, 338163, 91598, "Seal Chamber II"),
        new LevelParams(4, 4, 337769, 91595, "Floored"),
        new LevelParams(4, "Balcony"), // an ending
    };

    public static void LoadScene(int sceneIndex) {
        if (titleDrop == null) {
            titleDrop = GameObject.Instantiate(Resources.Load<TitleDrop>("Prefabs/Title Drop"));
            GameObject.DontDestroyOnLoad(titleDrop.gameObject);
            TransitionKit.onScreenObscured += () => SoundManager.Instance.Play(transitionSound);
            TransitionKit.onTransitionComplete += () => {
                if (IsCurrentLevelIndex) {
                    titleDrop.Init(GetParams(LevelIndex).Name);
                }
            };
        }

        bool isPlayTransition = (Application.platform != RuntimePlatform.WebGLPlayer);

        var transition = new VerticalSlicesTransition() {
            nextScene = sceneIndex,
            duration = isPlayTransition ? 0.25f : 0f,
            divisions = 800
        };
        TransitionKit.instance.transitionWithDelegate(transition);
    }

    public static DungeonSet GetSet(int levelIndex) {
        return dungeonSets[GetParams(levelIndex).WorldIndex];
    }

    public static LevelParams GetParams(int levelIndex) {
        return levels[levelIndex];
    }

    public static int GetExitSceneIndex() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = -1;
        switch (Play) {
            case PlayType.LEVEL_BROWSER:
                nextSceneIndex = LEVEL_BROWSER_INDEX;
                break;

            case PlayType.LEVEL_SELECT:
                nextSceneIndex = LEVEL_SELECT_INDEX;
                break;

            case PlayType.PLAY_TEST:
                nextSceneIndex = EDITOR_INDEX;
                break;

            case PlayType.STORY_MODE:
                nextSceneIndex = MAIN_MENU_INDEX;
                break;
        }
        return nextSceneIndex;
    }

    public static int GetNextSceneIndex() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Playtype: " + Play.ToString());
        int nextSceneIndex = -1;

        switch (Play) {
            case PlayType.LEVEL_BROWSER:
                nextSceneIndex = LEVEL_BROWSER_INDEX;
                break;

            case PlayType.LEVEL_SELECT:
                nextSceneIndex = LEVEL_SELECT_INDEX;
                break;

            case PlayType.PLAY_TEST:
                nextSceneIndex = EDITOR_INDEX;
                break;

            case PlayType.STORY_MODE:
                if (currentSceneIndex < LEVEL_END_INDEX) {
                    nextSceneIndex = currentSceneIndex + 1;
                } else {
                    nextSceneIndex = MAIN_MENU_INDEX;
                }
                break;
        }
        return nextSceneIndex;
    }
}