using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntitiesPanel : Panel {
    private static EntitiesPanel _instance;

    /// <summary>
    /// The prefabs. Should be parallel to the dropdown!
    /// </summary>
    [SerializeField]
    private AddableTile[] prefabs;

    [SerializeField]
    private Dropdown dropdown;

    [SerializeField]
    private Transform tileHolder;

    [SerializeField]
    private Scrollbar scroll;

    [SerializeField]
    private float scrollDuration = 0.50f;

    private Coroutine current;

    [HideInInspector]
    public AddableTile LastSelected;

    public static EntitiesPanel Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<EntitiesPanel>();
            }
            return _instance;
        }
    }

    public void AddTile() {
        Instantiate(prefabs[dropdown.value], tileHolder);
        if (current != null) {
            StopCoroutine(current);
        }
        current = StartCoroutine(AnimatedScrollToBottom());
    }

    private IEnumerator AnimatedScrollToBottom() {
        float timer = 0;
        float startValue = scroll.value;
        while ((timer += Time.deltaTime) < scrollDuration) {
            scroll.value = Mathf.SmoothStep(startValue, 0, timer / scrollDuration);
            yield return null;
        }
        scroll.value = 0;
    }
}