using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

[ExecuteInEditMode]
public class BoosterStatsDisplay : MonoBehaviour {
    private static readonly Color LIFE = Color.yellow;
    private static readonly Color POWER = Color.magenta;
    private static readonly Color DEFENSE = Color.cyan;
    private static readonly Color STARS = Color.white;

    [SerializeField]
    private Font font;

    [SerializeField]
    private GameObject wrapper;

    [SerializeField]
    private Pickup pickup;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Text text;

    [SerializeField]
    private Sprite life;

    [SerializeField]
    private Sprite power;

    [SerializeField]
    private Sprite defense;

    [SerializeField]
    private Sprite stars;

    private void Start() {
        Util.Assert(font != null, "Font is null.");
        Sprite sprite = null;
        Color color = Color.black;
        switch (pickup.Type) {
            case PickupType.LIFE:
                sprite = life;
                color = LIFE;
                break;

            case PickupType.POWER:
                sprite = power;
                color = POWER;
                break;

            case PickupType.DEFENSE:
                sprite = defense;
                color = DEFENSE;
                break;

            case PickupType.EXPERIENCE:
                sprite = stars;
                color = STARS;
                break;

            default:
                Util.Assert(false, "Unhandled pickup type: {0}", pickup.Type);
                break;
        }
        image.sprite = sprite;
        image.color = color;
        text.font = font;
        text.text = "+" + pickup.Amount;
    }

    private void Update() {
        wrapper.SetActive(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0));
    }
}