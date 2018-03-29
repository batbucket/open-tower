using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour {

    public void OnAnimationFinish() {
        Player.Instance.IsMovementEnabled = true;
        transform.parent.gameObject.SetActive(false);
    }
}