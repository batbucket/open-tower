using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

    [SerializeField]
    private int life;

    [SerializeField]
    private int power;

    [SerializeField]
    private int defense;

    [SerializeField]
    private int experience;

    [SerializeField]
    private Splat prefab;

    private int steps;

    public int Life {
        get {
            return life;
        }
    }

    public int Power {
        get {
            return power;
        }
    }

    public int Defense {
        get {
            return defense;
        }
    }

    public int Experience {
        get {
            return experience;
        }
    }

    public int StepCount {
        get {
            return steps;
        }
    }

    public void Init(int life, int power, int defense, int experience) {
        this.life = life;
        this.power = power;
        this.defense = defense;
        this.experience = experience;
        AssertValuesAreValid();
    }

    public void AddToLife(int amount) {
        this.life += amount;
        StartCoroutine(Util.ValueChange(
            amount,
            new Transform[] { PlayerStatsDisplay.Instance.Life.transform },
            new Action<Color>[] { t => PlayerStatsDisplay.Instance.Life.color = t })
            );
        if (amount != 0) {
            Instantiate(prefab).Init(amount, PlayerStatsDisplay.Instance.Life.transform);
        }
    }

    public void AddToPower(int amount) {
        this.power += amount;
        StartCoroutine(Util.ValueChange(
            amount,
            new Transform[] { PlayerStatsDisplay.Instance.Power.transform },
            new Action<Color>[] { t => PlayerStatsDisplay.Instance.Power.color = t })
            );
        if (amount != 0) {
            Instantiate(prefab).Init(amount, PlayerStatsDisplay.Instance.Power.transform);
        }
    }

    public void AddToDefense(int amount) {
        this.defense += amount;
        StartCoroutine(Util.ValueChange(
            amount,
            new Transform[] { PlayerStatsDisplay.Instance.Defense.transform },
            new Action<Color>[] { t => PlayerStatsDisplay.Instance.Defense.color = t })
            );
        if (amount != 0) {
            Instantiate(prefab).Init(amount, PlayerStatsDisplay.Instance.Defense.transform);
        }
    }

    public void AddToExperience(int amount) {
        this.experience += amount;
        StartCoroutine(Util.ValueChange(
            amount,
            new Transform[] { PlayerStatsDisplay.Instance.Experience.transform },
            new Action<Color>[] { t => PlayerStatsDisplay.Instance.Experience.color = t })
            );
        if (amount != 0) {
            Instantiate(prefab).Init(amount, PlayerStatsDisplay.Instance.Experience.transform);
        }
    }

    public void IncrementSteps() {
        this.steps++;
    }

    private void Start() {
        AssertValuesAreValid();
    }

    private void AssertValuesAreValid() {
        Util.Assert(Life > 0, "Life must be positive number.");
        Util.Assert(Power >= 0, "Power must be nonnegative.");
        Util.Assert(Defense >= 0, "Defense must be nonnegative.");
        Util.Assert(Experience >= 0, "Experience must be nonnegative.");
    }
}