using Scripts.LevelEditor.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour {
    private static LevelInfo _instance;

    [SerializeField]
    private LevelInfoMode mode;

    [SerializeField]
    private Upload upload;

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

    public LevelInfoMode Mode {
        get {
            return mode;
        }
    }

    public Upload Upload {
        get {
            return upload;
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

    public void Init(LevelInfoMode mode, Upload upload, string exitScene) {
        this.mode = mode;
        this.upload = upload;
        this.exitScene = exitScene;
    }
}