using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntitiesPanel : Panel {
    private static EntitiesPanel _instance;

    /// <summary>
    /// The prefabs. Should be parallel to the dropdown!
    /// </summary>
    [SerializeField]
    private AddableTile[] prefabs;

    [SerializeField]
    private Dropdown dropdown;

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

    public void AddTile() {
        Instantiate(prefabs[dropdown.value], tileHolder);
        if (current != null) {
            StopCoroutine(current);
        }
        current = StartCoroutine(Util.AnimatedScrollToBottom(scroll));
    }

    public override void Clear() {
        foreach (AddableTile tile in tileHolder.GetComponentsInChildren<AddableTile>(true)) {
            if (!tile.IsStaticTileType) {
                Destroy(tile.gameObject);
            }
        }
        LastSelected = null;
    }
}