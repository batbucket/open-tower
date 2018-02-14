using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsManager : MonoBehaviour {
    private static ResultsManager _instance;

    public static ResultsManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<ResultsManager>();
            }
            return _instance;
        }
    }

    [SerializeField]
    private GameObject window;

    private string destination;

    public void ShowResults(string destination) {
        window.SetActive(true);
        this.destination = destination;
    }

    public void ChangeScene() {
        SceneManager.LoadScene(destination);
    }
}