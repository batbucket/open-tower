using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsDisplay : MonoBehaviour {
    private static PlayerStatsDisplay _instance;

    [SerializeField]
    private Color lerpMin;

    [SerializeField]
    private Color lerpMax;

    [SerializeField]
    private float lerpInterval;

    [SerializeField]
    private int lifeIncreaseAmount;

    [SerializeField]
    private int powerAndDefenseIncreaseAmount;

    [SerializeField]
    private Image lifeIcon;

    [SerializeField]
    private Image powerIcon;

    [SerializeField]
    private Image defenseIcon;

    [SerializeField]
    private Image experienceIcon;

    [SerializeField]
    private Text life;

    [SerializeField]
    private Text power;

    [SerializeField]
    private Text defense;

    [SerializeField]
    private Text experience;

    [SerializeField]
    private Button addLife;

    [SerializeField]
    private Button addPower;

    [SerializeField]
    private Button addDefense;

    [SerializeField]
    private AudioClip boostSound;

    private Stats player;

    private Coroutine buttonEnableRoutine;

    private Button[] addButtons;

    public static PlayerStatsDisplay Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<PlayerStatsDisplay>();
            }
            return _instance;
        }
    }

    public Image LifeIcon {
        get {
            return lifeIcon;
        }
    }

    public Image PowerIcon {
        get {
            return powerIcon;
        }
    }

    public Image DefenseIcon {
        get {
            return defenseIcon;
        }
    }

    public Image ExperienceIcon {
        get {
            return experienceIcon;
        }
    }

    public Text Life {
        get {
            return life;
        }
    }

    public Text Power {
        get {
            return power;
        }
    }

    public Text Defense {
        get {
            return defense;
        }
    }

    public Text Experience {
        get {
            return experience;
        }
    }

    public void IncreaseLife() {
        if (player.Experience > 0) {
            player.AddToLife(lifeIncreaseAmount);
            player.AddToExperience(-1);
            SoundManager.Instance.Play(boostSound);
        }
    }

    public void IncreasePower() {
        if (player.Experience > 0) {
            player.AddToPower(powerAndDefenseIncreaseAmount);
            player.AddToExperience(-1);
            SoundManager.Instance.Play(boostSound);
        }
    }

    public void IncreaseDefense() {
        if (player.Experience > 0) {
            player.AddToDefense(powerAndDefenseIncreaseAmount);
            player.AddToExperience(-1);
            SoundManager.Instance.Play(boostSound);
        }
    }

    private void Start() {
        addButtons = new Button[] {
            addLife,
            addPower,
            addDefense
        };
        this.player = Player.Instance.Stats;
    }

    private void Update() {
        this.Life.text = player.Life.ToString();
        this.Power.text = player.Power.ToString();
        this.Defense.text = player.Defense.ToString();
        this.Experience.text = player.Experience.ToString();

        if (buttonEnableRoutine == null
            && player.Experience > 0) {
            buttonEnableRoutine = StartCoroutine(EnableButtonEffects());
        }
        if (buttonEnableRoutine != null && player.Experience <= 0) {
            StopCoroutine(buttonEnableRoutine);
            foreach (Button button in addButtons) {
                button.interactable = false;
            }
            buttonEnableRoutine = null;
        }
    }

    private IEnumerator EnableButtonEffects() {
        foreach (Button button in addButtons) {
            button.interactable = true;
        }
        float timer = 0;
        bool isIncreasing = true;
        while (true) {
            timer += Time.deltaTime;

            if (timer >= lerpInterval) {
                isIncreasing = !isIncreasing;
                timer = 0;
            }
            foreach (Button button in addButtons) {
                ColorBlock cb = button.colors;

                if (isIncreasing) {
                    cb.normalColor = Color.Lerp(lerpMin, lerpMax, timer / lerpInterval);
                } else {
                    cb.normalColor = Color.Lerp(lerpMax, lerpMin, timer / lerpInterval);
                }
                button.colors = cb;
                yield return null;
            }
        }
    }
}