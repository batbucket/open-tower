using Scripts.LevelEditor.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deserializer : MonoBehaviour {
    private string json;

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

    private void Awake() {
        LevelInfo info = LevelInfo.Instance;
        Init(info.JSON, info.ExitScene);
    }

    private void Init(string json, string exitScene) {
        DontDestroyOnLoad(this.gameObject);
        DungeonManager dungeon = DungeonManager.Instance;
        DungeonInfo info = DungeonInfo.Instance;

        SerializationUtil.DeserializeDungeonToPlayable(
            json,
            exitScene,
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
    }
}