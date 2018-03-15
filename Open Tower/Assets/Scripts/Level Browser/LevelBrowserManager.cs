using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBrowserManager : MonoBehaviour {
    private static LevelBrowserManager _instance;

    [SerializeField]
    private LevelListing levelListingPrefab;

    [SerializeField]
    private Transform levelList;

    [SerializeField]
    private InspectorManager inspector;

    private LevelListing selected;

    public static LevelBrowserManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<LevelBrowserManager>();
            }
            return _instance;
        }
    }

    public bool IsLevelBeingSelected(LevelListing listingToCheck) {
        return selected == listingToCheck;
    }

    public void SelectLevel(LevelListing listingToSelect) {
        this.selected = listingToSelect;
        inspector.SetLevel(selected.Upload);
    }

    public void GoToScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    private void Start() {
        GameJolt.API.DataStore.GetKeys(true, keys => {
            StartCoroutine(InitLevelListing(keys));
        });
    }

    private IEnumerator InitLevelListing(string[] keys) {
        foreach (string key in keys) {
            string levelJson = string.Empty;
            GameJolt.API.DataStore.Get(key, true, json => {
                levelJson = json;
            });
            yield return new WaitWhile(() => string.IsNullOrEmpty(levelJson));
            Instantiate(levelListingPrefab, levelList).Init(levelJson);
        }
    }
}