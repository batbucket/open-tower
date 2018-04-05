using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour {
    private static Battle _instance;

    [SerializeField]
    private float transitionDuration = 0.5f;

    [SerializeField]
    private float shakeIntensity = 5f;

    [SerializeField]
    private float scaleIntensity = 0.10f;

    [SerializeField]
    private float shakeDuration = 1f;

    [SerializeField]
    private float shakeDurationDecay = 0.90f;

    [SerializeField]
    private float minShakeDuration = 0.01f;

    [SerializeField]
    private Transform window;

    [SerializeField]
    private BattleProfile hero;

    [SerializeField]
    private BattleProfile enemy;

    private bool isSkipped;

    public static Battle Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<Battle>();
            }
            return _instance;
        }
    }

    public IEnumerator Init(Player player, Sprite enemySprite, Stats enemyStats, Action callback) {
        this.hero.Init(player.Sprite.sprite, player.Stats.Life, player.Stats.Power, player.Stats.Defense);
        this.enemy.Init(enemySprite, enemyStats.Life, enemyStats.Power, enemyStats.Defense);
        window.gameObject.SetActive(true);
        yield return Util.Lerp(transitionDuration, t => {
            window.localScale = Vector3.Lerp(new Vector3(0, 1, 1), new Vector3(1, 1, 1), t);
        });
        isSkipped = false;
        int playerNetDamage = 0;
        int enemyNetDamage = 0;
        Enemy.GetNetDamages(player.Stats, enemyStats, out playerNetDamage, out enemyNetDamage);
        float currentShakeDuration = shakeDuration;
        while (enemy.IsAlive && !isSkipped) {
            currentShakeDuration *= shakeDurationDecay;
            currentShakeDuration = Math.Max(currentShakeDuration, minShakeDuration);
            yield return hero.Attack(enemy, playerNetDamage, shakeIntensity, scaleIntensity, currentShakeDuration);
            if (enemy.IsAlive) {
                yield return enemy.Attack(hero, enemyNetDamage, shakeIntensity, scaleIntensity, currentShakeDuration);
            }
        }
        yield return new WaitForSeconds(transitionDuration);
        window.gameObject.SetActive(false);
        callback();
    }

    private void Update() {
        if (Input.anyKey) {
            isSkipped = true;
        }
    }
}