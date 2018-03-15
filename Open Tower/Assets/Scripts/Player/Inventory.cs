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

    public int Yellow {
        get {
            return yellow;
        }
        set {
            AssertKeyParameterIsValid(value);
            this.yellow = value;
        }
    }

    public int Blue {
        get {
            return blue;
        }
        set {
            AssertKeyParameterIsValid(value);
            this.blue = value;
        }
    }

    public int Red {
        get {
            return red;
        }
        set {
            AssertKeyParameterIsValid(value);
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
}