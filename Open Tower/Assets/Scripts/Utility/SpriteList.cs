using System.Collections.Generic;
using UnityEngine;

public static class SpriteList {
    private static bool isInit = false;

    private static readonly string[] ENEMIES = new string[] {
        "wizard",
        "default_tiles_x_8",
        "placeholder"
    };

    private static readonly string[] BOOSTERS = new string[] {
        "Heart",
        "Sword",
        "Shield_Basic",
        "Star"
    };

    private static readonly IDictionary<int, Sprite> enemyIDs = new Dictionary<int, Sprite>();
    private static readonly IDictionary<int, Sprite> boosterIDs = new Dictionary<int, Sprite>();

    public static Sprite GetEnemy(int id) {
        LazyInit();
        return enemyIDs[id];
    }

    public static Sprite GetBooster(int id) {
        LazyInit();
        return boosterIDs[id];
    }

    public static IDictionary<int, Sprite> GetEnemyIDs() {
        LazyInit();
        return enemyIDs;
    }

    public static IDictionary<int, Sprite> GetBoosterIDs() {
        LazyInit();
        return boosterIDs;
    }

    private static void LazyInit() {
        if (!isInit) {
            isInit = true;
            int id = 0;
            foreach (string s in ENEMIES) {
                enemyIDs.Add(id++, LoadSprite(s));
            }
            id = 0;
            foreach (string s in BOOSTERS) {
                boosterIDs.Add(id++, LoadSprite(s));
            }
        }
    }

    private static Sprite LoadSprite(string name) {
        return Resources.Load<Sprite>("Sprites/" + name);
    }
}