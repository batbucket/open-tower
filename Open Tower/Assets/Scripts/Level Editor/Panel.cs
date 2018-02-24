using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Panel : MonoBehaviour {

    [SerializeField]
    private TabMode mode;

    [SerializeField]
    private Button button;

    public TabMode Mode {
        get {
            return mode;
        }
    }

    public void SetActive(bool isActive) {
        gameObject.SetActive(isActive);
        button.interactable = !isActive;
    }
}