using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    private static DungeonManager dungeon;

    private int Position {
        get {
            return GetComponentInParent<Tile>().transform.GetSiblingIndex();
        }
    }

    // Update is called once per frame
    public void OnUpdate(Player player) {
        FloorManager current = dungeon.GetCurrentFloor();
        bool wasMovementUsed = false;
        int currentIndex = Position;

        int rowToCheck = current.GetRowOfIndex(currentIndex);
        int columnToCheck = current.GetColumnOfIndex(currentIndex);
        if (Input.GetKeyDown(KeyCode.W)) {
            wasMovementUsed = true;
            rowToCheck--;
        } else if (Input.GetKeyDown(KeyCode.A)) {
            wasMovementUsed = true;
            columnToCheck--;
        } else if (Input.GetKeyDown(KeyCode.S)) {
            wasMovementUsed = true;
            rowToCheck++;
        } else if (Input.GetKeyDown(KeyCode.D)) {
            wasMovementUsed = true;
            columnToCheck++;
        }
        if (wasMovementUsed && current.IsRowColumnValid(rowToCheck, columnToCheck)) {
            current.GetTileAtPosition(rowToCheck, columnToCheck).Interact(player);
        }
    }

    private void Start() {
        dungeon = DungeonManager.Instance;
    }
}