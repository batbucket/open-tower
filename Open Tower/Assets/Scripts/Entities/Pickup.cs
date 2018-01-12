using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Entity {

    [SerializeField]
    private PickupType pickup;

    [SerializeField]
    private int amount;

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