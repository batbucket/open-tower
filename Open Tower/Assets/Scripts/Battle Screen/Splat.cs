using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Splat : MonoBehaviour {

    [SerializeField]
    private Text text;

    [SerializeField]
    private AudioClip sound;

    public void Init(int amount, Transform parent) {
        if (amount < 0) {
            text.color = Color.red;
        } else if (amount > 0) {
            text.color = Color.green;
        } else {
            text.color = Color.white;
        }
        text.text = amount.ToString("+#;-#;0");
        transform.SetParent(parent);
        transform.localPosition = new Vector3(25, 0, 0);
        SoundManager.Instance.Play(sound);
        StartCoroutine(Animate());
    }

    private IEnumerator Animate() {
        text.transform.localScale = Vector3.zero;
        yield return Util.Lerp(0.05f, t => {
            text.transform.localScale = Vector3.Slerp(Vector3.zero, new Vector3(1.5f, 1.5f, 1.5f), t);
        });
        yield return Util.Lerp(0.10f, t => {
            text.transform.localScale = Vector3.Slerp(new Vector3(1.5f, 1.5f, 1.5f), Vector3.one, t);
        });
        yield return new WaitForSeconds(2f);
        yield return Util.Lerp(0.10f, t => {
            text.transform.localScale = Vector3.Slerp(Vector3.one, Vector3.zero, t);
        });
        text.transform.localScale = Vector3.zero;
        Destroy(this.gameObject);
    }
}