using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    [SerializeField]
    private Button signIn;

    [SerializeField]
    private Button[] requiresSignIn;

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

    private void Start() {
        LevelInfo info = FindObjectOfType<LevelInfo>();
        if (info != null) {
            Destroy(info.gameObject);
        }
    }

    private void Update() {
        bool isUserAuthenticated = GameJolt.API.Manager.Instance.CurrentUser != null
            && GameJolt.API.Manager.Instance.CurrentUser.IsAuthenticated;
        signIn.gameObject.SetActive(!isUserAuthenticated);
        foreach (Button button in requiresSignIn) {
            button.gameObject.SetActive(isUserAuthenticated);
        }
    }
}