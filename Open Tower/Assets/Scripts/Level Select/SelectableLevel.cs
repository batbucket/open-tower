using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectableLevel : MonoBehaviour {

    [SerializeField]
    private string levelName;

    [SerializeField]
    private int worldIndex;

    [SerializeField]
    private char stageIndex;

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text levelNameText;

    [SerializeField]
    private Text indexText;

    private int destination;

    public void GoToLevel() {
        SceneUtil.Play = PlayType.LEVEL_SELECT;
        SceneManager.LoadScene(destination);
    }

    private void Start() {
        this.destination = transform.GetSiblingIndex() + SceneUtil.LEVEL_START_INDEX;
        icon.sprite = sprite;
        levelNameText.text = levelName;
        indexText.text = string.Format("{0}-{1}", worldIndex, stageIndex);
    }
}