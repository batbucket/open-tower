using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsManager : MonoBehaviour {
    private static ResultsManager _instance;

    public static ResultsManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<ResultsManager>();
            }
            return _instance;
        }
    }

    [SerializeField]
    private Transform window;

    [SerializeField]
    private float successFadeInDuration = 0.25f;

    [SerializeField]
    private float windowTransitionDuration = 0.1f;

    [SerializeField]
    private Text success;

    [SerializeField]
    private Button exit;

    [SerializeField]
    private Text results;

    [SerializeField]
    private GameObject[] stats;

    [SerializeField]
    private Text time;

    [SerializeField]
    private Text steps;

    [SerializeField]
    private Text rank;

    [SerializeField]
    private AudioClip displaySound;

    [SerializeField]
    private AudioClip scoreSound;

    private string destination;

    public void ShowResults(string destination) {
        StartCoroutine(ResultsAnim());
        this.destination = destination;
    }

    public void ChangeScene() {
        SceneManager.LoadScene(destination);

        LevelInfo info = LevelInfo.Instance;
        if (info != null) {
            info.IsLevelCleared = true;
        }
    }

    private IEnumerator ResultsAnim() {
        success.gameObject.SetActive(true);
        yield return Util.Lerp(successFadeInDuration, t => success.color = Color.Lerp(Color.clear, Color.white, t));
        window.gameObject.SetActive(true);
        yield return Util.Lerp(windowTransitionDuration, t => window.transform.localScale = Vector2.Lerp(Vector2.up, Vector2.one, t));
        exit.gameObject.SetActive(true);
        yield return WaitThenDisplay(1f, results.gameObject);
        foreach (GameObject display in stats) {
            yield return WaitThenDisplay(0.25f, display);
        }
        yield return AnimateScore(time, (int)Time.timeSinceLevelLoad, 0.25f);
        yield return AnimateScore(steps, Player.Instance.Stats.StepCount, 0.25f);
        yield return AnimateScore(rank, 100, 0.25f);
        exit.gameObject.SetActive(true);
    }

    private IEnumerator WaitThenDisplay(float waitTime, GameObject go) {
        yield return new WaitForSeconds(waitTime);
        go.SetActive(true);
        SoundManager.Instance.Play(displaySound);
    }

    private IEnumerator AnimateScore(Text target, int endScore, float duration) {
        float timer = 0;
        while ((timer += Time.deltaTime) < duration) {
            target.text = Mathf.CeilToInt(Mathf.Lerp(-999, endScore, timer / duration)).ToString();
            yield return null;
        }
        target.text = endScore.ToString();
        SoundManager.Instance.Play(scoreSound);
    }
}