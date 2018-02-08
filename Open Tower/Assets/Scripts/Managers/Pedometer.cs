using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pedometer : MonoBehaviour {

    [SerializeField]
    private Text steps;

    private void Update() {
        this.steps.text = Player.Instance.Stats.StepCount.ToString();
    }
}