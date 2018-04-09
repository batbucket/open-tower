using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleProfile : MonoBehaviour {
    private const float NEXT_TO_PORTRAIT = 0.8f;

    [SerializeField]
    private new SpriteRenderer renderer;

    [SerializeField]
    private ParticleSystem ps;

    [SerializeField]
    private BattleStat life;

    [SerializeField]
    private BattleStat power;

    [SerializeField]
    private BattleStat defense;

    public bool IsAlive {
        get {
            return life.Count > 0;
        }
    }

    public void Init(Sprite sprite, int lifeCount, int powerCount, int defenseCount) {
        this.renderer.sprite = sprite;
        this.life.Init(lifeCount);
        this.power.Init(powerCount);
        this.defense.Init(defenseCount);
    }

    public IEnumerator Attack(BattleProfile other, int damageTaken, float shakeIntensity, float scaleIntensity, float duration, AudioClip hit) {
        Transform ourSprite = this.renderer.transform;
        Transform theirSprite = other.renderer.transform;

        Vector3 startPosition = ourSprite.position;
        Vector3 endPosition = theirSprite.position;

        yield return Approach(ourSprite, startPosition, endPosition, duration, NEXT_TO_PORTRAIT);
        other.ps.Play();
        SoundManager.Instance.Play(hit);
        StartCoroutine(other.life.ChangeCount(shakeIntensity, scaleIntensity, other.life.Count - damageTaken, duration));
        StartCoroutine(Util.ShakeItem(shakeIntensity, scaleIntensity, duration, () => { }, theirSprite));
        yield return new WaitForSeconds(duration);
        yield return Approach(ourSprite, ourSprite.position, startPosition, duration, 1f);
    }

    private IEnumerator Approach(Transform mover, Vector3 start, Vector3 destination, float duration, float inhibition) {
        Vector3 trueDestination;
        trueDestination = Vector3.Lerp(start, destination, inhibition);
        yield return Util.Lerp(duration, t => {
            mover.position = Vector3.Lerp(start, trueDestination, t);
        });
        mover.position = trueDestination;
    }
}