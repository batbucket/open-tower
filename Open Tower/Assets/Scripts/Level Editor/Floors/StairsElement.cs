using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Special element variation for stairs that shows if stairs are illegal
public class StairsElement : Element {
    private static FloorPanel _floors;

    private static FloorPanel Floors {
        get {
            if (_floors == null) {
                _floors = FloorPanel.Instance;
            }
            return _floors;
        }
    }

    [SerializeField]
    private StairType type;

    public bool IsValid {
        get {
            int currentIndex = Floors.Selected.Index;
            bool isValidStairs = false;
            switch (type) {
                case StairType.GOES_UP:
                    isValidStairs = Floors.CanGoUp(currentIndex) && Floors.IsFloorContainsType<StairsElement>(currentIndex + 1, TileType.DOWN_STAIRS);
                    break;

                case StairType.GOES_DOWN:
                    isValidStairs = Floors.CanGoDown(currentIndex) && Floors.IsFloorContainsType<StairsElement>(currentIndex - 1, TileType.UP_STAIRS);
                    break;
            }
            return isValidStairs;
        }
    }

    private void Update() {
        image.color = IsValid ? Color.white : Color.red;
    }
}