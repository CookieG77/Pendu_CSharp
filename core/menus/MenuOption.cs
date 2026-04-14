namespace PenduSharp.Core.Menus;

public sealed class MenuOption
{
    public string Label { get; }
    public Action<MenuController> Action { get; }

    public MenuOption(string label, Action<MenuController> action)
    {
        if (string.IsNullOrWhiteSpace(label))
            throw new ArgumentException("Label cannot be null or empty.", nameof(label));

        Action = action ?? throw new ArgumentNullException(nameof(action));
        Label = label;
    }
}