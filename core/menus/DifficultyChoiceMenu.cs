namespace PenduSharp.Core.Menus;

public class DifficultyChoiceMenu : MultiChoiceMenu
{
    public DifficultyChoiceMenu(string title) : base(title)
    {
        for (var i = 0; i < AssetLoader.WordLists.Count; i++)
        {
            var wordList = AssetLoader.WordLists[i];
            
            Options.Add(new MenuOption($"{wordList.DisplayName} ({wordList.Words.Count} words)", controller =>
            {
                controller.GetMenu<GameMenu>("game_menu").StartNewGame(i);
                controller.SetActiveMenu("game_menu");
            }));
        }

        Options.Add(new MenuOption("Back to Main Menu", controller => { controller.SetActiveMenu("main_menu"); }));
    }
}