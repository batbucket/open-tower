﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour {
    private const int TOP_LEFT = 0;
    private const int TOP_RIGHT = 1;
    private const int BOT_LEFT = 2;
    private const int BOT_RIGHT = 3;
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

    private void SetPathSprites() {
        DungeonSet set;
        if (SceneUtil.IsCurrentLevelIndex) {
            Debug.Log("loading dungeon set from world index");
            set = SceneUtil.GetSet(SceneUtil.LevelIndex);
        } else {
            Debug.Log("picking a random dungeon set");
            set = SpriteList.GetRandomDungeonSet();
        }
        SoundManager.Instance.Loop(set.Music);
        Sprite[] tiles = set.PathTiles;
        foreach (FloorManager floor in floors) {
            for (int i = 0; i < floor.Rows; i++) {
                for (int j = 0; j < floor.Columns; j++) {
                    bool isRowEven = (i % 2 == 0);
                    bool isColEven = (j % 2 == 0);
                    Sprite tile = null;
                    if (isRowEven) {
                        if (isColEven) {
                            tile = tiles[TOP_LEFT];
                        } else {
                            tile = tiles[TOP_RIGHT];
                        }
                    } else {
                        if (isColEven) {
                            tile = tiles[BOT_LEFT];
                        } else {
                            tile = tiles[BOT_RIGHT];
                        }
                    }
                    SetTileSprite(floor, tile, i, j);
                }
            }
        }
    }

    private void Start() {
        SetPathSprites();
    }

    private void Awake() {
        this.floors = GetComponentsInChildren<FloorManager>(true);
    }

    private void SetTileSprite(FloorManager floor, Sprite sprite, int row, int column) {
        floor.GetTileAtPosition(row, column)
            .GetComponentInChildren<Path>(true)
            .GetComponentInChildren<SpriteRenderer>(true)
            .sprite = sprite;
    }
}