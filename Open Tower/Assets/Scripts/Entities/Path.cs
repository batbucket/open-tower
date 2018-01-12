using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : Entity {

    [SerializeField]
    private float movementDuration = 0.1f;

    protected override void DoAction(Player player) {
        player.Stats.IncrementSteps();
        StartCoroutine(LerpMovement(player));
    }

    protected override bool IsActionPossible(Player player) {
        return true;
    }

    private IEnumerator LerpMovement(Player player) {
        player.IsMovementEnabled = false;
        float timer = 0;
        player.transform.SetParent(this.transform);
        Vector2 startPosition = player.transform.localPosition;
        while ((timer += Time.deltaTime) < movementDuration) {
            Vector2 newPos = Vector3.Slerp(startPosition, Vector2.zero, timer / movementDuration);
            player.transform.localPosition = newPos;
            yield return null;
        }
        player.transform.localPosition = Vector3.zero;
        player.IsMovementEnabled = true;
    }
}