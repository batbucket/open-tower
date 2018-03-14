using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBrowserManager : MonoBehaviour {

    [SerializeField]
    private LevelListing levelListingPrefab;

    [SerializeField]
    private Transform levelList;

    public void GoToScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    private void Start() {
        GameJolt.API.DataStore.GetKeys(true, keys => {
            foreach (string key in keys) {
                Debug.Log("trying to parse: " + key);
                Instantiate<LevelListing>(levelListingPrefab, levelList).Init(key);
            }
        });
    }
}