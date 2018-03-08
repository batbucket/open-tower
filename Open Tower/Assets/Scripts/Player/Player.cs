using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(Inventory))]
public class Player : MonoBehaviour {
    private static Player _instance;

    [SerializeField]
    private Movement movement;

    [SerializeField]
    private Stats stats;

    [SerializeField]
    private Inventory keys;

    public static Player Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<Player>();
            }
            return _instance;
        }
    }

    public Stats Stats {
        get {
            return stats;
        }
    }

    public Inventory Keys {
        get {
            return keys;
        }
    }

    public bool IsMovementEnabled {
        set {
            movement.enabled = value;
        }
    }

    public void Init(Scripts.LevelEditor.Serialization.StartingValues sv) {
        Stats.Init(sv.Life, sv.Power, sv.Defense, sv.Stars);
        Keys.Init(sv.GoldKeys, sv.BlueKeys, sv.RedKeys);
    }

    private void Update() {
        if (movement.isActiveAndEnabled) {
            movement.OnUpdate(this);
        }
    }
}