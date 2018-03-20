using Scripts.LevelEditor.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPreview : MonoBehaviour {
    private const int PLACEHOLDER_PLAYER_INDEX = -2;

    [SerializeField]
    private Transform tileParent;

    private PreviewElement[] elements;

    public void Clear() {
        foreach (PreviewElement element in elements) {
            element.Sprite = null;
        }
    }

    public void Init(Dungeon dungeon) {
        Addable[] addables = dungeon.Addables;
        int playerIndex = PLACEHOLDER_PLAYER_INDEX;

        // find player index
        for (int i = 0; i < addables.Length && playerIndex == PLACEHOLDER_PLAYER_INDEX; i++) {
            Addable addable = addables[i];
            if (addable.AddableType == AddableType.STATIC && addable.StaticData.TileType == TileType.PLAYER) {
                playerIndex = i;
            }
        }

        // find floor with player
        int[] startingFloor = null;

        Floor[] dungeonFloors = dungeon.Floors;
        for (int i = 0; i < dungeonFloors.Length && startingFloor == null; i++) {
            int[] indices = dungeonFloors[i].Indices;
            for (int j = 0; j < indices.Length && startingFloor == null; j++) {
                int index = indices[j];
                if (index == playerIndex) {
                    startingFloor = indices;
                }
            }
        }

        // Who submits an empty level???
        if (startingFloor != null) {
            // setup preview, startingFloor length should match preview
            for (int i = 0; i < startingFloor.Length; i++) {
                PreviewElement element = elements[i];
                int index = startingFloor[i];

                element.IsVisible = (index != SerializationUtil.NO_ELEMENT);
                if (index != SerializationUtil.NO_ELEMENT) {
                    Addable addable = addables[index];
                    Sprite sprite = null;
                    switch (addable.AddableType) {
                        case AddableType.STATIC:
                            sprite = SpriteList.GetStatic(addable.StaticData.TileType);
                            break;

                        case AddableType.BOOSTER:
                            sprite = SpriteList.GetBooster(addable.BoosterData.SpriteID);
                            break;

                        case AddableType.ENEMY:
                            sprite = SpriteList.GetEnemy(addable.EnemyData.SpriteID);
                            break;
                    }
                    element.Sprite = sprite;
                }
            }
        }
    }

    public void Init(Upload upload) {
        // setup preview
        Dungeon dungeon = JsonUtility.FromJson<Dungeon>(upload.LevelJson);
        Init(dungeon);
    }

    private void Awake() {
        elements = tileParent.GetComponentsInChildren<PreviewElement>();
    }
}