using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour {
    private static LevelInfo _instance;

    [SerializeField]
    private string json;

    [SerializeField]
    private string exitScene;

    [SerializeField]
    private bool isLevelCleared;

    public static LevelInfo Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<LevelInfo>();
            }
            return _instance;
        }
    }

    public string JSON {
        get {
            return json;
        }
    }

    public string ExitScene {
        get {
            return exitScene;
        }
    }

    public bool IsLevelCleared {
        get {
            return isLevelCleared;
        }

        set {
            isLevelCleared = value;
        }
    }

    public void Init(string json, string exitScene) {
        this.json = json;
        this.exitScene = exitScene;
    }
}