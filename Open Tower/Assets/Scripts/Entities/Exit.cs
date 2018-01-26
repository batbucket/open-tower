using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : Entity {

    [SerializeField]
    private string destinationScene;

    protected override void DoAction(Player player) {
        SceneManager.LoadScene(destinationScene);
    }

    protected override bool IsActionPossible(Player player) {
        return true;
    }
}