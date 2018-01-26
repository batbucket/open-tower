using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Tooltip box for displaying tips
/// </summary>
/// <seealso cref="UnityEngine.MonoBehaviour" />
public class TooltipBox : MonoBehaviour {

    /// <summary>
    /// The body
    /// </summary>
    [SerializeField]
    private Text body;

    /// <summary>
    /// The backdrop
    /// </summary>
    [SerializeField]
    private RectTransform backdrop;

    /// <summary>
    /// The rt
    /// </summary>
    [SerializeField]
    private RectTransform rt;

    /// <summary>
    /// The outline
    /// </summary>
    [SerializeField]
    private Outline outline;

    /// <summary>
    /// Gets the outline.
    /// </summary>
    /// <value>
    /// The outline.
    /// </value>
    public Outline Outline {
        get {
            return outline;
        }
    }

    /// <summary>
    /// Sets the body.
    /// </summary>
    /// <value>
    /// The body.
    /// </value>
    public string Body {
        set {
            body.text = value;
        }
    }

    /// <summary>
    /// Sets the position.
    /// </summary>
    /// <value>
    /// The position.
    /// </value>
    public Vector2 Position {
        set {
            transform.position = value;
        }
    }

    /// <summary>
    /// Sets the pivot.
    /// </summary>
    /// <value>
    /// The pivot.
    /// </value>
    public Vector2 Pivot {
        set {
            rt.pivot = value;
        }
    }
}