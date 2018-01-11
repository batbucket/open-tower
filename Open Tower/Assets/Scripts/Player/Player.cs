using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private Movement movement;

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