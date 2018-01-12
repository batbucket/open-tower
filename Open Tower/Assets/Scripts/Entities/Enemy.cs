using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class Enemy : Entity {
    private const int ENEMY_CANNOT_BE_DEFEATED = -1;

    [SerializeField]
    private Stats stats;

    protected override void DoAction(Player player) {
        player.Stats.AddToLife(GetDamageToPlayer(player));
        player.Stats.AddToExperience(this.stats.Experience);
        gameObject.SetActive(false);
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

        int enemyTurnsToKillPlayer = 0;
        if (damageToPlayer > 0) {
            enemyTurnsToKillPlayer = player.Stats.Life / damageToPlayer;
        }
        int playerTurnsToKillEnemy = 0;
        if (damageToEnemy > 0) {
            playerTurnsToKillEnemy = this.stats.Life / damageToEnemy;
        }

        // Player gets killed
        if (playerTurnsToKillEnemy >= enemyTurnsToKillPlayer) {
            return ENEMY_CANNOT_BE_DEFEATED;
        }

        return playerTurnsToKillEnemy * -damageToPlayer;
    }
}