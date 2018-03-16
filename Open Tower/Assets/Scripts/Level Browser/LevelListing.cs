using Scripts.LevelEditor.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelListing : MonoBehaviour {

    [SerializeField]
    private Color selectedColor;

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

    private Upload upload;

    private Color normalColor;

    public Upload Upload {
        get {
            return upload;
        }
    }

    public void SelectThisLevel() {
        LevelBrowserManager.Instance.SelectLevel(this);
    }

    public void Init(string uploadJson) {
        this.upload = JsonUtility.FromJson<Upload>(uploadJson);
        levelName.text = upload.LevelName;
        string leader;
        if (upload.Leaderboards.Count > 0) {
            leader = upload.Leaderboards[0].Username;
        } else {
            leader = "<color=grey>Nobody!</color>";
        }

        GameJolt.API.Users.Get(upload.AuthorID, user => {
            authorAndLeader.text = string.Format("<color=yellow>{0}</color>\n{1}", user.Name, leader);
        });

        DateTime date = DateTime.Parse(upload.DateCreated);
        string dateString = date.ToString("MM/dd/yy\nhh:mm tt");
        dateCreated.text = dateString;

        int tryCount = upload.AttemptedUserIds.Count;
        int clearCount = upload.CompletedUserIds.Count;

        clearsAndTries.text = string.Format("{0}\n<color=grey>{1}</color>", clearCount, tryCount);
    }

    private void Start() {
        this.normalColor = background.color;
    }

    private void Update() {
        Color color = normalColor;
        if (LevelBrowserManager.Instance.IsLevelBeingSelected(this)) {
            color = selectedColor;
        }
        background.color = color;
    }
}