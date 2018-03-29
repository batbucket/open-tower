using Scripts.LevelEditor.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelEditorManager : MonoBehaviour {
    private static LevelEditorManager _instance;

    public static LevelEditorManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<LevelEditorManager>();
            }
            return _instance;
        }
    }

    [SerializeField]
    private TabMode current;

    [SerializeField]
    private EntitiesPanel entitiesPanel;

    [SerializeField]
    private PlayerPanel playerPanel;

    [SerializeField]
    private FloorPanel floorsPanel;

    [SerializeField]
    private Panel menuPanel;

    [SerializeField]
    private LevelSubmissionManager levelSubmission;

    [SerializeField]
    private Panel[] allPanels;

    [SerializeField]
    private GameObject boosterPrefab;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject floorPrefab;

    [SerializeField]
    private GameObject floorListingPrefab;

    [SerializeField]
    private GameObject elementPrefab;

    [SerializeField]
    private GameObject upStairsPrefab;

    [SerializeField]
    private GameObject downStairsPrefab;

    public Element Selected;

    public string DungeonJson {
        get {
            return SerializationUtil.GetSerializedDungeon(floorsPanel.FloorParent, entitiesPanel, playerPanel);
        }
    }

    public void SetTab(int modeIndex) {
        SetTab((TabMode)modeIndex);
    }

    public void SetTab(TabMode mode) {
        foreach (Panel panel in allPanels) {
            panel.SetActive(false);
            if (panel.Mode == current) {
                panel.OnExit();
            }
            if (panel.Mode == mode) {
                panel.OnEnter();
                panel.SetActive(true);
            }
        }
        this.current = mode;
    }

    public void ImportLevel(string levelJson) {
        foreach (Panel panel in allPanels) {
            panel.Clear();
        }
        SerializationUtil.DeserializeDungeonToEditor(
            levelJson,
            entitiesPanel,
            boosterPrefab,
            enemyPrefab,
            playerPanel,
            floorsPanel,
            floorListingPrefab,
            floorPrefab,
            elementPrefab,
            upStairsPrefab,
            downStairsPrefab);
    }

    private void Start() {
        SetTab(current);

        LevelInfo info = LevelInfo.Instance;
        // if playtest mode not set, this level info can be from the level browser
        // if you play a level browser game and then enter the level editor it'll mistakenly parse the browser game's json
        if (info.Mode == LevelInfoMode.PLAY_TEST && !string.IsNullOrEmpty(info.Upload.LevelJson)) {
            ImportLevel(info.Upload.LevelJson);
        }
        if (info.IsLevelCleared) {
            info.IsLevelCleared = false;
            levelSubmission.OpenWindow();
        }
    }

    private void Update() {
    }
}