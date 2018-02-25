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
    private Panel[] allPanels;

    public Element Selected;

    public void SetTab(int modeIndex) {
        SetTab((TabMode)modeIndex);
    }

    public void SetTab(TabMode mode) {
        foreach (Panel panel in allPanels) {
            panel.SetActive(false);
            if (panel.Mode == mode) {
                panel.SetActive(true);
            }
        }
        this.current = mode;
    }

    private void Start() {
        SetTab(current);
        floorsPanel.AddFloor();
    }

    private void Update() {
    }
}