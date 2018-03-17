using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyShowInEditor : MonoBehaviour {

    private void Start() {
        if (!Application.isEditor) {
            this.gameObject.SetActive(false);
        }
    }
}