using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardListing : MonoBehaviour {

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text rank;

    [SerializeField]
    private Text username;

    [SerializeField]
    private Text stepCount;

    [SerializeField]
    private Text date;

    public void Init(Sprite icon, int rank, string username, int stepCount, string date) {
        this.icon.sprite = icon;
        this.rank.text = rank.ToString();
        this.username.text = username;
        this.stepCount.text = stepCount.ToString();
        this.date.text = DateTime.Parse(date).ToString("MM/dd/yy\nhh:mm tt");
    }
}