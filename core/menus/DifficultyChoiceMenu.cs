namespace PenduSharp.Core.Menus;

public class DifficultyChoiceMenu : MultiChoiceMenu
{
    public DifficultyChoiceMenu() : base("Choose Difficulty")
    {
        for (var i = 0; i < AssetLoader.WordLists.Count; i++)
        {
            var wordList = AssetLoader.WordLists[i];
            
            var index = i; // Capture the current value of i for use in the lambda
            Options.Add(new MenuOption($"{wordList.DisplayName} ({wordList.Words.Count} words)", controller =>
            {
                controller.GetMenu<GameMenu>("game_menu").StartNewGame(index);
                controller.SetActiveMenu("game_menu");
            }));
        }

        Options.Add(new MenuOption("Back to Main Menu", controller => { controller.SetActiveMenu("main_menu"); }));
    }
}