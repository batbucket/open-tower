using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextProcess : Process {

    [SerializeField]
    private string charName;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private string[] texts; // ensure that too much text is wrapped to next lines

    [SerializeField]
    private float waitTime;

    protected override IEnumerator PlayHelper() {
        yield return new WaitForSeconds(waitTime);
        Textbox.Instance.Enabled = true;
        yield return Textbox.Instance.LoadContent(icon, texts, charName);
        Textbox.Instance.Enabled = false;
    }
}