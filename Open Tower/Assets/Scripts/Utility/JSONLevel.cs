using Scripts.LevelEditor.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONLevel : MonoBehaviour {

    [SerializeField]
    private TextAsset levelJson; // NOT upload

    private Upload _upload;

    public Upload Upload {
        get {
            return new Upload(levelJson.text, string.Empty, int.MinValue, DateTime.MinValue.ToString());
        }
    }
}