using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextProcess : Process {

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private string[] texts; // ensure that too much text is wrapped to next lines

    protected override IEnumerator PlayHelper() {
        Textbox.Instance.Enabled = true;
        yield return Textbox.Instance.LoadContent(icon, texts);
        Textbox.Instance.Enabled = false;
    }
}