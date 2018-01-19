using System;
using System.Collections;
using UnityEngine;

public class MoveProcess : Process {

    [SerializeField]
    private Transform actor;

    [SerializeField]
    private Transform destination;

    [SerializeField]
    private float duration;

    public override IEnumerator Play() {
        float timer = 0;
        Vector3 startPos = actor.position;
        Vector3 endPos = destination.position;
        while ((timer += Time.deltaTime) < duration) {
            actor.transform.position = Vector3.Lerp(startPos, endPos, timer / duration);
            yield return null;
        }
    }
}