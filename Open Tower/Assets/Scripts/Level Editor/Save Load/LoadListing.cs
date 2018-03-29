using Scripts.LevelEditor.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadListing : MonoBehaviour {

    [SerializeField]
    private Outline outline;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Text text;

    [SerializeField]
    private Button select;

    private string json;

    public string DungeonJson {
        get {
            return json;
        }
    }

    private string key;

    public void Init(string key) {
        this.key = key;
        GameJolt.API.DataStore.Get(key, false, value => {
            json = value;
            text.text = string.Format(key);
        });
    }

    public void Select() {
        LoadLevelManager.Instance.Selected = this;
    }

    public void Delete() {
        LoadLevelManager.Instance.Selected = null;
        select.interactable = false;
        GameJolt.API.DataStore.Delete(key, false, isSuccess => {
            if (isSuccess) {
                Destroy(this.gameObject);
            }
        });
    }

    private void Update() {
        outline.effectColor = (LoadLevelManager.Instance.Selected == this) ? Color.grey : Color.black;
    }
}