using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelBrowserManager : MonoBehaviour {
    private static LevelBrowserManager _instance;

    [SerializeField]
    private LevelListing levelListingPrefab;

    [SerializeField]
    private Transform levelList;

    [SerializeField]
    private InspectorManager inspector;

    [SerializeField]
    private Button playLevel;

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

    public void PlaySelectedLevel() {
        if (selected != null) {
            int currentUserID = GameJolt.API.Manager.Instance.CurrentUser.ID;
            if (currentUserID != selected.Upload.AuthorID) {
                selected.Upload.AttemptedUserIds.Add(currentUserID);
            }
            LevelInfo.Instance.Init(LevelInfoMode.USER_GENERATED_LEVEL, selected.Upload, "Level_Browser");
            SceneManager.LoadScene("Custom_Level");
        }
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

    private void Update() {
        playLevel.interactable = (selected != null);
    }
}