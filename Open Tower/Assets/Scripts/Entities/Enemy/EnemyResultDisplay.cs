using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyResultDisplay : MonoBehaviour {

    [SerializeField]
    private Font font;

    [SerializeField]
    private GameObject wrapper;

    [SerializeField]
    private Text damageTaken;

    private Stats enemy;
    private Stats player;

    public static bool IsActive {
        get {
            return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Mouse1);
        }
    }

    public void Init(Stats enemy) {
        this.enemy = enemy;
    }

    private void Start() {
        Util.Assert(font != null, "Font is null.");
        damageTaken.font = font;
        this.player = Player.Instance.Stats;
    }

    private void Update() {
        wrapper.SetActive(IsActive);
        int damage = Enemy.GetDamageToPlayer(this.enemy, this.player);
        if (damage == Enemy.ENEMY_CANNOT_BE_DEFEATED) {
            damageTaken.text = "-???";
            damageTaken.color = Color.magenta;
        } else if (damage == 0) {
            damageTaken.text = damage.ToString();
            damageTaken.color = Color.white;
        } else {
            damageTaken.text = damage.ToString();
            damageTaken.color = Color.magenta;
        }
    }
}