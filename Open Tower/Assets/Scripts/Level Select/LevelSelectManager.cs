using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour {

    [SerializeField]
    private GameObject levelParent;

    [SerializeField]
    private SelectableLevel prefab;

    private void Start() {
        for (int i = SceneUtil.LEVEL_START_INDEX; i <= SceneUtil.LEVEL_END_INDEX; i++) {
            Instantiate(prefab, levelParent.transform).Init();
        }
    }

    public void ShowLeaderboards() {
        GameJolt.UI.Manager.Instance.ShowLeaderboards();
    }

    public void ReturnToMainMenu() {
        SceneManager.LoadScene("Main_Menu");
    }
}