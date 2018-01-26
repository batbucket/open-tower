using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A tip is added to a gameobject
/// when one wants to display a tooltip on hoverover.
/// </summary>
/// <seealso cref="UnityEngine.MonoBehaviour" />
[RequireComponent(typeof(Image))]
public class Tip : MonoBehaviour {

    /// <summary>
    /// The body
    /// </summary>
    [SerializeField]
    private string body;

    /// <summary>
    /// Gets or sets the body.
    /// </summary>
    /// <value>
    /// The body.
    /// </value>
    public string Body {
        get {
            return body;
        }

        set {
            body = value;
        }
    }

    /// <summary>
    /// Setups the specified bundle.
    /// </summary>
    /// <param name="bundle">The bundle.</param>
    public void Setup(string body) {
        this.body = body;
    }

    /// <summary>
    /// Resets this instance.
    /// </summary>
    public void Reset() {
        this.body = string.Empty;
    }
}