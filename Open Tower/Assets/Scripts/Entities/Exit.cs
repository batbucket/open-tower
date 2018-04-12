using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : Entity {

    protected override void DoAction(Player player) {
        player.IsMovementEnabled = false;
        StartCoroutine(TeleportAway(player));
    }

    protected override bool IsActionPossible(Player player) {
        return true;
    }

    private IEnumerator TeleportAway(Player target) {
        yield return target.TransitionOut();
        ResultsManager.Instance.ShowResults();
    }
}