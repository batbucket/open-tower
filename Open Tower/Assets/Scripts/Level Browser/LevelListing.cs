using Scripts.LevelEditor.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelListing : MonoBehaviour {

    [SerializeField]
    private Text levelName;

    [SerializeField]
    private Text authorAndLeader;

    [SerializeField]
    private Text dateCreated;

    [SerializeField]
    private Text clearsAndTries;

    [SerializeField]
    private Image background;

    [SerializeField]
    private string levelJson;

    public void Init(string key) {
        GameJolt.API.DataStore.Get(key, true, json => {
            Upload upload = JsonUtility.FromJson<Upload>(json);
            levelName.text = upload.LevelName;
            string leader;
            if (upload.Leaderboards.Length > 0) {
                leader = upload.Leaderboards[0].User;
            } else {
                leader = "<color=grey>Nobody!</color>";
            }
            authorAndLeader.text = string.Format("<color=yellow>{0}</color>\n{1}", upload.AuthorName, leader);

            DateTime date = DateTime.Parse(upload.DateCreated);
            string dateString = date.ToString("MM/dd/yy\nhh:mm tt");
            dateCreated.text = dateString;

            int clearCount = upload.UsersAttempted.Length;
            int tryCount = upload.UsersCompleted.Length;

            clearsAndTries.text = string.Format("{0}\n<color=grey>{1}</color>", clearCount, tryCount);
        });
    }
}