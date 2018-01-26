using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util {
    private static readonly bool IS_DEBUG = true && Application.isEditor;

    public static void Assert(bool expression, string format, params object[] args) {
        if (IS_DEBUG && !expression) {
            throw new UnityException(string.Format(format, args));
        }
    }

    public static float Random() {
        return UnityEngine.Random.value;
    }
}