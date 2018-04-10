using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryDisplay : MonoBehaviour {
    private static PlayerInventoryDisplay _instance;

    private Inventory player;

    [SerializeField]
    private Text yellow;

    [SerializeField]
    private Text blue;

    [SerializeField]
    private Text red;

    [SerializeField]
    private Image goldIcon;

    [SerializeField]
    private Image blueIcon;

    [SerializeField]
    private Image redIcon;

    public Image GoldIcon {
        get {
            return goldIcon;
        }
    }

    public Image BlueIcon {
        get {
            return blueIcon;
        }
    }

    public Image RedIcon {
        get {
            return redIcon;
        }
    }

    public static PlayerInventoryDisplay Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<PlayerInventoryDisplay>();
            }
            return _instance;
        }
    }

    private void Start() {
        player = Player.Instance.Keys;
    }

    private void Update() {
        this.yellow.text = player.Yellow.ToString();
        this.blue.text = player.Blue.ToString();
        this.red.text = player.Red.ToString();
    }
}