using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour {

    public void ShowLeaderboards() {
        GameJolt.UI.Manager.Instance.ShowLeaderboards();
    }

    public void ReturnToMainMenu() {
        SceneManager.LoadScene("Main_Menu");
    }
}