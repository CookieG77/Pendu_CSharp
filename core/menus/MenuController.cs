namespace PenduSharp.Core.Menus;

public class MenuController
{
    public static readonly MenuController Instance = new();
    
    private readonly Dictionary<string, AbstractMenu> _menus = new();
    private string? _currentMenu;
    
    public bool IsRunning { get; private set; } = true;
    
    private MenuController() {} // Private constructor to enforce singleton pattern

    public void Register(string key, AbstractMenu menu)
    {
        if(string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        ArgumentNullException.ThrowIfNull(menu);

        if (!_menus.TryAdd(key, menu))
            throw new ArgumentException($"A menu with the key '{key}' is already registered.", nameof(key));
    }

    /**
     * Gets a menu by its key.
     * Throws a KeyNotFoundException if no menu is found with the specified key.
     */
    public AbstractMenu GetMenu(string key)
    {
        return !_menus.TryGetValue(key, out var menu) ? throw new KeyNotFoundException($"No menu found with the key '{key}'.") : menu;
    }
    
    /**
     * Gets a menu by its key and casts it to the specified type T.
     * Throws a KeyNotFoundException if no menu is found with the specified key.
     * Throws an InvalidCastException if the menu found with the specified key cannot be cast to type T.
     */
    public T GetMenu<T>(string key) where T : AbstractMenu
    {
        return GetMenu(key) is not T typedMenu ? throw new InvalidCastException($"Menu with key '{key}' is not of type {typeof(T).Name}.") : typedMenu;
    }
    
    /**
     * Gets the currently active menu.
     * Throws an InvalidOperationException if no active menu is set.
     */
    public AbstractMenu GetCurrentMenu()
    {
        return _currentMenu is null ? throw new InvalidOperationException("No active menu set. Please set an active menu before trying to get the current menu.") : _menus[_currentMenu];
    }
    
    /**
     * Sets the active menu by its key. The specified menu must be registered in the controller.
     * Throws a KeyNotFoundException if no menu is found with the specified key.
     */
    public void SetActiveMenu(string key)
    {
        if (!_menus.ContainsKey(key))
            throw new KeyNotFoundException($"No menu found with the key '{key}'.");
        
        _currentMenu = key;
    }
    
    public void Stop()
    {
        IsRunning = false;
    }

    public void Run()
    {
        if (_currentMenu is null)
            throw new InvalidOperationException("No active menu set. Please set an active menu before running the controller.");

        while (IsRunning)
        {
            var currentMenu = _menus[_currentMenu];
            currentMenu.Run(this);
        }
    }
    
}