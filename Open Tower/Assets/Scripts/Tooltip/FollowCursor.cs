using UnityEngine;

/// <summary>
/// Code from http://answers.unity3d.com/questions/903716/object-follow-cursor-how-to-access-mouse-cursor-x.html
/// </summary>
public class FollowCursor : MonoBehaviour {

    [SerializeField]
    private RectTransform parent;

    // Update is called once per frame
    private void Update() {
        this.transform.position = Input.mousePosition;
    }
}