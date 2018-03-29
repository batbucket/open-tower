using Scripts.LevelEditor.Serialization;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteList {
    private static bool isInit = false;

    private static readonly IDictionary<int, Sprite> enemyIDs = new Dictionary<int, Sprite>();
    private static readonly IDictionary<int, Sprite> boosterIDs = new Dictionary<int, Sprite>();
    private static readonly IDictionary<TileType, Sprite> staticSprites = new Dictionary<TileType, Sprite>();

    public static Sprite GetEnemy(int id) {
        LazyInit();
        return enemyIDs[id];
    }

    public static Sprite GetBooster(int id) {
        LazyInit();
        return boosterIDs[id];
    }

    public static Sprite GetStatic(TileType type) {
        LazyInit();
        return staticSprites[type];
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
            SpriteListLoader loader = SpriteListLoader.Instance;
            foreach (Sprite s in loader.Enemies) {
                enemyIDs.Add(id++, s);
            }
            id = 0;
            foreach (Sprite s in loader.Boosters) {
                boosterIDs.Add(id++, s);
            }

            AddStaticSprite(TileType.WALL, loader.Wall);
            AddStaticSprite(TileType.UP_STAIRS, loader.UpStairs);
            AddStaticSprite(TileType.DOWN_STAIRS, loader.DownStairs);
            AddStaticSprite(TileType.GOLD_KEY, loader.GoldKey);
            AddStaticSprite(TileType.BLUE_KEY, loader.BlueKey);
            AddStaticSprite(TileType.RED_KEY, loader.RedKey);
            AddStaticSprite(TileType.PLAYER, loader.Player);
            AddStaticSprite(TileType.EXIT, loader.Exit);
            AddStaticSprite(TileType.GOLD_DOOR, loader.GoldDoor);
            AddStaticSprite(TileType.BLUE_DOOR, loader.BlueDoor);
            AddStaticSprite(TileType.RED_DOOR, loader.RedDoor);
            Util.Assert(staticSprites.Count == SerializationUtil.StaticTypeCount);
        }
    }

    private static void AddStaticSprite(TileType type, Sprite sprite) {
        Util.Assert(SerializationUtil.IsStaticType(type));
        staticSprites.Add(type, sprite);
    }

    private static Sprite LoadSprite(string name) {
        return Resources.Load<Sprite>("Sprites/" + name);
    }
}