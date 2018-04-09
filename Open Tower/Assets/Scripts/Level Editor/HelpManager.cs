using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpManager : MonoBehaviour {

    [SerializeField]
    private GameObject window;

    public void SetWindow(bool isOpen) {
        window.SetActive(isOpen);
    }
}