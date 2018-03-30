using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorManager : MonoBehaviour {

    [SerializeField]
    private RectTransform rt;

    [SerializeField]
    private GridLayoutGroup grid;

    private int rows;
    private int columns;

    private Tile[] children;

    public int Rows {
        get {
            if (rows == 0) {
                int cellWidth = Mathf.RoundToInt(grid.cellSize.x);
                this.rows = Mathf.RoundToInt(rt.rect.width) / cellWidth;
            }
            return rows;
        }
    }

    public int Columns {
        get {
            if (columns == 0) {
                int cellHeight = Mathf.RoundToInt(grid.cellSize.y);
                this.columns = Mathf.RoundToInt(rt.rect.height) / cellHeight;
            }
            return columns;
        }
    }

    private void Awake() {
        this.children = GetComponentsInChildren<Tile>(true);
    }

    public Tile GetTileAtIndex(int index) {
        return children[index];
    }

    public bool IsRowColumnValid(int row, int column) {
        return
            row >= 0
            && row < Rows
            && column >= 0
            && column < Columns;
    }

    public int GetRowOfIndex(int index) {
        Util.Assert(0 <= index && index < children.Length, "Invalid index {0} for array of length={1}", index, children.Length);
        return index / Columns;
    }

    public int GetColumnOfIndex(int index) {
        Util.Assert(0 <= index && index < children.Length, "Invalid index {0} for array of length={1}", index, children.Length);
        return index % Columns;
    }

    public Tile GetTileAtPosition(int row, int column) {
        Util.Assert(row < Rows, "Input row {0} is not in range [0, {1}]", row, Rows - 1);
        Util.Assert(column < Columns, "Input column {0} is not in range [0, {1}]", column, Columns - 1);
        return children[row * Columns + column];
    }
}