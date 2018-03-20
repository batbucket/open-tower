using Scripts.LevelEditor.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SaveLevelManager : MonoBehaviour {

    [SerializeField]
    private GameObject window;

    [SerializeField]
    private InputField field;

    [SerializeField]
    private Text warning;

    [SerializeField]
    private Button save;

    [SerializeField]
    private Button cancel;

    [SerializeField]
    private Text button;

    private HashSet<string> existingLevels;

    private bool isInitializing;

    public void DoEnter() {
        Util.FocusOnField(field);
        window.SetActive(true);
        isInitializing = true;
        save.interactable = false;
        GameJolt.API.DataStore.GetKeys(false, keys => {
            this.existingLevels = new HashSet<string>(keys);
            isInitializing = false;
            save.interactable = true;
        });
    }

    public void OnEditField(string newText) {
        string trimmed = newText.Trim();
        bool isLevelNameFilled = !string.IsNullOrEmpty(field.text);
        bool isLevelNameUnique = !existingLevels.Contains(trimmed);
        bool isValidLevelName = isLevelNameFilled && isLevelNameUnique;
        save.interactable = isValidLevelName;
        save.interactable = !isInitializing && isValidLevelName;
        if (!isLevelNameFilled) {
            warning.text = "<color=red>Level name cannot be empty.</color>";
        } else if (!isLevelNameUnique) {
            warning.text = "<color=red>Level name is taken.</color>";
        } else {
            warning.text = "<color=lime>Level name available!</color>";
        }
        field.text = trimmed;
    }

    public void SaveLevel() {
        string levelName = field.text;
        save.interactable = false;
        field.interactable = false;
        cancel.interactable = false;
        GameJolt.API.DataStore.Set(
            levelName,
            LevelEditorManager.Instance.DungeonJson,
            false,
            isSuccess => {
                if (isSuccess) {
                    window.SetActive(false);
                } else {
                    button.text = "<color=red>An unknown error occurred.</color>";
                }
                save.interactable = true;
                field.interactable = true;
                cancel.interactable = true;
            });
    }

    public void Cancel() {
        window.SetActive(false);
    }
}