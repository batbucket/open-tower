using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Textbox : MonoBehaviour {
    private static Textbox _instance;

    private string TAG_START;

    private const string TAG_CLOSE = "</color>";

    private const char BLOCK_CHARACTER = '█';

    private const char MESSAGE_SPLIT_CHARACTER = '/';

    [SerializeField]
    private GameObject child;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private AudioClip sound;

    [SerializeField]
    private Image background;

    [SerializeField]
    private RectTransform iconRt;

    [SerializeField]
    private Text text;

    [SerializeField]
    private Text shadow;

    [SerializeField]
    private Text nameBox;

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
    public IEnumerator LoadContent(Sprite sprite, string[] messages, string charName = "") {
        // move in sprite
        icon.sprite = sprite;
        Vector3 iconOriginalPos = icon.transform.localPosition;
        Vector3 iconDisplacedPos = new Vector3(-iconRt.rect.width, 0, 0);
        float timer = 0;
        nameBox.text = charName;
        text.text = string.Empty;
        shadow.text = string.Empty;

        while ((timer += Time.deltaTime) < iconMoveInDuration) {
            icon.transform.localPosition = Vector3.Lerp(iconDisplacedPos, iconOriginalPos, timer / iconMoveInDuration);
            yield return null;
        }
        icon.transform.localPosition = iconOriginalPos;

        for (int i = 0; i < messages.Length; i++) {
            isDone = false;
            text.text = string.Empty;
            shadow.text = string.Empty;
            // fancy typing algo goes here
            string message = messages[i];

            for (int j = 0; j < message.Length + 1; j++) {
                text.text = string.Format("{0}{1}", message.Insert(j, TAG_START), TAG_CLOSE);
                if (j < message.Length && char.IsLetterOrDigit(message[j])) {
                    SoundManager.Instance.Play(sound);
                }
                yield return new WaitForSeconds(secondsPerCharacter);
            }
            shadow.text = message;

            text.text += " ";

            while (!isDone) {
                text.text += BLOCK_CHARACTER;
                shadow.text += BLOCK_CHARACTER;
                if (!isDone) {
                    yield return WaitForInput(secondsPerBlink);
                }
                // get rid of last block character
                text.text = text.text.Substring(0, text.text.Length - 1);
                shadow.text = shadow.text.Substring(0, shadow.text.Length - 1);
                if (!isDone) {
                    yield return WaitForInput(secondsPerBlink);
                }
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