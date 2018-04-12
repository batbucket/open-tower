using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    private Entity[] children;
    private int index;

    // Interact with highest priority object
    public void Interact(Player player) {
        bool isInteractedWith = false;

        for (int i = 0; i < children.Length && !isInteractedWith; i++) {
            Entity entity = children[i];
            if (entity.gameObject.activeInHierarchy && entity.enabled) {
                children[i].TryDoAction(player);
                isInteractedWith = true;
            }
        }
    }

    private void Start() {
        children = GetComponentsInChildren<Entity>();
        Array.Sort(children);
    }
}