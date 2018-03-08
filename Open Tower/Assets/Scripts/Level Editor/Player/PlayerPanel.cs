using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : Panel {
    private const int MAX_VALUE = 99999;

    [SerializeField]
    private InputField life;

    [SerializeField]
    private InputField power;

    [SerializeField]
    private InputField defense;

    [SerializeField]
    private InputField stars;

    [SerializeField]
    private InputField yellow;

    [SerializeField]
    private InputField blue;

    [SerializeField]
    private InputField red;

    public int Life {
        get {
            return int.Parse(life.text);
        }
    }

    public int Power {
        get {
            return int.Parse(power.text);
        }
    }

    public int Defense {
        get {
            return int.Parse(defense.text);
        }
    }

    public int Stars {
        get {
            return int.Parse(stars.text);
        }
    }

    public int GoldKeys {
        get {
            return int.Parse(yellow.text);
        }
    }

    public int BlueKeys {
        get {
            return int.Parse(blue.text);
        }
    }

    public int RedKeys {
        get {
            return int.Parse(red.text);
        }
    }

    public void OnLifeChange(string value) {
        AllowPositive(life, value);
    }

    public void OnPowerChange(string value) {
        AllowNonnegative(power, value);
    }

    public void OnDefenseChange(string value) {
        AllowNonnegative(defense, value);
    }

    public void OnStarsChange(string value) {
        AllowNonnegative(stars, value);
    }

    public void OnYellowKeyChange(string value) {
        AllowNonnegative(yellow, value);
    }

    public void OnBlueKeyChange(string value) {
        AllowNonnegative(blue, value);
    }

    public void OnRedKeyChange(string value) {
        AllowNonnegative(red, value);
    }

    private void AllowNonnegative(InputField input, string value) {
        Util.ClampField(input, 0, MAX_VALUE);
    }

    private void AllowPositive(InputField input, string value) {
        Util.ClampField(input, 1, MAX_VALUE);
    }
}