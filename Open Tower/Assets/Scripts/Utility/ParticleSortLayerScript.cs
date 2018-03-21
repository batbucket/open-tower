using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSortLayerScript : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private void Start() {
        Renderer psr = GetComponent<ParticleSystem>().GetComponent<Renderer>();
        psr.sortingLayerID = spriteRenderer.sortingLayerID;
        psr.sortingOrder = spriteRenderer.sortingOrder + 1;
    }
}