using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntitiesPanel : Panel {
    private static EntitiesPanel _instance;

    [SerializeField]
    private AddableTile enemyPrefab;

    [SerializeField]
    private AddableTile pickupPrefab;

    [SerializeField]
    private Transform tileHolder;

    [SerializeField]
    private Scrollbar scroll;

    private Coroutine current;

    [HideInInspector]
    public AddableTile LastSelected;

    public static EntitiesPanel Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<EntitiesPanel>();
            }
            return _instance;
        }
    }

    public Transform TileHolder {
        get {
            return tileHolder;
        }
    }

    public void AddEnemy() {
        AddTile(enemyPrefab.gameObject);
    }

    public void AddPickup() {
        AddTile(pickupPrefab.gameObject);
    }

    public override void Clear() {
        foreach (AddableTile tile in tileHolder.GetComponentsInChildren<AddableTile>(true)) {
            if (!tile.IsStaticTileType) {
                Destroy(tile.gameObject);
            }
        }
        LastSelected = null;
    }

    private void AddTile(GameObject prefab) {
        Instantiate(prefab, tileHolder);
        if (current != null) {
            StopCoroutine(current);
        }
        current = StartCoroutine(Util.AnimatedScrollToBottom(scroll));
    }
}