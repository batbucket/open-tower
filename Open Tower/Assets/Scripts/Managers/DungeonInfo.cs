using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonInfo : MonoBehaviour {
    private static DungeonInfo _instance;

    [SerializeField]
    private Text stage;

    [SerializeField]
    private Text location;

    [SerializeField]
    private ButtonManager buttons;

    public static DungeonInfo Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<DungeonInfo>();
            }
            return _instance;
        }
    }

    public void Init(string stage, string location, string backScene) {
        this.stage.text = stage;
        this.location.text = location;
        this.buttons.Init(backScene);
    }
}