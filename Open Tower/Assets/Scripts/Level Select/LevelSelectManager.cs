using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour {

    [SerializeField]
    private GameObject levelParent;

    private void Start() {
        int numberOfLevels = levelParent.GetComponentsInChildren<SelectableLevel>().Length;
        Util.Assert(numberOfLevels == SceneUtil.NUMBER_OF_LEVELS, "Expected {0} levels, got {1} instead", SceneUtil.NUMBER_OF_LEVELS, numberOfLevels);
    }

    public void ShowLeaderboards() {
        GameJolt.UI.Manager.Instance.ShowLeaderboards();
    }

    public void ReturnToMainMenu() {
        SceneManager.LoadScene("Main_Menu");
    }
}