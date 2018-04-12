using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    private Process[] processes;

    private bool isSkipping;

    // Use this for initialization
    private void Start() {
        this.processes = GetComponentsInChildren<Process>();
        StartCoroutine(DoProcesses());
    }

    private IEnumerator DoProcesses() {
        for (int i = 0; i < processes.Length && !isSkipping; i++) {
            Process current = processes[i];
            yield return current.Play();
        }
        SceneUtil.LoadScene(SceneUtil.GetNextSceneIndex());
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isSkipping = true;
            StopAllCoroutines();
            SceneUtil.LoadScene(SceneUtil.GetNextSceneIndex());
        }
    }
}