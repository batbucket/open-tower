using System.Collections;
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

    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private Transform mask;

    [SerializeField]
    private ParticleSystem ps;

    [SerializeField]
    private AudioClip teleport;

    public static Player Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<Player>();
            }
            return _instance;
        }
    }

    public SpriteRenderer Sprite {
        get {
            return sprite;
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

    public IEnumerator TransitionOut() {
        mask.gameObject.SetActive(true);
        Vector3 start = mask.localPosition;
        ps.Play();
        SoundManager.Instance.Play(teleport);
        yield return Util.Lerp(0.5f, t => {
            mask.localPosition = Vector3.Lerp(start, Vector3.zero, t);
        });
        ps.Stop();
    }

    private void Update() {
        if (movement.isActiveAndEnabled) {
            movement.OnUpdate(this);
        }
    }
}