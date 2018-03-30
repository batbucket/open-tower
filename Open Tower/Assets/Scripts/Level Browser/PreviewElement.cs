using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewElement : MonoBehaviour {

    [SerializeField]
    private Image image;

    public Sprite Sprite {
        set {
            image.sprite = value;
        }
    }

    public bool IsVisible {
        set {
            image.color = value ? Color.white : Color.clear;
        }
    }
}