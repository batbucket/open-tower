using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : Panel {
    private const string SUCCESS = "<color=lime>SUCCESS!</color>";

    private const string FAILURE = "<color=red>FAILED!</color>";

    private const string PLAYER_CHECK = "Player-in-level check: {0}\n";
    private const string STAIRS_CHECK = "Stairs-all-valid check: {0}\n";
    private const string EXIT_CHECK = "Exit-in-level check: {0}";

    [SerializeField]
    private Text error;

    [SerializeField]
    private Transform floorsParent;

    [SerializeField]
    private Button playtest;

    private bool isLevelValidated;

    public void CheckIfLevelIsValid() {
        bool isPlayerPlaced = IsPlayerPlaced();
        bool isStairsValid = IsStairsValid();
        bool isExitPlaced = IsExitPlaced();

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(PLAYER_CHECK, isPlayerPlaced ? SUCCESS : FAILURE);
        sb.AppendFormat(STAIRS_CHECK, isStairsValid ? SUCCESS : FAILURE);
        sb.AppendFormat(EXIT_CHECK, isExitPlaced ? SUCCESS : FAILURE);

        error.text = sb.ToString();
        isLevelValidated = isPlayerPlaced && isStairsValid && isExitPlaced;
    }

    public void StartPlaytest() {
        throw new NotImplementedException();
    }

    public override void OnEnter() {
        isLevelValidated = false;
        SetFloorEditability(false);
    }

    public override void OnExit() {
        SetFloorEditability(true);
    }

    private void SetFloorEditability(bool isEditable) {
        foreach (EditableTile tile in floorsParent.GetComponentsInChildren<EditableTile>(true)) {
            tile.SetButtonInteractivity(isEditable);
        }
    }

    private bool IsStairsValid() {
        return floorsParent.GetComponentsInChildren<StairsElement>(true)
            .All(stairs => stairs.IsValid);
    }

    private bool IsPlayerPlaced() {
        return floorsParent.GetComponentsInChildren<Element>(true).Any(element => element.IsType(TileType.PLAYER));
    }

    private bool IsExitPlaced() {
        return floorsParent.GetComponentsInChildren<Element>(true).Any(element => element.IsType(TileType.EXIT));
    }

    private void Update() {
        playtest.interactable = isLevelValidated;
    }
}