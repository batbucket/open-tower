using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    [SerializeField]
    private string destinationScene;

    private Process[] processes;

    // Use this for initialization
    private void Start() {
        this.processes = GetComponentsInChildren<Process>();
        StartCoroutine(DoProcesses());
    }

    private IEnumerator DoProcesses() {
        for (int i = 0; i < processes.Length; i++) {
            Process current = processes[i];
            yield return current.Play();
        }
        SceneManager.LoadScene(destinationScene);
    }
}