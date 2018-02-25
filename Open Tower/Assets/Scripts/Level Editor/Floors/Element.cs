using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Element : MonoBehaviour {

    [SerializeField]
    protected Image image;

    private AddableTile source;

    public Sprite Sprite {
        set {
            image.sprite = value;
        }
    }

    public bool IsSource(AddableTile source) {
        return this.source == source;
    }

    public bool IsType(TileType type) {
        return source.TileType == type;
    }

    public void Init(AddableTile source) {
        this.source = source;
        Sprite = source.Sprite;
    }
}