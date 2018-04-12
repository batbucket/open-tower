using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonInfo : MonoBehaviour {
    private static DungeonInfo _instance;

    [SerializeField]
    private Text stage;

    [SerializeField]
    private Text location;

    [SerializeField]
    private ButtonManager buttons;

    public string LevelName {
        get {
            return location.text;
        }
    }

    public static DungeonInfo Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<DungeonInfo>();
            }
            return _instance;
        }
    }

    public void Init(string stage, string location, int backScene) {
        this.stage.text = stage;
        this.location.text = location;
        this.buttons.Init(backScene);
        SetupIfIsOfficialLevel();
    }

    private void Start() {
        SetupIfIsOfficialLevel();
    }

    private void SetupIfIsOfficialLevel() {
        if (SceneUtil.IsLevelIndex) {
            LevelParams level = SceneUtil.GetParams(SceneUtil.LevelIndex);
            this.stage.text = string.Format("{0}-{1}", level.WorldIndex, level.StageIndex);
            this.location.text = level.Name;
        }
    }
}