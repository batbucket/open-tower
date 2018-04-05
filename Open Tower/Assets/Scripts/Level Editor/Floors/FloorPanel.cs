using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FloorPanel : Panel {
    private static FloorPanel _instance;

    [SerializeField]
    private Transform floorListingParent;

    [SerializeField]
    private Transform floorParent;

    [SerializeField]
    private FloorListing floorListingPrefab;

    [SerializeField]
    private EditableFloor floorPrefab;

    [SerializeField]
    private Scrollbar scroll;

    private FloorListing _selected;

    public static FloorPanel Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<FloorPanel>();
            }
            return _instance;
        }
    }

    private IList<FloorListing> Listings {
        get {
            return floorListingParent.GetComponentsInChildren<FloorListing>(true);
        }
    }

    private IList<EditableFloor> Floors {
        get {
            return floorParent.GetComponentsInChildren<EditableFloor>(true);
        }
    }

    public FloorListing Selected {
        set {
            _selected = value;

            foreach (EditableFloor ef in Floors) {
                ef.gameObject.SetActive(false);
            }
            if (value != null) {
                _selected.Associated.gameObject.SetActive(true);
            }
        }
        get {
            return _selected;
        }
    }

    public Transform FloorParent {
        get {
            return floorParent;
        }
    }

    public Transform FloorListingParent {
        get {
            return floorListingParent;
        }
    }

    public bool CanGoUp(int index) {
        return index < Listings.Count - 1;
    }

    public bool CanGoDown(int index) {
        return index > 0;
    }

    public bool IsFloorContainsType<T>(int indexToCheck, TileType type) where T : Element {
        EditableFloor floorToCheck = GetFloorAtIndex(indexToCheck);
        return floorToCheck.GetComponentsInChildren<T>(true).Any(e => e.IsType(type));
    }

    public void SetFloorEditability(bool isEditable) {
        foreach (EditableTile tile in floorParent.GetComponentsInChildren<EditableTile>(true)) {
            tile.SetButtonInteractivity(isEditable);
        }
    }

    public void AddFloor() {
        FloorListing listing = Instantiate(floorListingPrefab, floorListingParent);
        EditableFloor floor = Instantiate(floorPrefab, floorParent);

        listing.Init(Listings.Count - 1, floor);
        Selected = listing;
    }

    public void Delete(FloorListing item) {
        Destroy(item.Associated.gameObject);
        Destroy(item.gameObject);
        StartCoroutine(UpdateFloorIndices());
    }

    public void Swap(int item1, int item2) {
        FloorListing a = Listings[item1];
        FloorListing b = Listings[item2];
        int temp = a.Index;
        a.Index = b.Index;
        b.Index = temp;
        a.transform.SetSiblingIndex(item2);
        b.transform.SetSiblingIndex(item1);
        a.Associated.transform.SetSiblingIndex(item2);
        b.Associated.transform.SetSiblingIndex(item1);
    }

    private EditableFloor GetFloorAtIndex(int index) {
        return Floors[index];
    }

    private IEnumerator UpdateFloorIndices() {
        yield return new WaitForEndOfFrame();
        foreach (FloorListing listing in Listings) {
            listing.Index = listing.transform.GetSiblingIndex();
        }
    }

    public override void Clear() {
        foreach (FloorListing listing in floorListingParent.GetComponentsInChildren<FloorListing>(true)) {
            Delete(listing);
        }
        _selected = null;
    }
}