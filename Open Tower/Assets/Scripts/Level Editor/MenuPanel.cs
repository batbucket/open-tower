using Scripts.LevelEditor.Serialization;
using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPanel : Panel {
    private const string SUCCESS = "lime";
    private const string FAILURE = "red";
    private const string PLAYER_CHECK = "<color={0}>Player in level</color>\n";
    private const string STAIRS_CHECK = "<color={0}>Stairs valid</color>\n";
    private const string EXIT_CHECK = "<color={0}>Exit in level</color>";

    private static MenuPanel _instance;

    [SerializeField]
    private Text error;

    [SerializeField]
    private Transform floorsParent;

    [SerializeField]
    private Button playtest;

    [SerializeField]
    private EntitiesPanel entities;

    [SerializeField]
    private PlayerPanel player;

    [SerializeField]
    private FloorPanel floor;

    [SerializeField]
    private ImportExportLevelManager export;

    [SerializeField]
    private ImportExportLevelManager import;

    [SerializeField]
    private LoadLevelManager load;

    [SerializeField]
    private SaveLevelManager save;

    private bool isLevelValidated;

    public static MenuPanel Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<MenuPanel>();
            }
            return _instance;
        }
    }

    public void StartPlaytest() {
        string json = SerializationUtil.GetSerializedDungeon(floorsParent, entities, player);
        LevelInfo.Instance.Init(
                LevelInfoMode.PLAY_TEST,
                new Upload(
                json,
                "Playtest",
                GameJolt.API.Manager.Instance.CurrentUser.ID,
                string.Empty),
            SceneUtil.LEVEL_SELECT_INDEX,
            SceneUtil.LEVEL_SELECT_INDEX);
        SceneManager.LoadScene("Custom_Level");
    }

    public void OpenImport() {
        import.DoEnter();
    }

    public void OpenExport() {
        export.DoEnter();
    }

    public void OpenSave() {
        save.DoEnter();
    }

    public void OpenLoad() {
        load.DoEnter();
    }

    public void GoToScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public override void OnEnter() {
        isLevelValidated = false;
        floor.SetFloorEditability(false);
        CheckIfLevelIsValid();
    }

    public override void OnExit() {
        floor.SetFloorEditability(true);
    }

    private bool IsStairsValid() {
        return floorsParent
            .GetComponentsInChildren<StairsElement>(true)
            .All(stairs => stairs.IsValid);
    }

    private bool IsPlayerPlaced() {
        return floorsParent.GetComponentsInChildren<Element>(true).Any(element => element.IsType(TileType.PLAYER));
    }

    private bool IsExitPlaced() {
        return floorsParent.GetComponentsInChildren<Element>(true).Any(element => element.IsType(TileType.EXIT));
    }

    private void Update() {
        playtest.interactable = isLevelValidated;
    }

    public override void Clear() {
    }

    private void CheckIfLevelIsValid() {
        bool isPlayerPlaced = IsPlayerPlaced();
        bool isStairsValid = IsStairsValid();
        bool isExitPlaced = IsExitPlaced();

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(PLAYER_CHECK, isPlayerPlaced ? SUCCESS : FAILURE);
        sb.AppendFormat(STAIRS_CHECK, isStairsValid ? SUCCESS : FAILURE);
        sb.AppendFormat(EXIT_CHECK, isExitPlaced ? SUCCESS : FAILURE);

        error.text = sb.ToString();
        isLevelValidated = isPlayerPlaced && isStairsValid && isExitPlaced;
    }
}