using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStat : MonoBehaviour {

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text countText;

    private int startingCount;
    private int _count;

    public int Count {
        get {
            return _count;
        }
        private set {
            countText.text = value.ToString();
            this._count = value;
        }
    }

    public void Init(int count) {
        startingCount = count;
        Count = count;
    }

    public IEnumerator ChangeCount(float shakeIntensity, float scaleIntensity, int target, float duration) {
        bool isCountingDone = false;
        this.Count = target;
        StartCoroutine(Util.AnimateScore(countText, Count, target, duration, null, () => {
            isCountingDone = true;
        }));
        bool isShakingDone = false;
        StartCoroutine(Util.ShakeItem(shakeIntensity, scaleIntensity, duration, () => {
            isShakingDone = true;
        }, countText.transform, icon.transform));
        yield return new WaitUntil(() => isCountingDone && isShakingDone);
    }

    private void Update() {
        if (startingCount != 0) {
            countText.color = Color.Lerp(Color.red, Color.white, ((float)Count) / startingCount);
        }
    }
}