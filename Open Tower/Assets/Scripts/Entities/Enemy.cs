using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class Enemy : Entity {
    private const int ENEMY_CANNOT_BE_DEFEATED = -1;

    [SerializeField]
    private Stats stats;

    [SerializeField]
    private ParticleSystem shakePs;

    [SerializeField]
    private ParticleSystem vanishPs;

    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private float shakeScale;

    [SerializeField]
    private float flickerInterval;

    [SerializeField]
    private float delayBeforeDisappear;

    private Coroutine flicker;

    protected override void DoAction(Player player) {
        player.Stats.AddToLife(GetDamageToPlayer(player));
        player.Stats.AddToExperience(this.stats.Experience);
        player.IsMovementEnabled = false;
        float effectDuration = shakePs.main.startLifetime.constantMax;
        StartCoroutine(DeathEffect(effectDuration, () => player.IsMovementEnabled = true));
        flicker = StartCoroutine(Flicker(effectDuration));
    }

    protected override bool IsActionPossible(Player player) {
        return (GetDamageToPlayer(player) != ENEMY_CANNOT_BE_DEFEATED);
    }

    private int GetDamageToPlayer(Player player) {
        int damageToPlayer = Mathf.Max(0, this.stats.Power - player.Stats.Defense);
        int damageToEnemy = Mathf.Max(0, player.Stats.Power - this.stats.Defense);

        // Stalemate check
        if (damageToEnemy <= 0) {
            return ENEMY_CANNOT_BE_DEFEATED;
        }

        int enemyTurnsToKillPlayer = int.MaxValue;
        if (damageToPlayer > 0) {
            enemyTurnsToKillPlayer = Mathf.Max(1, player.Stats.Life / damageToPlayer);
        }
        int playerTurnsToKillEnemy = int.MaxValue;
        if (damageToEnemy > 0) {
            playerTurnsToKillEnemy = Mathf.Max(1, this.stats.Life / damageToEnemy);
        }

        // Player gets killed
        if (playerTurnsToKillEnemy >= enemyTurnsToKillPlayer) {
            return ENEMY_CANNOT_BE_DEFEATED;
        }

        return playerTurnsToKillEnemy * -damageToPlayer;
    }

    private IEnumerator DeathEffect(float duration, Action callback) {
        float lifetime = shakePs.main.startLifetime.constantMax;
        // Start PS
        shakePs.Play();
        // Shake
        float timer = 0;
        float currentShakeScale = 0;
        while ((timer += Time.deltaTime) < duration) {
            currentShakeScale = Mathf.Lerp(shakeScale, 0, timer / duration);
            sprite.transform.localPosition = new Vector2(Util.Random(-1, 1) * currentShakeScale, Util.Random(-1, 1) * currentShakeScale);
            yield return null;
        }
        sprite.transform.localPosition = Vector2.zero;
        timer = 0;
        Vector3 oldScales = sprite.transform.localScale;
        Vector3 target = new Vector3(0, 0, 1);
        while ((timer += Time.deltaTime) < delayBeforeDisappear) {
            sprite.transform.localScale = Vector3.Lerp(oldScales, target, timer / delayBeforeDisappear);
            yield return null;
        }
        yield return new WaitForSeconds(delayBeforeDisappear);
        // Disappearance explosion
        vanishPs.Play();
        StopCoroutine(flicker);
        sprite.enabled = false;
        yield return new WaitWhile(() => vanishPs.isPlaying);
        callback();
        gameObject.SetActive(false);
    }

    private IEnumerator Flicker(float duration) {
        // Flicker
        float timer = 0;
        while (true) {
            timer += Time.deltaTime;
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(flickerInterval);
            timer += flickerInterval;
        }
    }
}