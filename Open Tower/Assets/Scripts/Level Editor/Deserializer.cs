using Scripts.LevelEditor.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deserializer : MonoBehaviour {
    private static Deserializer _instance;

    [SerializeField]
    private JSONLevel jsonLevel;

    [SerializeField]
    private GameObject floorPrefab;

    [SerializeField]
    private GameObject wallPrefab;

    [SerializeField]
    private GameObject upstairsPrefab;

    [SerializeField]
    private GameObject downstairsPrefab;

    [SerializeField]
    private GameObject goldKeyPrefab;

    [SerializeField]
    private GameObject blueKeyPrefab;

    [SerializeField]
    private GameObject redKeyPrefab;

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject exitPrefab;

    [SerializeField]
    private GameObject goldDoorPrefab;

    [SerializeField]
    private GameObject blueDoorPrefab;

    [SerializeField]
    private GameObject redDoorPrefab;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject boosterPrefab;

    private Upload upload;

    public static Deserializer Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<Deserializer>();
            }
            return _instance;
        }
    }

    public void CreateLevelFromJson() {
        this.upload = jsonLevel.Upload;

        // check play type
        int victoryScene = -1;
        int exitScene = -1;
        if (SceneUtil.Play == PlayType.STORY_MODE) {
            victoryScene = SceneUtil.GetNextSceneIndex();
            exitScene = SceneUtil.MAIN_MENU_INDEX;
        } else { // mode won't ever be level editor playtesting because it uses LevelInfo instead to init
            victoryScene = SceneUtil.LEVEL_SELECT_INDEX;
            exitScene = victoryScene;
        }
        Init(upload, jsonLevel.Stage, victoryScene, exitScene);
    }

    private void Awake() {
        LevelInfo info = LevelInfo.Instance;
        if (jsonLevel == null) {
            this.upload = info.Upload;
            Init(upload, "N/A", info.ExitScene, info.ExitScene);
        } else {
            CreateLevelFromJson();
        }
    }

    private void Init(Upload upload, string stage, int sceneOnVictory, int sceneOnExit) {
        DungeonManager dungeon = DungeonManager.Instance;
        DungeonInfo info = DungeonInfo.Instance;

        SerializationUtil.DeserializeDungeonToPlayable(
            upload,
            stage,
            sceneOnVictory,
            sceneOnExit,
            info,
            dungeon.gameObject,
            floorPrefab,
            wallPrefab,
            upstairsPrefab,
            downstairsPrefab,
            goldKeyPrefab,
            blueKeyPrefab,
            redKeyPrefab,
            playerPrefab,
            exitPrefab,
            goldDoorPrefab,
            blueDoorPrefab,
            redDoorPrefab,
            enemyPrefab,
            boosterPrefab);
        Debug.Log("deserialized");
    }
}