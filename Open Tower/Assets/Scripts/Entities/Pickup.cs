using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Entity {

    [SerializeField]
    private PickupType pickup;

    [SerializeField]
    private int amount;

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
        return true;
    }

    protected override void DoAction(Player player) {
        switch (pickup) {
            case PickupType.LIFE:
                player.Stats.AddToLife(amount);
                break;

            case PickupType.POWER:
                player.Stats.AddToPower(amount);
                break;

            case PickupType.DEFENSE:
                player.Stats.AddToDefense(amount);
                break;

            case PickupType.EXPERIENCE:
                player.Stats.AddToExperience(amount);
                break;

            case PickupType.YELLOW_KEY:
                player.Keys.Yellow += amount;
                break;

            case PickupType.BLUE_KEY:
                player.Keys.Blue += amount;
                break;

            case PickupType.RED_KEY:
                player.Keys.Red += amount;
                break;
        }
        this.gameObject.SetActive(false);
    }
}