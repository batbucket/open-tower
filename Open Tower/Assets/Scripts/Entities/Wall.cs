using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Entity {

    [SerializeField]
    private KeyType key;

    protected override void DoAction(Player player) {
        // Do nothing
    }

    protected override bool IsActionPossible(Player player) {
        return true;
    }
}