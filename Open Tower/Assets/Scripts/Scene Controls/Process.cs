using System.Collections;
using UnityEngine;

public abstract class Process : MonoBehaviour {

    public IEnumerator Play() {
        yield return PlayHelper();
    }

    protected abstract IEnumerator PlayHelper();
}