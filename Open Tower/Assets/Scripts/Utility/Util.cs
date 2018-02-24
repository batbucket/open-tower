using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Util {
    private static readonly bool IS_DEBUG = true && Application.isEditor;

    public static void Assert(bool expression, string format, params object[] args) {
        if (IS_DEBUG && !expression) {
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
}

public static class Extensions {

    public static T Next<T>(this T src) where T : struct {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }
}