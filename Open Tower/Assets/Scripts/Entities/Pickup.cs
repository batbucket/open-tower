using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : Entity {

    [SerializeField]
    private PickupType pickup;

    [SerializeField]
    private int amount;

    private new SpriteRenderer renderer;

    private bool isUsed;

    public PickupType Type {
        get {
            return pickup;
        }
    }

    public int Amount {
        get {
            return amount;
        }
    }

    public void Init(StatType type, int amount) {
        PickupType pickup = PickupType.BLUE_KEY;
        switch (type) {
            case StatType.LIFE:
                pickup = PickupType.LIFE;
                break;

            case StatType.POWER:
                pickup = PickupType.POWER;
                break;

            case StatType.DEFENSE:
                pickup = PickupType.DEFENSE;
                break;

            case StatType.EXPERIENCE:
                pickup = PickupType.EXPERIENCE;
                break;
        }
        this.pickup = pickup;
        this.amount = amount;
    }

    protected override bool IsActionPossible(Player player) {
        return !isUsed;
    }

    protected override void DoAction(Player player) {
        isUsed = true;
        transform.SetParent(null);
        PlayerStatsDisplay display = PlayerStatsDisplay.Instance;
        PlayerInventoryDisplay inv = PlayerInventoryDisplay.Instance;
        switch (pickup) {
            case PickupType.LIFE:
                player.Stats.AddToLife(amount);
                StartCoroutine(Util.FlyTo(renderer, this.gameObject, display.LifeIcon));
                break;

            case PickupType.POWER:
                player.Stats.AddToPower(amount);
                StartCoroutine(Util.FlyTo(renderer, this.gameObject, display.PowerIcon));
                break;

            case PickupType.DEFENSE:
                player.Stats.AddToDefense(amount);
                StartCoroutine(Util.FlyTo(renderer, this.gameObject, display.DefenseIcon));
                break;

            case PickupType.EXPERIENCE:
                player.Stats.AddToExperience(amount);
                StartCoroutine(Util.FlyTo(renderer, this.gameObject, display.ExperienceIcon));
                break;

            case PickupType.YELLOW_KEY:
                player.Keys.Yellow += amount;
                StartCoroutine(Util.FlyTo(renderer, this.gameObject, inv.GoldIcon));
                break;

            case PickupType.BLUE_KEY:
                player.Keys.Blue += amount;
                StartCoroutine(Util.FlyTo(renderer, this.gameObject, inv.BlueIcon));
                break;

            case PickupType.RED_KEY:
                player.Keys.Red += amount;
                StartCoroutine(Util.FlyTo(renderer, this.gameObject, inv.RedIcon));
                break;
        }
        enabled = false;
    }

    private void Start() {
        renderer = GetComponentInChildren<SpriteRenderer>(true);
    }
}