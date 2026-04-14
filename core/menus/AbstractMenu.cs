namespace PenduSharp.Core.Menus;

/**
 * Represents a generic menu in the console application, which can be extended to create specific types of menus (e.g., main menu, settings menu, etc.).
 */
public abstract class AbstractMenu
{
    public abstract void Display(MenuController controller);
    public abstract void HandleInput(MenuController controller);

    public virtual void Run(MenuController controller)
    {
        Display(controller);
        HandleInput(controller);
    }
}