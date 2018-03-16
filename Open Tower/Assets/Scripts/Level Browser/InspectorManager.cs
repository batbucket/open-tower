using Scripts.LevelEditor.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectorManager : MonoBehaviour {
    private const int PLACEHOLDER_PLAYER_INDEX = -2;

    [SerializeField]
    private Text nameDisplay;

    [SerializeField]
    private Transform tileParent;

    [SerializeField]
    private LeaderboardListing leaderboardListingPrefab;

    [SerializeField]
    private Transform leaderboardParent;

    [SerializeField]
    private Transform panel;

    private PreviewElement[] elements;
    private Upload current;

    public void SetLevel(Upload upload) {
        this.current = upload;
        panel.gameObject.SetActive(true);
        GameJolt.API.Users.Get(upload.AuthorID, user => {
            nameDisplay.text = string.Format("<b><color=yellow>{0}</color></b>'s\n{1}", user.Name, upload.LevelName);
        });

        // setup leaderboard
        Util.KillAllChildren(leaderboardParent);
        List<Score> leaderboard = upload.Leaderboards;
        for (int i = 0; i < leaderboard.Count; i++) {
            Score score = leaderboard[i];
            int ranking = i; // prevent i from being passed into callback
            GameJolt.API.Users.Get(score.UserID, user => {
                if (upload.LevelName.Equals(current.LevelName)) { // prevent past level's listings from popping up if we've already switched to a new level
                    Instantiate(leaderboardListingPrefab, leaderboardParent)
                        .Init(user.Avatar,
                        ranking,
                        score.Username,
                        score.Steps,
                        score.DateAchieved);
                }
            });
        }

        // setup preview
        Dungeon dungeon = JsonUtility.FromJson<Dungeon>(upload.LevelJson);
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

    private void Start() {
        elements = tileParent.GetComponentsInChildren<PreviewElement>();
    }
}