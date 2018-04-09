using Scripts.LevelEditor.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectorManager : MonoBehaviour {
    private const int PLACEHOLDER_PLAYER_INDEX = -2;

    [SerializeField]
    private LevelPreview preview;

    [SerializeField]
    private Text nameDisplay;

    [SerializeField]
    private LeaderboardListing leaderboardListingPrefab;

    [SerializeField]
    private Transform leaderboardParent;

    [SerializeField]
    private Transform panel;

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

        preview.Init(upload);
    }
}