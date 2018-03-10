using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSubmissionManager : MonoBehaviour {

    [SerializeField]
    private InputField nameField;

    [SerializeField]
    private Text nameAvailable;

    [SerializeField]
    private GameObject window;

    public void CheckNameUniqueness() {
        string nameToCheck = nameField.text;
        GameJolt.API.DataStore.GetKeys(true, keys => {
            HashSet<string> keySet = new HashSet<string>(keys);
            if (keySet.Contains(nameToCheck)) {
                SetWarning("Name taken.", Color.red);
            } else {
                SetWarning("Name available!", Color.green);
            }
        });
        // cool checking happens here
    }

    public void SubmitLevel() {
        // Hello world and Hello world_  (_ is a space) don't get saved correctly
        string nameToCheck = nameField.text;
        GameJolt.API.DataStore.GetKeys(true, keys => {
            HashSet<string> keySet = new HashSet<string>(keys);
            if (keySet.Contains(nameToCheck)) {
                SetWarning("Name taken.", Color.red);
            } else {
                GameJolt.API.DataStore.Set(nameToCheck, LevelInfo.Instance.JSON, true, isSuccess => {
                    Debug.Log("isSuccess=" + isSuccess);
                    if (isSuccess) {
                        ReturnToEditor();
                    }
                });
            }
        });
    }

    public void OpenWindow() {
        window.SetActive(true);
    }

    public void ReturnToEditor() {
        window.SetActive(false);
    }

    private void Start() {
        SetWarning("null", Color.clear);
    }

    private void SetWarning(string warning, Color color) {
        nameAvailable.text = warning;
        nameAvailable.color = color;
    }
}