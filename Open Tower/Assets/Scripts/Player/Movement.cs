using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    private const float AUTOFIRE_DELAY = 0.5f;
    private const float AUTOFIRE_COOLDOWN = 0.1f;
    private static DungeonManager dungeon;

    [SerializeField]
    private SpriteAnimator animator;

    private float autofireTimer;
    private float cooldown;
    private HashSet<KeyCode> keysDown;

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
        if (IsKeyValid(KeyCode.W, KeyCode.UpArrow)) {
            keysDown.Add(KeyCode.W);
            keysDown.Add(KeyCode.UpArrow);
            wasMovementUsed = true;
            rowToCheck--;
        } else if (IsKeyValid(KeyCode.A, KeyCode.LeftArrow)) {
            keysDown.Add(KeyCode.A);
            keysDown.Add(KeyCode.LeftArrow);
            wasMovementUsed = true;
            columnToCheck--;
        } else if (IsKeyValid(KeyCode.S, KeyCode.DownArrow)) {
            keysDown.Add(KeyCode.S);
            keysDown.Add(KeyCode.DownArrow);
            wasMovementUsed = true;
            rowToCheck++;
        } else if (IsKeyValid(KeyCode.D, KeyCode.RightArrow)) {
            keysDown.Add(KeyCode.D);
            keysDown.Add(KeyCode.RightArrow);
            wasMovementUsed = true;
            columnToCheck++;
        }
        if (wasMovementUsed && current.IsRowColumnValid(rowToCheck, columnToCheck)) {
            current.GetTileAtPosition(rowToCheck, columnToCheck).Interact(player);
            if (autofireTimer > AUTOFIRE_DELAY) {
                cooldown = 0;
            }
        }

        bool isKeyHeldDown = false;
        foreach (KeyCode key in keysDown) {
            if (Input.GetKey(key)) {
                isKeyHeldDown = true;
            }
        }
        if (!isKeyHeldDown) {
            keysDown.Clear();
            autofireTimer = 0;
            cooldown = 0;
        } else {
            autofireTimer += Time.deltaTime;
            if (autofireTimer > AUTOFIRE_DELAY) {
                cooldown += Time.deltaTime;
            }
        }
    }

    private bool IsKeyValid(KeyCode main, KeyCode alt) {
        return (Input.GetKeyDown(main) || Input.GetKeyDown(alt))
            || ((Input.GetKey(main) || Input.GetKey(alt))
            && autofireTimer > AUTOFIRE_DELAY
            && cooldown > AUTOFIRE_COOLDOWN);
    }

    private void Start() {
        dungeon = DungeonManager.Instance;
        keysDown = new HashSet<KeyCode>();
    }
}