using Scripts.LevelEditor.Serialization;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImportExportLevelManager : MonoBehaviour {

    public enum Mode {
        IMPORT, EXPORT
    }

    public enum InteractionType {
        ENTER, LEAVE, DO_IMPORT
    }

    [SerializeField]
    private Mode mode;

    [SerializeField]
    private GameObject window;

    [SerializeField]
    private InputField jsonField;

    [SerializeField]
    private Button import;

    public void DoEnter() {
        DoInteraction(InteractionType.ENTER);
        Util.FocusOnField(jsonField);
    }

    public void DoLeave() {
        DoInteraction(InteractionType.LEAVE);
    }

    public void DoImport() {
        DoInteraction(InteractionType.DO_IMPORT);
    }

    private void DoInteraction(InteractionType interaction) {
        window.SetActive(interaction == InteractionType.ENTER);
        if (interaction == InteractionType.ENTER) {
            if (mode == Mode.EXPORT) {
                jsonField.text = SerializationUtil.GetSerializedDungeon(FloorPanel.Instance.FloorParent, EntitiesPanel.Instance, PlayerPanel.Instance);
            }
        } else if (interaction == InteractionType.LEAVE) {
            jsonField.text = string.Empty;
        } else if (interaction == InteractionType.DO_IMPORT) {
            try {
                LevelEditorManager.Instance.ImportLevel(jsonField.text);
            } catch (Exception e) {
                Debug.Log("invalid json detected");
                Debug.Log(e);
                window.SetActive(true);
            }
        }
    }
}