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

    public Element Selected;

    private void Update() {
    }
}