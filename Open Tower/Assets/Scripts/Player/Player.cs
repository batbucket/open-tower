using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(Inventory))]
public class Player : MonoBehaviour {

    [SerializeField]
    private Movement movement;

    [SerializeField]
    private Stats stats;

    public Stats Stats {
        get {
            return stats;
        }
    }

    public bool IsMovementEnabled {
        set {
            movement.enabled = value;
        }
    }

    private void Update() {
        if (movement.isActiveAndEnabled) {
            movement.OnUpdate(this);
        }
    }
}