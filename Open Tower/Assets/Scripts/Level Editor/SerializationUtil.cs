using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.LevelEditor.Serialization {

    public static class SerializationUtil {
        private const int NO_ELEMENT = -1;

        // Saving

        public static string GetSerializedDungeon(string name, string author, Transform floorsParent, EntitiesPanel entities, PlayerPanel player) {
            AddableTile[] addableTiles = entities.TileHolder.GetComponentsInChildren<AddableTile>(true);

            Addable[] addables = SerializeAddableTiles(addableTiles);
            Floor[] floors = GetSerializedFloors(addableTiles, floorsParent.GetComponentsInChildren<EditableFloor>(true));
            StartingValues startingValues = SerializeStartingValues(player);

            return JsonUtility.ToJson(new Dungeon(name, author, addables, floors, startingValues), true);
        }

        private static StartingValues SerializeStartingValues(PlayerPanel player) {
            return new StartingValues(
                player.Life,
                player.Power,
                player.Defense,
                player.Stars,
                player.GoldKeys,
                player.BlueKeys,
                player.RedKeys
                );
        }

        private static Addable[] SerializeAddableTiles(AddableTile[] addableTiles) {
            Addable[] addables = new Addable[addableTiles.Length];
            for (int i = 0; i < addableTiles.Length; i++) {
                AddableTile tile = addableTiles[i];
                if (tile.IsStaticTileType) {
                    addables[i] = new Addable(new StaticData(tile.TileType));
                } else {
                    addables[i] = GetDynamicAddable(tile);
                }
            }
            return addables;
        }

        private static Addable GetDynamicAddable(AddableTile tile) {
            Util.Assert(!tile.IsStaticTileType, "Tile not dynamic.");
            Addable addable = null;
            switch (tile.TileType) {
                case TileType.BOOSTER:
                    addable = new Addable(new BoosterData(
                        tile.SpriteID,
                        tile.BoostedStatType,
                        tile.BoostedAmount));
                    break;

                case TileType.ENEMY:
                    addable = new Addable(new EnemyData(
                        tile.SpriteID,
                        tile.EnemyLife,
                        tile.EnemyPower,
                        tile.EnemyDefense,
                        tile.EnemyStars));
                    break;
            }
            return addable;
        }

        private static Floor[] GetSerializedFloors(AddableTile[] addables, EditableFloor[] floors) {
            Floor[] serializedFloors = new Floor[floors.Length];
            for (int i = 0; i < floors.Length; i++) {
                serializedFloors[i] = GetSerializedFloor(addables, floors[i]);
            }
            return serializedFloors;
        }

        private static Floor GetSerializedFloor(AddableTile[] addables, EditableFloor floor) {
            EditableTile[] tiles = floor.GetComponentsInChildren<EditableTile>(true);
            int[] IDs = new int[tiles.Length];

            for (int i = 0; i < tiles.Length; i++) {
                Element possible = tiles[i].GetComponentInChildren<Element>(true);
                if (possible == null) { // nothing here
                    IDs[i] = NO_ELEMENT;
                } else { // store index
                    IDs[i] = ArrayUtility.FindIndex(addables, a => possible.IsSource(a));
                }
            }
            return new Floor(IDs);
        }

        // Loading into level editor
        public static void DeserializeDungeon(
            string json,
            EntitiesPanel entities,
            PlayerPanel player,
            FloorPanel floors
            ) {
            Dungeon dungeon = JsonUtility.FromJson<Dungeon>(json);

            // setup starting values
            StartingValues values = dungeon.StartingValues;
            player.Init(
                values.Life,
                values.Power,
                values.Defense,
                values.Stars,
                values.GoldKeys,
                values.BlueKeys,
                values.RedKeys);

            // setup entities

            // setup floors
        }

        // Loading into game
        public static void DeserializeDungeon(
            string json,
            string exitScene,
            DungeonInfo infoTarget,
            GameObject floorsParent,
            GameObject floorPrefab,
            GameObject wallPrefab,
            GameObject upstairsPrefab,
            GameObject downstairsPrefab,
            GameObject goldKeyPrefab,
            GameObject blueKeyPrefab,
            GameObject redKeyPrefab,
            GameObject playerPrefab,
            GameObject exitPrefab,
            GameObject goldDoorPrefab,
            GameObject blueDoorPrefab,
            GameObject redDoorPrefab,
            GameObject enemyPrefab,
            GameObject boosterPrefab
            ) {
            Debug.Log(json);
            Dungeon dungeon = JsonUtility.FromJson<Dungeon>(json);

            string name = dungeon.Name;
            string author = dungeon.Author;
            Addable[] addables = dungeon.Addables;
            Floor[] floors = dungeon.Floors;
            StartingValues startingValues = dungeon.StartingValues;

            infoTarget.Init(name, author, exitScene);

            for (int i = 0; i < floors.Length; i++) {
                GameObject floor = GameObject.Instantiate(floorPrefab, floorsParent.transform);
                floor.transform.localPosition = Vector3.zero;
                floor.SetActive(false);
                Tile[] tiles = floor.GetComponentsInChildren<Tile>(true);
                int[] indices = floors[i].Indices;
                for (int j = 0; j < indices.Length; j++) {
                    int index = indices[j];
                    Tile current = tiles[j];

                    if (index != NO_ELEMENT) {
                        Addable addable = addables[index];
                        GameObject instantiated = null;

                        if (addable.AddableType == AddableType.STATIC) {
                            StaticData data = addable.StaticData;
                            switch (data.TileType) {
                                case TileType.WALL:
                                    instantiated = GameObject.Instantiate(wallPrefab, current.transform);
                                    break;

                                case TileType.UP_STAIRS:
                                    instantiated = GameObject.Instantiate(upstairsPrefab, current.transform);
                                    break;

                                case TileType.DOWN_STAIRS:
                                    instantiated = GameObject.Instantiate(downstairsPrefab, current.transform);
                                    break;

                                case TileType.GOLD_KEY:
                                    instantiated = GameObject.Instantiate(goldKeyPrefab, current.transform);
                                    break;

                                case TileType.BLUE_KEY:
                                    instantiated = GameObject.Instantiate(blueKeyPrefab, current.transform);
                                    break;

                                case TileType.RED_KEY:
                                    instantiated = GameObject.Instantiate(redKeyPrefab, current.transform);
                                    break;

                                case TileType.PLAYER:
                                    instantiated = GameObject.Instantiate(playerPrefab, current.transform);
                                    instantiated.GetComponent<Player>().Init(startingValues);
                                    floor.SetActive(true);
                                    break;

                                case TileType.EXIT:
                                    instantiated = GameObject.Instantiate(exitPrefab, current.transform);
                                    instantiated.GetComponent<Exit>().Init(exitScene);
                                    break;

                                case TileType.GOLD_DOOR:
                                    instantiated = GameObject.Instantiate(goldDoorPrefab, current.transform);
                                    break;

                                case TileType.BLUE_DOOR:
                                    instantiated = GameObject.Instantiate(blueDoorPrefab, current.transform);
                                    break;

                                case TileType.RED_DOOR:
                                    instantiated = GameObject.Instantiate(redDoorPrefab, current.transform);
                                    break;

                                default:
                                    Util.Assert(false, "Unhandled type: {0}", data.TileType);
                                    break;
                            }
                        } else if (addable.AddableType == AddableType.ENEMY) {
                            EnemyData data = addable.EnemyData;
                            instantiated = GameObject.Instantiate(enemyPrefab, current.transform);
                            instantiated.GetComponent<Stats>().Init(data.Life, data.Power, data.Defense, data.Stars);
                            instantiated.GetComponentInChildren<SpriteRenderer>().sprite = SpriteList.GetEnemy(data.SpriteID);
                        } else if (addable.AddableType == AddableType.BOOSTER) {
                            BoosterData data = addable.BoosterData;
                            instantiated = GameObject.Instantiate(boosterPrefab, current.transform);
                            instantiated.GetComponent<Pickup>().Init(data.StatToBoost, data.AmountBoosted);
                            instantiated.GetComponentInChildren<SpriteRenderer>().sprite = SpriteList.GetBooster(data.SpriteID);
                        } else {
                            Util.Assert(false, "Unhandled type: {0}", addable.GetType());
                        }
                    }
                }
            }
        }
    }
}