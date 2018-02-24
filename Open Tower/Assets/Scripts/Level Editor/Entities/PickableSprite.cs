using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickableSprite : MonoBehaviour {

    [SerializeField]
    private Button button;

    [SerializeField]
    private Image image;

    private int id;

    public Sprite Sprite {
        get {
            return image.sprite;
        }
    }

    public int ID {
        get {
            return id;
        }
    }

    public void Init(int id, Sprite sprite) {
        this.id = id;
        this.image.sprite = sprite;
    }

    public void SetButtonAction(Action action) {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(new UnityEngine.Events.UnityAction(action));
    }
}