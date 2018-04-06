using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectableLevel : MonoBehaviour {

    [SerializeField]
    private string destination;

    [SerializeField]
    private string levelName;

    [SerializeField]
    private int worldIndex;

    [SerializeField]
    private float stageIndex;

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text levelNameText;

    [SerializeField]
    private Text indexText;

    public void GoToLevel() {
        SceneManager.LoadScene(destination);
    }

    private void Start() {
        icon.sprite = sprite;
        levelNameText.text = levelName;
        indexText.text = string.Format("{0}-{1}", worldIndex, stageIndex);
    }
}