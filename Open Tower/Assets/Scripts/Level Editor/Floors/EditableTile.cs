using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditableTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private static EntitiesPanel _entitiesPanel;

    [SerializeField]
    private Outline outline;

    private static EntitiesPanel EntitiesPanel {
        get {
            if (_entitiesPanel == null) {
                _entitiesPanel = FindObjectOfType<EntitiesPanel>();
            }
            return _entitiesPanel;
        }
    }

    public void Interact() {
        foreach (Element e in transform.GetComponentsInChildren<Element>()) {
            Destroy(e.gameObject);
        }
        EntitiesPanel.LastSelected.CreateElement(transform);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) {
        outline.enabled = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData) {
        outline.enabled = false;
    }
}