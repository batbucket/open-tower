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
        PlayerStatsDisplay display = PlayerStatsDisplay.Instance;
        PlayerInventoryDisplay inv = PlayerInventoryDisplay.Instance;
        switch (pickup) {
            case PickupType.LIFE:
                player.Stats.AddToLife(amount);
                StartCoroutine(FlyTo(display.LifeIcon));
                break;

            case PickupType.POWER:
                player.Stats.AddToPower(amount);
                StartCoroutine(FlyTo(display.PowerIcon));
                break;

            case PickupType.DEFENSE:
                player.Stats.AddToDefense(amount);
                StartCoroutine(FlyTo(display.DefenseIcon));
                break;

            case PickupType.EXPERIENCE:
                player.Stats.AddToExperience(amount);
                StartCoroutine(FlyTo(display.ExperienceIcon));
                break;

            case PickupType.YELLOW_KEY:
                player.Keys.Yellow += amount;
                StartCoroutine(FlyTo(inv.GoldIcon));
                break;

            case PickupType.BLUE_KEY:
                player.Keys.Blue += amount;
                StartCoroutine(FlyTo(inv.BlueIcon));
                break;

            case PickupType.RED_KEY:
                player.Keys.Red += amount;
                StartCoroutine(FlyTo(inv.RedIcon));
                break;
        }
    }

    private void Start() {
        renderer = GetComponentInChildren<SpriteRenderer>(true);
    }

    private IEnumerator FlyTo(Image destination) {
        renderer.sortingOrder = 1;
        renderer.sortingLayerName = "Default";

        Vector3 start = transform.position;
        Vector3 end = destination.transform.position + new Vector3(16, 0, 0);
        yield return Util.Lerp(0.25f, t => {
            transform.position = Vector3.Lerp(start, end, t);
            transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.5f, 0.5f, 0.5f), t);
        });
        transform.position = end;
        this.gameObject.SetActive(false);
    }
}