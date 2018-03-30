using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProcess : Process {

    [SerializeField]
    private SpriteRenderer renderer;

    [SerializeField]
    private bool isRendererEnabled;

    [SerializeField]
    private float waitTime;

    protected override IEnumerator PlayHelper() {
        yield return new WaitForSeconds(waitTime);
        renderer.enabled = isRendererEnabled;
    }
}
