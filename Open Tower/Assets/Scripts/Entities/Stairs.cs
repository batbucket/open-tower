using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : Entity {

    [SerializeField]
    private Tile destination;

    protected override void DoAction(Player player) {
        player.gameObject.transform.SetParent(destination.gameObject.transform, false);
        player.transform.localPosition = Vector3.zero;
        GetComponentInParent<FloorManager>().gameObject.SetActive(false);
        destination.transform.parent.GetComponent<FloorManager>().gameObject.SetActive(true);
        player.transform.localPosition = destination.GetComponentInChildren<Path>().transform.localPosition;
    }

    protected override bool IsActionPossible(Player player) {
        return true;
    }
}