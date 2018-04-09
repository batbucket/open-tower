using Scripts.LevelEditor.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONLevel : MonoBehaviour {

    [SerializeField]
    private TextAsset levelJson; // NOT upload

    [SerializeField]
    private int scoreID = 0;

    [SerializeField]
    private string stage;

    [SerializeField]
    private string levelName;

    private Upload _upload;

    public int ScoreID {
        get {
            return scoreID;
        }
    }

    public Upload Upload {
        get {
            return new Upload(levelJson.text, levelName, int.MinValue, DateTime.MinValue.ToString());
        }
    }

    public string Stage {
        get {
            return stage;
        }
    }

    public string LevelName {
        get {
            return levelName;
        }
    }
}