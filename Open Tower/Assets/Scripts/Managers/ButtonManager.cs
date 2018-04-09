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
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void GoBack() {
        SceneManager.LoadScene(backScene);
    }
}