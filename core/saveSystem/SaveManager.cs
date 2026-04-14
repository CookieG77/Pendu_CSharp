using System.Text.Json;

namespace PenduSharp.Core;

public static class SaveManager
{
    private static string GetPath(int slot)
    {
        return $"save{slot}.txt";
    }

    public static void Save(GameState state, int slot)
    {
        if (slot < 1 || slot > 3)
        {
            throw new ArgumentException("Slot must be 1, 2 or 3");
        }
        
        var json = JsonSerializer.Serialize(
            state,
            new JsonSerializerOptions{WriteIndented = true}
        );
        
        File.WriteAllText(GetPath(slot), json);
    }

    public static GameState Load(int slot)
    {
        if (slot < 1 || slot > 3)
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