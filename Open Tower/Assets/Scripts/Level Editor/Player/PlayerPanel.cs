using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : Panel {
    private const int MAX_VALUE = 99999;
    private static PlayerPanel _instance;

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

    public static PlayerPanel Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<PlayerPanel>();
            }
            return _instance;
        }
    }

    public int Life {
        get {
            return int.Parse(life.text);
        }
        private set {
            life.text = value.ToString();
        }
    }

    public int Power {
        get {
            return int.Parse(power.text);
        }
        private set {
            power.text = value.ToString();
        }
    }

    public int Defense {
        get {
            return int.Parse(defense.text);
        }
        private set {
            defense.text = value.ToString();
        }
    }

    public int Stars {
        get {
            return int.Parse(stars.text);
        }
        private set {
            stars.text = value.ToString();
        }
    }

    public int GoldKeys {
        get {
            return int.Parse(yellow.text);
        }
        private set {
            yellow.text = value.ToString();
        }
    }

    public int BlueKeys {
        get {
            return int.Parse(blue.text);
        }
        private set {
            blue.text = value.ToString();
        }
    }

    public int RedKeys {
        get {
            return int.Parse(red.text);
        }
        private set {
            red.text = value.ToString();
        }
    }

    public void Init(int life, int power, int defense, int stars, int goldKeys, int blueKeys, int redKeys) {
        this.Life = life;
        this.Power = power;
        this.Defense = defense;
        this.Stars = stars;
        this.GoldKeys = goldKeys;
        this.BlueKeys = blueKeys;
        this.RedKeys = redKeys;
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

    public override void Clear() {
        life.text = string.Empty;
        power.text = string.Empty;
        defense.text = string.Empty;
        stars.text = string.Empty;

        yellow.text = string.Empty;
        blue.text = string.Empty;
        red.text = string.Empty;
    }
}