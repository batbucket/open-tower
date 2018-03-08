using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour {
    private static LevelInfo _instance;
    private string json;
    private string exitScene;

    public static LevelInfo Instance {
        get {
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

    public void Init(string json, string exitScene) {
        this.json = json;
        this.exitScene = exitScene;
    }

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            DestroyImmediate(gameObject);
        }
    }
}