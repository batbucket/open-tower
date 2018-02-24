using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditableFloor : MonoBehaviour {

    [SerializeField]
    private Text floorDisplay;

    public int FloorNumber {
        set {
            floorDisplay.text = value.ToString();
        }
    }

    public void Init(int floor) {
        FloorNumber = floor;
    }
}