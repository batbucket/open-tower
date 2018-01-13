using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour {
    private static DungeonManager _instance;

    private FloorManager[] floors;
    private FloorManager current;

    public static DungeonManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<DungeonManager>();
            }
            return _instance;
        }
    }

    public FloorManager GetFloor(int index) {
        return floors[index];
    }

    public FloorManager GetCurrentFloor() {
        if (current == null || !current.gameObject.activeInHierarchy) {
            current = FindObjectOfType<FloorManager>();
        }
        return current;
    }

    private void Awake() {
        this.floors = GetComponentsInChildren<FloorManager>(true);
    }
}