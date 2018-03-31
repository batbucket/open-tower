using System.Collections.Generic;
using UnityEngine;

namespace Scripts.LevelEditor.Serialization {

    public static class SerializationUtil {
        public const int NO_ELEMENT = -1;

        // Util
        // Static types have no customizable values
        private static readonly HashSet<TileType> STATIC_TYPES = new HashSet<TileType>() {
                TileType.WALL,
                TileType.UP_STAIRS,
                TileType.DOWN_STAIRS,
                TileType.GOLD_KEY,
                TileType.BLUE_KEY,
                TileType.RED_KEY,
                TileType.PLAYER,
                TileType.EXIT,
                TileType.GOLD_DOOR,
                TileType.BLUE_DOOR,
                TileType.RED_DOOR
            };

        public static int StaticTypeCount {
            get {
                return STATIC_TYPES.Count;
            }
        }

        public static bool IsStaticType(TileType type) {
            return STATIC_TYPES.Contains(type);
        }

        // Saving

        public static string GetSerializedDungeon(Transform floorsParent, EntitiesPanel entities, PlayerPanel player) {
            AddableTile[] addableTiles = entities.TileHolder.GetComponentsInChildren<AddableTile>(true);

            Addable[] addables = SerializeAddableTiles(addableTiles);
            Floor[] floors = GetSerializedFloors(addableTiles, floorsParent.GetComponentsInChildren<EditableFloor>(true));
            StartingValues startingValues = SerializeStartingValues(player);

            return JsonUtility.ToJson(new Dungeon(addables, floors, startingValues));
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
                    IDs[i] = addables.IndexOf(a => possible.IsSource(a));
                }
            }
            return new Floor(IDs);
        }

        // Loading into level editor
        public static void DeserializeDungeonToEditor(
            string json,
            EntitiesPanel entitiesPanel,
            GameObject boosterPrefab,
            GameObject enemyPrefab,
            PlayerPanel playerPanel,
            FloorPanel floorPanel,
            GameObject floorListingPrefab,
            GameObject floorPrefab,
            GameObject elementPrefab,
            GameObject upStairsPrefab,
            GameObject downStairsPrefab
            ) {
            Dungeon dungeon = JsonUtility.FromJson<Dungeon>(json);

            // setup starting values
            StartingValues values = dungeon.StartingValues;
            playerPanel.Init(
                values.Life,
                values.Power,
                values.Defense,
                values.Stars,
                values.GoldKeys,
                values.BlueKeys,
                values.RedKeys);

            // setup entities
            Transform tileHolder = entitiesPanel.TileHolder;
            Addable[] addables = dungeon.Addables;
            foreach (Addable addable in addables) {
                GameObject go;
                AddableTile at;
                switch (addable.AddableType) {
                    case AddableType.BOOSTER:
                        BoosterData booster = addable.BoosterData;
                        go = GameObject.Instantiate(boosterPrefab, tileHolder);
                        at = go.GetComponent<AddableTile>();
                        at.ChooseBoostedStat(booster.StatToBoost);
                        at.BoostedAmount = booster.AmountBoosted;
                        at.SetSprite(booster.SpriteID, AddableType.BOOSTER);
                        break;

                    case AddableType.ENEMY:
                        EnemyData enemy = addable.EnemyData;
                        go = GameObject.Instantiate(enemyPrefab, tileHolder);
                        at = go.GetComponent<AddableTile>();
                        at.SetSprite(enemy.SpriteID, AddableType.ENEMY);
                        at.InitEnemy(enemy.Life, enemy.Power, enemy.Defense, enemy.Stars);
                        break;
                }
            }

            AddableTile[] addableTiles = tileHolder.GetComponentsInChildren<AddableTile>();
            FloorListing firstFloorListing = null;

            // setup floors
            Floor[] floors = dungeon.Floors;
            for (int i = 0; i < floors.Length; i++) {
                Floor floor = floors[i];
                GameObject floorGo = GameObject.Instantiate(floorPrefab, floorPanel.FloorParent);
                GameObject floorListingGo = GameObject.Instantiate(floorListingPrefab, floorPanel.FloorListingParent);
                FloorListing floorListing = floorListingGo.GetComponent<FloorListing>();
                EditableFloor editableFloor = floorGo.GetComponent<EditableFloor>();
                floorListing.Init(i, editableFloor);

                if (i == 0) { // first floor
                    firstFloorListing = floorListing;
                }

                EditableTile[] tiles = floorGo.GetComponentsInChildren<EditableTile>(true);
                int[] indices = floor.Indices;
                // same range as serialized array
                for (int j = 0; j < tiles.Length; j++) {
                    EditableTile tile = tiles[j];
                    int index = indices[j];
                    if (index != NO_ELEMENT) {
                        AddableTile source = addableTiles[index];
                        GameObject chosenPrefab = null;

                        // up and down stairs use a special prefab
                        if (source.TileType == TileType.UP_STAIRS) {
                            chosenPrefab = upStairsPrefab;
                        } else if (source.TileType == TileType.DOWN_STAIRS) {
                            chosenPrefab = downStairsPrefab;
                        } else {
                            chosenPrefab = elementPrefab;
                        }
                        GameObject elementGo = GameObject.Instantiate(chosenPrefab, tile.transform);
                        elementGo.GetComponent<Element>().Init(source);
                    }
                }
            }
            floorPanel.Selected = firstFloorListing; // do this last so the other floors are properly disabled
        }

        // Loading into game
        public static void DeserializeDungeonToPlayable(
            Upload upload,
            string stage,
            string sceneOnVictory,
            string sceneOnExit,
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
            Dungeon dungeon = JsonUtility.FromJson<Dungeon>(upload.LevelJson);

            Addable[] addables = dungeon.Addables;
            Floor[] floors = dungeon.Floors;
            StartingValues startingValues = dungeon.StartingValues;

            infoTarget.Init(stage, upload.LevelName, sceneOnExit);

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
                                    instantiated.GetComponent<Exit>().Init(sceneOnVictory);
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