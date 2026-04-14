using PenduSharp.Core.SaveSystem;

namespace PenduSharp.Core.Menus;

public class LoadGameMenu() : MultiChoiceMenu("Load Game")
{
    private bool _noSavesAvailable = false;

    public override void Display(MenuController controller)
    {
        // Update the list of available saves each time the menu is displayed
        UpdateOptions();
        
        // If there are no saves available, set the flag to display the appropriate message
        Console.Clear();
        if (_noSavesAvailable)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No saved games available.");
            Console.ResetColor();
            Console.WriteLine("Press any key to return to the main menu.");
            return;
        }
        
        base.Display(controller);
    }

    public override void HandleInput(MenuController controller)
    {
        // If there are no saves available, wait for a key press and return to the main menu
        if (_noSavesAvailable)
        {
            Console.ReadKey();
            controller.SetActiveMenu("main_menu");
            return;
        }
        
        base.HandleInput(controller);
    }

    private void UpdateOptions()
    {
        var savedGames = SaveManager.GetAvailableSlots();
        if (savedGames.Count == 0)
        {
            _noSavesAvailable = true;
            Options.Clear();
            return;
        }
        
        Options.Clear();
        foreach (var save in savedGames)
        {
            _noSavesAvailable = false;
            var saveSlot = save; // Capture the current value of save for use in the lambda
            Options.Add(new MenuOption($"Save Slot {saveSlot}", controller =>
            {
                var gameState = SaveManager.Load(saveSlot);
                if (gameState != null)
                {
                    controller.GetMenu<GameMenu>("game_menu").LoadGame(gameState, saveSlot);
                    controller.SetActiveMenu("game_menu");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Failed to load game from slot {saveSlot}. Press any key to return to the load menu.");
                    Console.ResetColor();
                    Console.ReadKey();
                    controller.SetActiveMenu("load_game_menu");
                }
            }));
        }
        
        // Add an option to return to the main menu
        Options.Add(new MenuOption("Back to Main Menu", controller => { controller.SetActiveMenu("main_menu"); }));
    }
}