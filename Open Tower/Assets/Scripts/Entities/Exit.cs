using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : Entity {

    [SerializeField]
    private string destinationScene;

    [SerializeField]
    private int trophyID = int.MinValue;

    public void Init(string destination) {
        this.destinationScene = destination;
    }

    protected override void DoAction(Player player) {
        player.IsMovementEnabled = false;
        if (trophyID != int.MinValue) {
            GameJolt.API.Trophies.Unlock(trophyID, isSuccess => {
                Debug.Log(isSuccess);
            });
        }
        StartCoroutine(TeleportAway(player));
    }

    protected override bool IsActionPossible(Player player) {
        return true;
    }

    private IEnumerator TeleportAway(Player target) {
        yield return target.TransitionOut();
        ResultsManager.Instance.ShowResults(destinationScene);
    }
}