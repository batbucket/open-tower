using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentFloorDisplay : MonoBehaviour {

    [SerializeField]
    private DungeonManager dungeon;

    [SerializeField]
    private Text text;

    private void Update() {
        text.text = string.Format("{0}F", dungeon.GetCurrentFloor().transform.GetSiblingIndex());
    }
}