using UnityEngine;
using UnityEngine.UI;

public class Toggle : MonoBehaviour {
    public const string BATTLE_KEY = "battle";
    public const string STATS_KEY = "stats";

    [SerializeField]
    private ToggleType type;

    [SerializeField]
    private Image background;

    [SerializeField]
    private Text button;

    private string Key {
        get {
            string key = string.Empty;
            switch (type) {
                case ToggleType.BATTLE:
                    key = BATTLE_KEY;
                    break;

                case ToggleType.STATS:
                    key = STATS_KEY;
                    break;
            }
            return key;
        }
    }

    private bool IsEnabled {
        get {
            return Util.GetBool(Key);
        }
    }

    public void ToggleMode() {
        bool isEnabled = !Util.GetBool(Key);
        Util.SetBool(Key, isEnabled);
        UpdateDisplay();
    }

    private void Start() {
        UpdateDisplay();
    }

    private void UpdateDisplay() {
        Color bg = Color.white;
        string text = string.Empty;
        if (IsEnabled) {
            bg = Color.green;
            text = "ON";
        } else {
            bg = Color.red;
            text = "OFF";
        }
        background.color = bg;
        button.text = text;
    }
}