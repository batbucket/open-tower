using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credit : MonoBehaviour {

    [SerializeField]
    private new SpriteRenderer renderer;

    [SerializeField]
    private new Text name;

    [SerializeField]
    private Text role;

    public void Init(Sprite sprite, string name, string role, Color a, Color b) {
        this.renderer.color = a;
        this.renderer.material.color = b;
        this.renderer.sprite = sprite;
        this.name.text = name;
        this.role.text = role;
    }
}