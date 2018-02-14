using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Element : MonoBehaviour {

    [SerializeField]
    private Image image;

    public int ID = int.MinValue;

    public Sprite Sprite {
        set {
            image.sprite = value;
        }
    }
}