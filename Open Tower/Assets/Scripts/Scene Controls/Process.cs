using System.Collections;
using UnityEngine;

public abstract class Process : MonoBehaviour {

    public abstract IEnumerator Play();
}