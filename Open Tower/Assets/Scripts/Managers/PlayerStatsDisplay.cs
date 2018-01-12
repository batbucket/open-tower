using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsDisplay : MonoBehaviour {

    [SerializeField]
    private Stats player;

    [SerializeField]
    private Text life;

    [SerializeField]
    private Text power;

    [SerializeField]
    private Text defense;

    [SerializeField]
    private Text experience;

    private void Update() {
        this.life.text = player.Life.ToString();
        this.power.text = player.Power.ToString();
        this.defense.text = player.Defense.ToString();
        this.experience.text = player.Experience.ToString();
    }
}