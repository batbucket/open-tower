using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Panel : MonoBehaviour {

    [SerializeField]
    private TabMode mode;

    [SerializeField]
    private Button button;

    [SerializeField]
    private Transform holder;

    public TabMode Mode {
        get {
            return mode;
        }
    }

    public void SetActive(bool isActive) {
        holder.gameObject.SetActive(isActive);
        button.interactable = !isActive;
    }

    public virtual void OnEnter() {
    }

    public virtual void OnExit() {
    }
}