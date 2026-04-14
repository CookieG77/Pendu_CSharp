using System.Text.Json;

namespace PenduSharp.Core.SaveSystem;

public static class SaveManager
{
    // System specific path to store save files, using AppData for user-specific data
    private static readonly string SaveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PenduSharp");
    
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };
    
    private static string GetPath(int slot)
    {
        return Path.Combine(SaveDirectory, $"save{slot}.txt");
    }

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
        
        File.WriteAllText(GetPath(slot), json);
    }

    public static GameState? Load(int slot)
    {
        if (slot is < 1 or > 3)
        {
            throw new ArgumentException("Slot must be 1, 2 or 3");
        }
        
        var path = GetPath(slot);

        if (!File.Exists(path)) return null;
        
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<GameState>(json) ?? new GameState();
    }
    
    public static bool SlotExists(int slot)
    {
        return File.Exists(GetPath(slot));
    }

    public static void Delete(int slot)
    {
        var path = GetPath(slot);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}