using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour {
    private const float secondsPerFrame = 1f;

    [SerializeField]
    private Sprite[] sprites;

    private new SpriteRenderer renderer;
    private Coroutine routine;

    private void Start() {
        renderer = GetComponentInChildren<SpriteRenderer>(true);
    }

    private void OnEnable() {
        routine = StartCoroutine(DoAnimation());
    }

    private void OnDisable() {
        if (routine != null) {
            StopCoroutine(routine);
        }
    }

    private IEnumerator DoAnimation() {
        while (true) {
            foreach (Sprite sprite in sprites) {
                yield return new WaitForSeconds(secondsPerFrame);
                renderer.sprite = sprite;
                yield return null;
            }
        }
    }
}