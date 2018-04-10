using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Entity {
    private const string ANIM_START_TRIGGER = "Open";

    [SerializeField]
    private KeyType key;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private AudioClip opening;

    protected override void DoAction(Player player) {
        switch (key) {
            case KeyType.YELLOW:
                player.Keys.Yellow--;
                break;

            case KeyType.BLUE:
                player.Keys.Blue--;
                break;

            case KeyType.RED:
                player.Keys.Red--;
                break;
        }
        Player.Instance.IsMovementEnabled = false;
        SoundManager.Instance.Play(opening);
        anim.SetTrigger(ANIM_START_TRIGGER);
    }

    protected override bool IsActionPossible(Player player) {
        bool hasKey = false;
        switch (key) {
            case KeyType.YELLOW:
                hasKey = player.Keys.Yellow > 0;
                break;

            case KeyType.BLUE:
                hasKey = player.Keys.Blue > 0;
                break;

            case KeyType.RED:
                hasKey = player.Keys.Red > 0;
                break;
        }
        return hasKey;
    }
}