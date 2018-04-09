using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentFloorDisplay : MonoBehaviour {

    [SerializeField]
    private Text text;

    private DungeonManager dungeon;

    private void Start() {
        dungeon = DungeonManager.Instance;
    }

    private void Update() {
        text.text = string.Format("{0}F", dungeon.GetCurrentFloor().transform.GetSiblingIndex());
    }
}