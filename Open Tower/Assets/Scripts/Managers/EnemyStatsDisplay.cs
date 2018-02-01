using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatsDisplay : MonoBehaviour {

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

    private void Start() {
        life.text = enemy.Life.ToString();
        power.text = enemy.Power.ToString();
        defense.text = enemy.Defense.ToString();
        experience.text = enemy.Experience.ToString();

        if (enemy.Experience == 0) {
            Destroy(experience.transform.parent.gameObject);
        }
    }

    private void Update() {
        wrapper.SetActive(Input.GetKey(KeyCode.Space));
    }
}