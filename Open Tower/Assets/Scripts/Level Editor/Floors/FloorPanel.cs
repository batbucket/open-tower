using System.Collections;
using System.Collections.Generic;
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
            return floorListingParent.GetComponentsInChildren<FloorListing>();
        }
    }

    private IList<EditableFloor> Floors {
        get {
            return floorParent.GetComponentsInChildren<EditableFloor>();
        }
    }

    public FloorListing Selected {
        set {
            _selected = value;

            foreach (EditableFloor ef in Floors) {
                ef.gameObject.SetActive(false);
            }
            _selected.Associated.gameObject.SetActive(true);
        }
        get {
            return _selected;
        }
    }

    public bool CanGoUp(int index) {
        return index < Listings.Count - 1;
    }

    public bool CanGoDown(int index) {
        return index > 0;
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
    }

    private void Start() {
        AddFloor();
    }

    private IEnumerator UpdateFloorIndices() {
        yield return new WaitForEndOfFrame();
        foreach (FloorListing listing in Listings) {
            listing.Index = listing.transform.GetSiblingIndex();
        }
    }
}