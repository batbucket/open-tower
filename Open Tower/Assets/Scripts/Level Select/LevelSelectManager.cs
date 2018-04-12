using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour {
    private const int SELECT_ALL = 0;
    private static int lastUsedIndex = SELECT_ALL;

    [SerializeField]
    private GameObject levelParent;

    [SerializeField]
    private SelectableLevel prefab;

    [SerializeField]
    private Button[] worldSelectors;

    private void Start() {
        for (int i = SceneUtil.LEVEL_START_INDEX; i <= SceneUtil.LEVEL_END_INDEX; i++) {
            Instantiate(prefab, levelParent.transform).Init();
        }
        SetupButtons();

        if (lastUsedIndex == SELECT_ALL) {
            ShowAll(worldSelectors[0]);
        } else {
            ShowWorld(worldSelectors[lastUsedIndex], lastUsedIndex);
        }
    }

    private void SetupButtons() {
        for (int i = 0; i < worldSelectors.Length; i++) {
            Button b = worldSelectors[i];
            if (i == 0) {
                b.onClick.AddListener(() => ShowAll(b));
            } else {
                int j = i;
                b.onClick.AddListener(() => ShowWorld(b, j));
            }
        }
    }

    private void ShowAll(Button b) {
        lastUsedIndex = 0;
        levelParent
            .GetComponentsInChildren<SelectableLevel>(true)
            .ForEach(l => l.gameObject.SetActive(true));
        worldSelectors.ForEach(w => w.interactable = (w != b));
    }

    private void ShowWorld(Button b, int buttonIndex) {
        lastUsedIndex = buttonIndex;
        levelParent
            .GetComponentsInChildren<SelectableLevel>(true)
            .ForEach(l => l.gameObject.SetActive(l.WorldIndex == (buttonIndex - 1)));
        worldSelectors.ForEach(w => w.interactable = (w != b));
    }

    public void ShowLeaderboards() {
        GameJolt.UI.Manager.Instance.ShowLeaderboards();
    }

    public void ReturnToMainMenu() {
        SceneUtil.LoadScene(SceneUtil.MAIN_MENU_INDEX);
    }
}