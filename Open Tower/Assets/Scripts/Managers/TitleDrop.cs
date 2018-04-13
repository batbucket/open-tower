using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleDrop : MonoBehaviour {

    [SerializeField]
    private Text text;

    private void Start() {
        if (SceneUtil.IsCurrentLevelIndex) {
            this.Init(SceneUtil.GetParams(SceneUtil.LevelIndex).Name);
        }
        this.GetComponent<Canvas>().sortingOrder = 1;
        Rect rect = this.GetComponent<RectTransform>().rect;
        rect.width = 300;
        rect.height = 300;
    }

    private void Init(string title) {
        text.text = title;
        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut() {
        Debug.Log("hello");
        yield return Util.Lerp(0.5f, t => {
            text.color = Color.Lerp(Color.clear, Color.white, t);
        });
        text.color = Color.white;
        yield return new WaitForSeconds(2f);
        yield return Util.Lerp(0.5f, t => {
            text.color = Color.Lerp(Color.white, Color.clear, t);
        });
        text.color = Color.clear;
        Destroy(this.gameObject);
    }
}