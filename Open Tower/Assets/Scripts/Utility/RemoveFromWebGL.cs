using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFromWebGL : MonoBehaviour {

    private void Start() {
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            this.gameObject.SetActive(false);
        }
    }
}