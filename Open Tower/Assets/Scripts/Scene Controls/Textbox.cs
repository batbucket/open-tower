using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Textbox : MonoBehaviour {
    private static Textbox _instance;

    [SerializeField]
    private GameObject child;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private RectTransform iconRt;

    [SerializeField]
    private Text text;

    [SerializeField]
    private float iconMoveInDuration;

    [SerializeField]
    private float secondsPerCharacter;

    public static Textbox Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<Textbox>();
            }
            return _instance;
        }
    }

    public bool Enabled {
        set {
            child.SetActive(value);
        }
    }

    // magic happens here
    public IEnumerator LoadContent(Sprite sprite, string text) {
        // move in sprite
        Vector3 iconOriginalPos = icon.transform.localPosition;
        Vector3 iconDisplacedPos = new Vector3(-iconRt.rect.width, 0, 0);
        float timer = 0;
        while ((timer += Time.deltaTime) < iconMoveInDuration) {
            icon.transform.localPosition = Vector3.Lerp(iconDisplacedPos, iconOriginalPos, timer / iconMoveInDuration);
            yield return null;
        }
        // fancy typing algo goes here
        yield return null;
    }
}