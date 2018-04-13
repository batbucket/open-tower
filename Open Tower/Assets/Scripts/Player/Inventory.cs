using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    [SerializeField]
    private int yellow;

    [SerializeField]
    private int blue;

    [SerializeField]
    private int red;

    [SerializeField]
    private Splat prefab;

    public int Yellow {
        get {
            return yellow;
        }
        set {
            AssertKeyParameterIsValid(value);
            ChangeEffect(yellow, value, PlayerInventoryDisplay.Instance.Yellow);
            this.yellow = value;
        }
    }

    public int Blue {
        get {
            return blue;
        }
        set {
            AssertKeyParameterIsValid(value);
            ChangeEffect(blue, value, PlayerInventoryDisplay.Instance.Blue);
            this.blue = value;
        }
    }

    public int Red {
        get {
            return red;
        }
        set {
            AssertKeyParameterIsValid(value);
            ChangeEffect(red, value, PlayerInventoryDisplay.Instance.Red);
            this.red = value;
        }
    }

    public void Init(int gold, int blue, int red) {
        this.yellow = gold;
        this.blue = blue;
        this.red = red;
        AssertValuesAreValid();
    }

    private void AssertKeyParameterIsValid(int value) {
        Util.Assert(value >= 0, "Invalid parameter {0}", value);
    }

    private void Start() {
        AssertValuesAreValid();
    }

    private void AssertValuesAreValid() {
        AssertKeyParameterIsValid(yellow);
        AssertKeyParameterIsValid(blue);
        AssertKeyParameterIsValid(red);
    }

    private void ChangeEffect(int current, int value, Text text) {
        int amount = value - current;
        if (amount != 0) {
            StartCoroutine(Util.ValueChange(
                amount,
                new Transform[] { text.transform },
                new Action<Color>[] { t => text.color = t })
                );
            Instantiate(prefab).Init(amount, text.transform);
        }
    }
}