using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class Enemy : Entity {
    public const int ENEMY_CANNOT_BE_DEFEATED = 1;
    private const float DEATH_PLAYBACK_SPEED = 2;

    private static AudioClip hit;
    private static AudioClip steam;
    private static SpriteRenderer flyingStar;
    private static Material outline;

    private static IDictionary<StatType, Color> colors = new Dictionary<StatType, Color>() {
            { StatType.LIFE, Color.yellow },
            { StatType.POWER, Color.red },
            { StatType.DEFENSE, Color.cyan }
        };

    [SerializeField]
    private EnemyResultDisplay resultPrefab;

    [SerializeField]
    private Tip tip;

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
    private bool isDefeated;

    public static int GetDamageToPlayer(Stats enemy, Stats player) {
        int damageToPlayer = Mathf.Max(0, enemy.Power - player.Defense);
        int damageToEnemy = Mathf.Max(0, player.Power - enemy.Defense);

        // Stalemate check
        if (damageToEnemy <= 0) {
            return ENEMY_CANNOT_BE_DEFEATED;
        }

        int enemyTurnsToKillPlayer = int.MaxValue;
        if (damageToPlayer > 0) {
            enemyTurnsToKillPlayer = Mathf.Max(1, Mathf.CeilToInt(((float)player.Life) / damageToPlayer));
        }
        int playerTurnsToKillEnemy = int.MaxValue;
        if (damageToEnemy > 0) {
            playerTurnsToKillEnemy = Mathf.Max(1, Mathf.CeilToInt(((float)enemy.Life) / damageToEnemy));
        }

        // Player gets killed
        if (playerTurnsToKillEnemy > enemyTurnsToKillPlayer) {
            return ENEMY_CANNOT_BE_DEFEATED;
        }

        return Mathf.Max(0, playerTurnsToKillEnemy - 1) * -damageToPlayer;
    }

    public static void GetNetDamages(Stats player, Stats enemy, out int playerNetDamage, out int enemyNetDamage) {
        playerNetDamage = Mathf.Max(0, player.Power - enemy.Defense);
        enemyNetDamage = Mathf.Max(0, enemy.Power - player.Defense);
    }

    protected override void DoAction(Player player) {
        isDefeated = true;
        transform.SetParent(null);

        GetComponentInChildren<EnemyResultDisplay>(true).gameObject.SetActive(false);
        GetComponentInChildren<EnemyStatsDisplay>(true).gameObject.SetActive(false);

        player.IsMovementEnabled = false;
        ParticleSystem.MainModule main = shakePs.main;
        main.simulationSpeed = DEATH_PLAYBACK_SPEED;
        float effectDuration = shakePs.main.startLifetime.constantMax / DEATH_PLAYBACK_SPEED;

        if (!Util.GetBool(Toggle.BATTLE_KEY) && !IsSkipKeyDown()) {
            StartCoroutine(Battle.Instance.Init(player, this.sprite, this.stats, () => {
                OnBattleEnd(player, effectDuration);
            }));
        } else {
            OnBattleEnd(player, effectDuration);
        }
    }

    protected override bool IsActionPossible(Player player) {
        return !isDefeated && (GetDamageToPlayer(player) != ENEMY_CANNOT_BE_DEFEATED);
    }

    private void OnBattleEnd(Player player, float effectDuration) {
        StartCoroutine(DeathEffect(effectDuration, () => { }));
        flicker = StartCoroutine(Flicker(effectDuration));
        player.Stats.AddToLife(GetDamageToPlayer(player));
        player.Stats.AddToExperience(this.stats.Experience);
        player.IsMovementEnabled = true;
        this.enabled = false;

        if (this.stats.Experience > 0) {
            SpriteRenderer star = Instantiate<SpriteRenderer>(flyingStar);
            star.transform.position = this.transform.position;
            StartCoroutine(Util.FlyTo(star, star.gameObject, PlayerStatsDisplay.Instance.ExperienceIcon));
        }
    }

    private bool IsSkipKeyDown() {
        return (Input.GetKey(KeyCode.Space)
            || Input.GetKey(KeyCode.LeftShift)
            || Input.GetKey(KeyCode.Mouse0)
            || Input.GetKey(KeyCode.Mouse1));
    }

    private int GetDamageToPlayer(Player player) {
        return GetDamageToPlayer(this.stats, player.Stats);
    }

    private IEnumerator DeathEffect(float duration, Action callback) {
        SoundManager.Instance.Play(hit);
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
        SoundManager.Instance.Play(steam);
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

    private void Start() {
        Instantiate(resultPrefab, this.transform).Init(this.stats);
        if (outline == null) { // others are also null
            outline = Resources.Load<Material>("Materials/Red Sprite Outline");
            flyingStar = Resources.Load<SpriteRenderer>("Prefabs/Flying Star");
            hit = Resources.Load<AudioClip>("Sounds/qubodupPunch01");
            steam = Resources.Load<AudioClip>("Sounds/steam hiss");
        }
        this.sprite.material = outline;
        this.sprite.material.color = CalculateOutlineColor();
    }

    private Color CalculateOutlineColor() {
        IDictionary<StatType, int> dict = new Dictionary<StatType, int>() {
            { StatType.LIFE, stats.Life },
            { StatType.POWER, stats.Power },
            { StatType.DEFENSE, stats.Defense }
        };

        int total = dict.Sum(entry => entry.Value);
        var highestValues = dict.Select(entry => colors[entry.Key] * (entry.Value / (float)total)).ToArray();
        return highestValues.Aggregate((c1, c2) => c1 + c2);
    }

    private static Color CombineColors(ICollection<Color> colors) {
        Color result = new Color(0, 0, 0, 0);
        foreach (Color c in colors) {
            result += c;
        }
        result /= colors.Count;
        return result;
    }
}