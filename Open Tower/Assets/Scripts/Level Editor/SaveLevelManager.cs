using Scripts.LevelEditor.Serialization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SaveLevelManager : MonoBehaviour {

    private enum Mode {
        SAVE_NEW, OVERWRITE
    }

    [SerializeField]
    private GameObject window;

    [SerializeField]
    private InputField field;

    [SerializeField]
    private Text warning;

    [SerializeField]
    private Button save;

    [SerializeField]
    private Text button;

    private Mode _mode;

    private Mode Save {
        set {
            _mode = value;
            button.text = (_mode == Mode.SAVE_NEW) ? "Save" : "Overwrite";
        }
        get {
            return _mode;
        }
    }

    private bool CanSave {
        get {
            return save.IsInteractable();
        }
        set {
            save.interactable = value;
        }
    }

    public void OnEditField(string newText) {
        Save = Mode.SAVE_NEW;
    }

    public void SaveLevel() {
        string levelName = field.text;
        CanSave = false;
        if (Save == Mode.SAVE_NEW) {
            warning.text = "Attempting to save...";
            GameJolt.API.DataStore.GetKeys(false, keys => {
                if (keys.Contains(levelName)) {
                    warning.text = "<color=red>Duplicate level name found.\nWould you like to overwrite?</color>";
                    button.text = "Overwrite";
                    Save = Mode.OVERWRITE;
                }
                CanSave = true;
            });
        } else {
            warning.text = "Attempting to overwrite...";
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
                    CanSave = true;
                });
        }
    }

    public void Cancel() {
        CanSave = true;
    }
}