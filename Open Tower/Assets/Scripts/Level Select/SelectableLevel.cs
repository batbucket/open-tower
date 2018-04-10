using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectableLevel : MonoBehaviour {

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

    public void Init() {
        int index = transform.GetSiblingIndex();
        this.destination = index + SceneUtil.LEVEL_START_INDEX;
        LevelParams level = SceneUtil.GetParams(index);

        levelNameText.text = level.Name;
        indexText.text = string.Format("{0}-{1}", level.WorldIndex, level.StageIndex);
    }
}