using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorListing : MonoBehaviour {
    private static FloorPanel floorPanel;

    [SerializeField]
    private Text _number;

    [SerializeField]
    private Button up;

    [SerializeField]
    private Button down;

    [SerializeField]
    private Button delete;

    [SerializeField]
    private Outline outline;

    private EditableFloor _associated;

    private int _index;

    public EditableFloor Associated {
        get {
            return _associated;
        }
    }

    public int Index {
        set {
            this._index = value;
            _number.text = value.ToString();
            Associated.FloorNumber = value;
        }
        get {
            return _index;
        }
    }

    public void SelectThis() {
        floorPanel.Selected = this;
    }

    public void MoveUp() {
        floorPanel.Swap(this.Index, this.Index + 1);
    }

    public void MoveDown() {
        floorPanel.Swap(this.Index, this.Index - 1);
    }

    public void Delete() {
        floorPanel.Delete(this);
    }

    public void Init(int index, EditableFloor associated) {
        this._associated = associated;
        this.Index = index;
    }

    private void Start() {
        if (floorPanel == null) {
            floorPanel = FloorPanel.Instance;
        }
    }

    private void Update() {
        down.interactable = floorPanel.CanGoDown(Index);
        up.interactable = floorPanel.CanGoUp(Index);
        outline.effectColor = (floorPanel.Selected == this) ? Color.green : Color.white;
    }
}