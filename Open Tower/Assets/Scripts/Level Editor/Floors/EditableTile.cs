using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditableTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    private static EntitiesPanel _entitiesPanel;

    [SerializeField]
    private Image image;

    private bool isInteractive;

    private static EntitiesPanel EntitiesPanel {
        get {
            if (_entitiesPanel == null) {
                _entitiesPanel = FindObjectOfType<EntitiesPanel>();
            }
            return _entitiesPanel;
        }
    }

    public void SetButtonInteractivity(bool isInteractive) {
        this.isInteractive = isInteractive;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (!isInteractive) {
            return;
        }
        if (eventData.button == PointerEventData.InputButton.Left) {
            Clear();
            Interact();
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            Clear();
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) {
        if (!isInteractive) {
            return;
        }
        image.color = Color.cyan;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData) {
        image.color = Color.white;
    }

    private void Interact() {
        if (EntitiesPanel.LastSelected != null) {
            EntitiesPanel.LastSelected.CreateElement(transform);
        }
    }

    private void Clear() {
        foreach (Element e in transform.GetComponentsInChildren<Element>()) {
            Destroy(e.gameObject);
        }
    }

    private void Start() {
        this.isInteractive = true;
    }
}