using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryDisplay : MonoBehaviour {
    private Inventory player;

    [SerializeField]
    private Text yellow;

    [SerializeField]
    private Text blue;

    [SerializeField]
    private Text red;

    private void Start() {
        player = Player.Instance.Keys;
    }

    private void Update() {
        this.yellow.text = player.Yellow.ToString();
        this.blue.text = player.Blue.ToString();
        this.red.text = player.Red.ToString();
    }
}