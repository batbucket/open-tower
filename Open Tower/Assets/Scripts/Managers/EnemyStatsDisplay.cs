using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatsDisplay : MonoBehaviour {

    [SerializeField]
    private Font font;

    [SerializeField]
    private Stats enemy;

    [SerializeField]
    private Text life;

    [SerializeField]
    private Text power;

    [SerializeField]
    private Text defense;

    [SerializeField]
    private Text experience;

    [SerializeField]
    private GameObject wrapper;

    public static bool IsActive {
        get {
            return !Battle.Instance.IsWindowOpen 
                && (Util.GetBool(Toggle.STATS_KEY) != (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0)));
        }
    }

    private void Start() {
        Util.Assert(font != null, "Font is null.");
        life.font = font;
        power.font = font;
        defense.font = font;
        experience.font = font;
        life.text = enemy.Life.ToString();
        power.text = enemy.Power.ToString();
        defense.text = enemy.Defense.ToString();
        experience.text = enemy.Experience.ToString();

        if (enemy.Experience == 0) {
            Destroy(experience.transform.parent.gameObject);
        }
    }

    private void Update() {
        wrapper.SetActive(!EnemyResultDisplay.IsActive && IsActive);
    }
}