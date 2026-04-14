using PenduSharp.Core.Menus;
using PenduSharp.Core.SaveSystem;

namespace PenduSharp.Core;

/**
 * Main class of the application, responsible for initializing the menu system and starting the application loop.
 */
public static class Pendu
{
    public static void StartGame()
    {
        // ======= Menu initialization =======

        var menuController = MenuController.Instance; // Get the singleton instance of the MenuController
        
        menuController.Register("main_menu", new MultiChoiceMenu.Builder()
            .WithTitle("Main Menu")
            .WithOption("Start Game", controller => { controller.SetActiveMenu("difficulty_menu"); })
            .WithOption("Load Game", controller => { controller.SetActiveMenu("load_game_menu"); })
            .WithOption("Exit", controller => { controller.Stop(); })
            .Build()
        );
        
        menuController.Register("difficulty_menu", new DifficultyChoiceMenu());

        menuController.Register("load_game_menu", new LoadGameMenu());
        
        menuController.Register("game_menu", new GameMenu());
        
        // ======= Start the application =======
        
        menuController.SetActiveMenu("main_menu");
        menuController.Run();
    }
}