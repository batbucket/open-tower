using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelSubmissionManager : MonoBehaviour {

    [SerializeField]
    private float warningFadeWaitDuration = 5f;

    [SerializeField]
    private float warningFadeDuration = 2f;

    [SerializeField]
    private FloorPanel floor;

    [SerializeField]
    private InputField nameField;

    [SerializeField]
    private Text nameAvailable;

    [SerializeField]
    private GameObject window;

    [SerializeField]
    private TextAsset badWords;

    private HashSet<string> badWordSet;
    private Coroutine warningRoutine;

    public void CheckNameUniqueness() {
        CheckName();
    }

    public void FormatName(string name) {
        if (!string.IsNullOrEmpty(name)) {
            nameField.text = name.Trim();
        }
    }

    public void SubmitLevel() {
        string nameToCheck = nameField.text;
        Debug.Log(nameToCheck);
        GameJolt.API.DataStore.GetKeys(true, keys => {
            CheckName(() => {
                GameJolt.API.DataStore.Set(nameToCheck, LevelInfo.Instance.JSON, true, isSuccess => {
                    if (isSuccess) {
                        ReturnToEditor();
                    } else {
                        SetWarning("Unknown error occurred.", false);
                    }
                });
            });
        });
    }

    public void OpenWindow() {
        floor.SetFloorEditability(false);
        window.SetActive(true);
    }

    public void ReturnToEditor() {
        floor.SetFloorEditability(true);
        window.SetActive(false);
    }

    private void CheckName(Action onAvailable = null) {
        string nameToCheck = nameField.text;

        string warning = string.Empty;
        bool isGreen = false;
        if (string.IsNullOrEmpty(nameToCheck)) {
            warning = "Name is too short.";
        } else if (IsNameContainsBadWords()) {
            warning = "Name contains forbidden words.";
        } else {
            GameJolt.API.DataStore.GetKeys(true, keys => {
                HashSet<string> keySet = new HashSet<string>(keys);
                if (keySet.Contains(nameToCheck)) {
                    warning = "Name taken.";
                } else {
                    warning = "Name available!";
                    isGreen = true;
                    if (onAvailable != null) {
                        onAvailable();
                    }
                }
            });
        }

        SetWarning(warning, isGreen);
    }

    private void Start() {
        SetWarning("", true);
        SetupBadWords();
    }

    private bool IsNameContainsBadWords() {
        string nameToCheck = nameField.text;
        return badWordSet.Any(nameToCheck.Contains);
    }

    private void SetupBadWords() {
        badWordSet = new HashSet<string>(badWords.text.Split('\n'));
    }

    private void SetWarning(string warning, bool isGreen) {
        nameAvailable.text = warning;
        nameAvailable.color = isGreen ? Color.green : Color.red;
        if (warningRoutine != null) {
            StopCoroutine(warning);
        }
        warningRoutine = StartCoroutine(WarningFade());
    }

    private IEnumerator WarningFade() {
        float timer = 0;
        Color startColor = nameAvailable.color;
        yield return new WaitForSeconds(warningFadeWaitDuration);
        while ((timer += Time.deltaTime) < warningFadeDuration) {
            nameAvailable.color = Color.Lerp(startColor, Color.clear, timer / warningFadeDuration);
            yield return null;
        }
    }
}