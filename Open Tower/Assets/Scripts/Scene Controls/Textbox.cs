using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Textbox : MonoBehaviour {
    private static Textbox _instance;

    private string TAG_START;

    private const string TAG_CLOSE = "</color>";

    private const char BLOCK_CHARACTER = '█';

    [SerializeField]
    private GameObject child;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Image background;

    [SerializeField]
    private RectTransform iconRt;

    [SerializeField]
    private Text text;

    [SerializeField]
    private float iconMoveInDuration;

    [SerializeField]
    private float secondsPerCharacter;

    [SerializeField]
    private float secondsPerBlink;

    private bool isDone;

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

    private void Start() {
        TAG_START = string.Format("<color=#{0}>", ColorUtility.ToHtmlStringRGB(background.color));
    }

    // magic happens here
    public IEnumerator LoadContent(Sprite sprite, string message) {
        isDone = false;
        // move in sprite
        Vector3 iconOriginalPos = icon.transform.localPosition;
        Vector3 iconDisplacedPos = new Vector3(-iconRt.rect.width, 0, 0);
        float timer = 0;
        while ((timer += Time.deltaTime) < iconMoveInDuration) {
            icon.transform.localPosition = Vector3.Lerp(iconDisplacedPos, iconOriginalPos, timer / iconMoveInDuration);
            yield return null;
        }

        // fancy typing algo goes here
        for (int i = 0; i < message.Length + 1; i++) {
            text.text = string.Format("{0}{1}", message.Insert(i, TAG_START), TAG_CLOSE);
            yield return new WaitForSeconds(secondsPerCharacter);
        }

        while (!isDone) {
            text.text += BLOCK_CHARACTER;
            if (!isDone) {
                yield return WaitForInput(secondsPerBlink);
            }
            text.text = text.text.Substring(0, text.text.Length - 1);
            if (!isDone) {
                yield return WaitForInput(secondsPerBlink);
            }
        }

        yield return null;
    }

    private IEnumerator WaitForInput(float secondsToWait) {
        float timer = 0;
        while ((timer += Time.deltaTime) < secondsToWait) {
            if (Input.anyKeyDown) {
                isDone = true;
                yield break;
            }
            yield return null;
        }
    }
}