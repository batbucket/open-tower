using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : Entity {

    [SerializeField]
    private string destinationScene;

    [SerializeField]
    private int trophyID = int.MinValue;

    protected override void DoAction(Player player) {
        player.IsMovementEnabled = false;
        if (trophyID != int.MinValue) {
            GameJolt.API.Trophies.Unlock(trophyID, isSuccess => {
                Debug.Log(isSuccess);
            });
        }
        ResultsManager.Instance.ShowResults(destinationScene);
    }

    protected override bool IsActionPossible(Player player) {
        return true;
    }
}