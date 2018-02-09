using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditableTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField]
    private Outline outline;

    public void Interact() {
        Util.KillAllChildren(transform);
        Element selected = Instantiate(LevelEditorManager.Instance.Selected);
        selected.transform.SetParent(transform);
        selected.transform.localPosition = Vector3.zero;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) {
        outline.enabled = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData) {
        outline.enabled = false;
    }
}