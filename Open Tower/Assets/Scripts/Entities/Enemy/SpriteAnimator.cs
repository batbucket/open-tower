using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour {
    private const float secondsPerFrame = 0.5f;

    [SerializeField]
    private Sprite[] sprites;

    private new SpriteRenderer renderer;
    private Coroutine routine;

    private Transform rendererT;

    private void Start() {
        renderer = GetComponentInChildren<SpriteRenderer>(true);
        rendererT = renderer.transform;
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
                renderer.sprite = sprite;
            }
            yield return null;
        }
    }
}