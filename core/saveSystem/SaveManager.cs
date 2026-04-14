using System.Text.Json;

namespace PenduSharp.Core.SaveSystem;

public static class SaveManager
{
    // System specific path to store save files, using AppData for user-specific data
    private static readonly string SaveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PenduSharp");
    
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };
    
    /**
     * Utility method to ensure that the save directory exists before attempting to read or write save files.
     */
    private static void EnsureSaveDirectoryExists()
    {
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }
    }
    
    /**
     * Gets the file path for the given save slot (1-3).
     * The save files are named "save1.json", "save2.json", and "save3.json" and are stored in the SaveDirectory.
     */
    private static string GetPath(int slot)
    {
        return Path.Combine(SaveDirectory, $"save{slot}.json");
    }

    /**
     * Saves the given game state to the specified save slot (1-3).
     * If a save file already exists in that slot, it will be overwritten.
     */
    public static void Save(GameState state, int slot)
    {
        if (slot is < 1 or > 3)
        {
            throw new ArgumentException("Slot must be 1, 2 or 3");
        }
        
        var json = JsonSerializer.Serialize(
            state,
            JsonOptions
        );
        
        EnsureSaveDirectoryExists();
        
        File.WriteAllText(GetPath(slot), json);
    }

    /**
     * Loads the game state from the specified save slot (1-3).
     * If the slot is empty or the file does not exist, returns null.
     */
    public static GameState? Load(int slot)
    {
        if (slot is < 1 or > 3)
        {
            throw new ArgumentException("Slot must be 1, 2 or 3");
        }
        
        var path = GetPath(slot);

        if (!File.Exists(path)) return null;
        
        EnsureSaveDirectoryExists();
        
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<GameState>(json) ?? new GameState();
    }
    
    /**
     * Checks if a save file exists for the given slot (1-3).
     * Returns true if a save file exists, false otherwise.
     */
    public static bool SlotExists(int slot)
    {
        EnsureSaveDirectoryExists();
        
        return File.Exists(GetPath(slot));
    }
    
    /**
     * Gets a list of available save slots (1-3) that currently have saved game data.
     * If no slots are available, returns an empty list.
     */
    public static List<int> GetAvailableSlots()
    {
        EnsureSaveDirectoryExists();
        
        var availableSlots = new List<int>();
        for (var i = 1; i <= 3; i++)
        {
            if (SlotExists(i))
            {
                availableSlots.Add(i);
            }
        }
        return availableSlots;
    }

    /**
     * Deletes the save file for the specified slot (1-3) if it exists.
     */
    public static void Delete(int slot)
    {
        EnsureSaveDirectoryExists();
        
        var path = GetPath(slot);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}