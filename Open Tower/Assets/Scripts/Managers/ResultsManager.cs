using Scripts.LevelEditor.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private Text pastStepsLabel;

    [SerializeField]
    private Text pastRankingLabel;

    [SerializeField]
    private int customScoreID = 0;

    private int destinationScene;

    private int scoreIDOverride;

    public void ShowResults(int destinationScene) {
        StartCoroutine(ResultsAnim());
        this.destinationScene = destinationScene;
    }

    public void ChangeScene() {
        SceneManager.LoadScene(destinationScene);

        LevelInfo info = LevelInfo.Instance;
        if (info != null) {
            info.IsLevelCleared = true;
        }
    }

    private void Start() {
        JSONLevel level = FindObjectOfType<JSONLevel>();
        if (level != null) {
            Util.Assert(level.ScoreID != 0 || scoreIDOverride != 0, "Score ID was not set.");
            scoreIDOverride = level.ScoreID + scoreIDOverride;
        }
    }

    private IEnumerator ResultsAnim() {
        foreach (TutorialBoard tb in FindObjectsOfType<TutorialBoard>()) {
            Destroy(tb.gameObject);
        }
        DungeonInfo info = DungeonInfo.Instance;

        success.text = string.Format("<color=yellow>{0}</color>\nwas cleared!", info.LevelName);
        success.gameObject.SetActive(true);
        yield return Util.Lerp(successFadeInDuration, t => success.color = Color.Lerp(Color.clear, Color.white, t));
        window.gameObject.SetActive(true);
        yield return Util.Lerp(windowTransitionDuration, t => window.transform.localScale = Vector2.Lerp(Vector2.up, Vector2.one, t));
        exit.gameObject.SetActive(true);
        yield return WaitThenDisplay(1f, results.gameObject);
        foreach (GameObject display in stats) {
            yield return WaitThenDisplay(0.25f, display);
        }
        yield return Util.AnimateScore(time, 0, (int)Time.timeSinceLevelLoad, 0.25f, scoreSound);
        int stepCount = Player.Instance.Stats.StepCount;
        yield return Util.AnimateScore(steps, 0, stepCount, 0.25f, scoreSound);

        // Ranking and leaderboard
        yield return AnimateRank(stepCount);
    }

    private IEnumerator WaitThenDisplay(float waitTime, GameObject go) {
        yield return new WaitForSeconds(waitTime);
        go.SetActive(true);
        SoundManager.Instance.Play(displaySound);
    }

    private IEnumerator AnimateRank(int stepCount) {
        int calculatedRank = -1;
        Score previousBestScore = null;
        int previousBestRank = -1;
        bool isRankLoaded = false;
        GameJolt.API.Misc.GetTime(dateTime => {
            if (scoreIDOverride == 0) {
                Upload upload = LevelInfo.Instance.Upload;
                List<Score> leaderboard = upload.Leaderboards;
                GameJolt.API.Objects.User currentUser = GameJolt.API.Manager.Instance.CurrentUser;

                bool isUserAuthor = (currentUser.ID == upload.AuthorID);
                upload.AddToLeaderboard(currentUser, stepCount, dateTime, out calculatedRank, out previousBestScore, out previousBestRank, isSuccess => {
                    isRankLoaded = true;
                });
            } else {
                GameJolt.API.Scores.GetRank(stepCount, scoreIDOverride, rank => {
                    Debug.Log("Rank load successful: " + rank);
                    calculatedRank = rank - 1;
                    GameJolt.API.Scores.Add(stepCount, stepCount.ToString(), scoreIDOverride);
                    isRankLoaded = true;
                });
            }
        });
        yield return new WaitUntil(() => isRankLoaded);
        yield return Util.AnimateScore(rank, 0, calculatedRank, 0.35f, scoreSound);

        if (previousBestScore != null) {
            pastStepsLabel.gameObject.SetActive(true);
            pastStepsLabel.color = Color.yellow;
            pastStepsLabel.text = string.Format("Old steps: {0}", previousBestScore.Steps);
            pastRankingLabel.gameObject.SetActive(true);
            pastRankingLabel.color = Color.yellow;
            pastRankingLabel.text = string.Format("Old rank: {0}", previousBestRank);
        }
    }
}