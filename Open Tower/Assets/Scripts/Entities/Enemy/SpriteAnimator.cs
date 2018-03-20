using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimator : MonoBehaviour {

    [SerializeField]
    private float secondsPerFrame = 0.5f;

    [SerializeField]
    private bool isReverse;

    [SerializeField]
    private Sprite[] sprites;

    private new SpriteRenderer renderer;
    private Image image;
    private Coroutine routine;

    private Transform rendererT;
    private Action<Sprite> setSprite;

    private void Start() {
        renderer = GetComponentInChildren<SpriteRenderer>(true);
        image = GetComponentInChildren<Image>(true);
        Util.Assert(renderer != null || image != null, "Either image or renderer has to be assigned.");
        if (renderer != null) {
            setSprite = (sprite => renderer.sprite = sprite);
        } else {
            setSprite = (sprite => image.sprite = sprite);
        }
    }

    private void OnEnable() {
        routine = StartCoroutine(DoAnimation());
    }

    private void OnDisable() {
        if (routine != null) {
            StopCoroutine(routine);
        }
    }

    private IEnumerator DoLazyAnimation() {
        Start();
        while (true) {
            yield return Util.Lerp(1, t => {
                rendererT.localPosition = Vector2.Lerp(new Vector2(0, -0.95f), new Vector2(0, 0.95f), t);
                rendererT.localScale = Vector2.Lerp(new Vector2(1, 0.95f), new Vector2(1, 1.05f), t);
            });
            yield return Util.Lerp(1, t => {
                rendererT.localPosition = Vector2.Lerp(new Vector2(0, 0.95f), new Vector2(0, -0.95f), t);
                rendererT.localScale = Vector2.Lerp(new Vector2(1, 1.05f), new Vector2(1, 0.95f), t);
            });
        }
    }

    private IEnumerator DoAnimation() {
        while (true) {
            foreach (Sprite sprite in sprites) {
                yield return new WaitForSeconds(secondsPerFrame);
                setSprite(sprite);
            }
            if (true) {
                for (int i = sprites.Length - 1; i <= 0; i--) {
                    yield return new WaitForSeconds(secondsPerFrame);
                    setSprite(sprites[i]);
                }
            }
            yield return null;
        }
    }
}