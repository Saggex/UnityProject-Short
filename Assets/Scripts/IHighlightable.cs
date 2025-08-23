/// <summary>
/// Interface for objects that can be visually highlighted.
/// </summary>
public interface IHighlightable
{
    /// <summary>
    /// Toggles the highlight effect on or off.
    /// </summary>
    /// <param name="highlighted">If set to <c>true</c>, highlight the object.</param>
    void SetHighlighted(bool highlighted);
}
