using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Util {
    private static readonly bool IS_DEBUG = true && Application.isEditor;

    public static void Assert(bool expression, string format = null, params object[] args) {
        if (IS_DEBUG && !expression && !string.IsNullOrEmpty(format)) {
            throw new UnityException(string.Format(format, args));
        }
    }

    public static float Random(float min, float max) {
        return UnityEngine.Random.Range(min, max);
    }

    public static void KillAllChildren(Transform t) {
        foreach (Transform child in t) {
            if (child != t) {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    public static IEnumerator DoSpriteAnimation(float secondsPerFrame, IList<Sprite> sprites, Action<Sprite> setSprite) {
        if (sprites.Count == 1) {
            setSprite(sprites[0]);
            yield break;
        }
        while (true) {
            foreach (Sprite sprite in sprites) {
                yield return new WaitForSeconds(secondsPerFrame);
                setSprite(sprite);
            }
            if (true) {
                for (int i = sprites.Count - 1; i <= 0; i--) {
                    yield return new WaitForSeconds(secondsPerFrame);
                    setSprite(sprites[i]);
                }
            }
            yield return null;
        }
    }

    public static void FocusOnField(InputField field) {
        EventSystem.current.SetSelectedGameObject(field.gameObject, null);
        field.ActivateInputField();
    }

    public static IEnumerator Lerp(float duration, Action<float> perStep) {
        float timer = 0;
        while ((timer += Time.deltaTime) < duration) {
            perStep(timer / duration);
            yield return null;
        }
    }

    public static void ClampField(InputField field, int min, int max) {
        int value = 0;
        int.TryParse(field.text, out value);
        int clampedValue = Mathf.Clamp(value, min, max);
        field.text = clampedValue.ToString();
    }

    public static IEnumerator AnimatedScrollToBottom(Scrollbar scroll, float scrollDuration = 0.5f) {
        float timer = 0;
        float startValue = scroll.value;
        while ((timer += Time.deltaTime) < scrollDuration) {
            scroll.value = Mathf.SmoothStep(startValue, 0, timer / scrollDuration);
            yield return null;
        }
        scroll.value = 0;
    }

    // assumes scales are initially 1,1,1
    public static IEnumerator ShakeItem(float shakeIntensity, float scaleIntensity, float duration, Action callback, params Transform[] targets) {
        Vector3[] originalPos = new Vector3[targets.Length];
        for (int i = 0; i < targets.Length; i++) {
            originalPos[i] = targets[i].transform.localPosition;
        }
        yield return Util.Lerp(duration, t => {
            for (int i = 0; i < targets.Length; i++) {
                Transform target = targets[i];
                Vector3 originalPosition = originalPos[i];
                Vector3 offset = new Vector3(shakeIntensity * Random(-1, 1), shakeIntensity * Random(-1, 1), 0);
                Vector3 scaleShift = new Vector3(Random(1 - scaleIntensity, 1 + scaleIntensity), Random(1 - scaleIntensity, 1 + scaleIntensity), 1);
                target.localPosition = (originalPosition + offset);
                target.localScale = scaleShift;
            }
        });
        for (int i = 0; i < targets.Length; i++) {
            targets[i].transform.localPosition = originalPos[i];
            targets[i].transform.localScale = Vector3.one;
        }
        callback();
    }

    public static IEnumerator AnimateScore(Text target, int startScore, int endScore, float duration, AudioClip scoreSound, Action callback = null) {
        float timer = 0;
        target.color = Color.grey;
        while ((timer += Time.deltaTime) < duration) {
            target.text = Mathf.CeilToInt(Mathf.Lerp(startScore, endScore, timer / duration)).ToString();
            yield return null;
        }
        target.color = Color.white;
        target.text = endScore.ToString();
        SoundManager.Instance.Play(scoreSound);
        if (callback != null) {
            callback();
        }
    }
}

public static class Extensions {

    public static int IndexOf<T>(this IList<T> list, Func<T, bool> predicate) {
        for (int i = 0; i < list.Count; i++) {
            if (predicate(list[i])) {
                return i;
            }
        }
        throw new KeyNotFoundException();
    }

    public static T Next<T>(this T src) where T : struct {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }

    public static bool IsPlaying(this Animator anim) {
        return anim.GetCurrentAnimatorStateInfo(0).length >
               anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public static bool IsPlaying(this Animator anim, string stateName) {
        return anim.IsPlaying() && anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public static IEnumerator WaitForAnimation(this Animation anim) {
        do {
            yield return null;
        } while (anim.isPlaying);
    }
}