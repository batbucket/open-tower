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
        LevelInfo level = LevelInfo.Instance;

        success.text = string.Format("<color=yellow>{1}</color>\nwas cleared!", level.Upload.AuthorName, level.Upload.LevelName);
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
        int stepCount = Player.Instance.Stats.StepCount;
        yield return AnimateScore(steps, stepCount, 0.25f);

        // Ranking and leaderboard
        yield return AnimateRank(level.Upload, stepCount);
    }

    private IEnumerator WaitThenDisplay(float waitTime, GameObject go) {
        yield return new WaitForSeconds(waitTime);
        go.SetActive(true);
        SoundManager.Instance.Play(displaySound);
    }

    private IEnumerator AnimateScore(Text target, int endScore, float duration) {
        float timer = 0;
        target.color = Color.grey;
        while ((timer += Time.deltaTime) < duration) {
            target.text = Mathf.CeilToInt(Mathf.Lerp(0, endScore, timer / duration)).ToString();
            yield return null;
        }
        target.color = Color.white;
        target.text = endScore.ToString();
        SoundManager.Instance.Play(scoreSound);
    }

    private IEnumerator AnimateRank(Upload upload, int stepCount) {
        int rankNumber = -1;
        bool isRankLoaded = false;
        List<Score> leaderboard = upload.Leaderboards;
        GameJolt.API.Objects.User currentUser = GameJolt.API.Manager.Instance.CurrentUser;

        bool isUserAuthor = (currentUser.ID == upload.AuthorID);
        GameJolt.API.Misc.GetTime(dateTime => {
            isRankLoaded = true;
            Score scoreToAdd = new Score(stepCount, dateTime.ToString(), currentUser.Name, currentUser.ID);
            bool isInserted = false;

            // Update successful attempts
            if (!isUserAuthor && !upload.CompletedUserIds.Contains(currentUser.ID)) {
                upload.CompletedUserIds.Add(currentUser.ID);
            }

            // Leaderboard updating

            // Find existing score by user
            Score existingRecord = leaderboard.Where(score => score.UserID == scoreToAdd.UserID).FirstOrDefault();

            if (existingRecord == null) { // Either count == 0 or needs to be appended to end
                rankNumber = leaderboard.Count;
                if (!isUserAuthor) {
                    leaderboard.Add(scoreToAdd);
                }
            } else if (existingRecord.CompareTo(scoreToAdd) < 0) { // existing record is not better
                // try to insert
                for (int i = 0; i < leaderboard.Count && !isInserted; i++) {
                    Score current = leaderboard[i];
                    if (scoreToAdd.CompareTo(current) < 0) {
                        if (!isUserAuthor) {
                            leaderboard.Insert(i, scoreToAdd);
                        }
                        rankNumber = i;
                        isInserted = true;
                    }
                }
            }
            GameJolt.API.DataStore.Set(upload.LevelName, JsonUtility.ToJson(upload), true, isSuccess => {
                isRankLoaded = isSuccess;
            });
        });
        yield return new WaitUntil(() => isRankLoaded);
        yield return AnimateScore(rank, rankNumber, 0.35f);
    }
}