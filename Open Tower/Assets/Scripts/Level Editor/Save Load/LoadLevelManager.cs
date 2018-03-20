using Scripts.LevelEditor.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevelManager : MonoBehaviour {
    private static LoadLevelManager _instance;

    [SerializeField]
    private LoadListing prefab;

    [SerializeField]
    private GameObject window;

    [SerializeField]
    private Transform listingParent;

    [SerializeField]
    private LevelPreview preview;

    [SerializeField]
    private Button load;

    [SerializeField]
    private Button cancel;

    private LoadListing selected;

    public static LoadLevelManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<LoadLevelManager>();
            }
            return _instance;
        }
    }

    public LevelPreview Preview {
        get {
            return preview;
        }
    }

    public LoadListing Selected {
        set {
            this.selected = value;
            preview.gameObject.SetActive(selected != null);
            if (selected != null) {
                preview.Init(JsonUtility.FromJson<Dungeon>(selected.DungeonJson));
            } else {
                preview.Clear();
            }
        }
        get {
            return this.selected;
        }
    }

    public void DoEnter() {
        window.SetActive(true);
        selected = null;
        GameJolt.API.DataStore.GetKeys(false, keys => {
            foreach (string key in keys) {
                LoadListing load = GameObject.Instantiate(prefab, listingParent);
                try {
                    load.Init(key);
                } catch (Exception e) {
                    Destroy(load);
                }
            }
        });
    }

    public void Load() {
        LevelEditorManager.Instance.ImportLevel(selected.DungeonJson);
        Cancel();
    }

    public void Cancel() {
        Util.KillAllChildren(listingParent);
        preview.Clear();
        Selected = null;
        window.SetActive(false);
    }

    private void Update() {
        load.interactable = (selected != null);
    }
}