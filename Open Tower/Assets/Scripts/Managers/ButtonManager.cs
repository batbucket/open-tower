using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    public void ResetScene() {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneUtil.LoadScene(scene);
    }

    public void GoBack() {
        SceneUtil.LoadScene(SceneUtil.GetExitSceneIndex());
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ResetScene();
        }
    }
}