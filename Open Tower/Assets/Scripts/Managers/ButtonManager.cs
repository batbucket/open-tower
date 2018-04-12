using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
    private int backScene;

    public void Init(int backScene) {
        this.backScene = backScene;
    }

    public void ResetScene() {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneUtil.LoadScene(scene);
    }

    public void GoBack() {
        SceneUtil.LoadScene(backScene);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ResetScene();
        }
    }
}