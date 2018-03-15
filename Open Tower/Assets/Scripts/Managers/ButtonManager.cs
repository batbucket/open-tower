using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
    private string backScene;

    public void Init(string backScene) {
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