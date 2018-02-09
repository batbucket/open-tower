using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public void GoToLevel(string levelName) {
        SceneManager.LoadScene(levelName);
    }

    public void SignIntoGameJolt() {
        GameJolt.UI.Manager.Instance.ShowSignIn(
          (bool signInSuccess) => {
              Debug.Log(string.Format("Sign-in {0}", signInSuccess ? "successful" : "failed or user's dismissed the window"));
          },
          (bool userFetchedSuccess) => {
              Debug.Log(string.Format("User details fetched {0}", userFetchedSuccess ? "successfully" : "failed"));
          });
    }
}