using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Element : MonoBehaviour {

    [SerializeField]
    private Image image;

    private AddableTile source;

    public void Init(AddableTile source) {
        this.source = source;
        Update();
    }

    public void RemoveFromSourceListing() {
        source.RemoveFromListing(this);
    }

    private void Update() {
        this.image.sprite = source.Sprite;
    }
}