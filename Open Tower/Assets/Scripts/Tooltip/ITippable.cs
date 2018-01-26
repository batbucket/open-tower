/// <summary>
/// Interface for classes that have tooltippable content.
/// </summary>
public interface ITippable {

    /// <summary>
    /// Gets the tooltip information.
    /// </summary>
    /// <value>
    /// The tooltip information.
    /// </value>
    Tip Tip {
        get;
    }
}