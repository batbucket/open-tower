using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}

public static class Extensions {

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