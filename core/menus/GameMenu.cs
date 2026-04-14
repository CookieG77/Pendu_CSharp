using PenduSharp.Core.SaveSystem;

namespace PenduSharp.Core.Menus;

public class GameMenu : AbstractMenu
{
    public GameState? CurrentGameState { get; private set; }
    
    private int? _currentSaveSlot;
    
    public void StartNewGame(int wordListIndex)
    {
        if (wordListIndex < 0 || wordListIndex >= AssetLoader.WordLists.Count)
            throw new ArgumentOutOfRangeException(nameof(wordListIndex), $"Word list index must be between 0 and {AssetLoader.WordLists.Count - 1}.");

        var wordList = AssetLoader.WordLists[wordListIndex];
        var random = new Random();
        var wordToGuess = wordList.Words[random.Next(wordList.Words.Count)];

        CurrentGameState = new GameState
        {
            Word = wordToGuess
        };
    }
    
    public void LoadGame(GameState gameState, int saveSlot)
    {
        CurrentGameState = gameState ?? throw new ArgumentNullException(nameof(gameState), "Game state cannot be null.");
        _currentSaveSlot = saveSlot;
    }

    public override void Display(MenuController controller)
    {
        if (CurrentGameState == null)
        {
            Console.WriteLine("No game in progress. Please start a new game from the difficulty selection menu.");
            return;
        }
        Console.Clear();
        
        // Display the ASCII art based on the number of attempts left
        var asciiIndex = AssetLoader.AsciiImages.Count - 1 - CurrentGameState.AttemptsLeft;
        asciiIndex = Math.Clamp(asciiIndex, 0, AssetLoader.AsciiImages.Count - 1);
        var asciiArt = AssetLoader.AsciiImages[asciiIndex];
        asciiArt.Print();
        
        var displayWord = string.Concat(CurrentGameState.Word.Select(c => CurrentGameState.GuessedLetters.Contains(c) ? $"{c} " : "_ "));
        Console.WriteLine($"Word: {displayWord.Trim()}");
        Console.WriteLine($"Attempts Left: {CurrentGameState.AttemptsLeft}");
        Console.WriteLine("Type 'save' to save your game, or 'exit' to return to the main menu.");
    }

    public override void HandleInput(MenuController controller)
    {
        if (CurrentGameState == null)
        {
            Console.WriteLine("Press any key to return to the menu.");
            Console.ReadKey();
            
            controller.SetActiveMenu("main_menu");
            return;
        }

        while (true)
        {
            var input = Console.ReadLine();
            
            // Handle special commands like "save" or "exit"
            if (string.Equals(input, "save", StringComparison.OrdinalIgnoreCase))
            {
                while (true)
                {
                    Console.WriteLine("Please choose a save slot (1-3) or type 'cancel' to return:");
                    var slotInput = Console.ReadLine();
                    
                    if (string.Equals(slotInput, "cancel", StringComparison.OrdinalIgnoreCase))
                    {
                        Display(controller);
                        Console.WriteLine("Save cancelled. Returning to game...");
                        break;
                    }
                    
                    if (int.TryParse(slotInput, out var slotNumber) && slotNumber is >= 1 and <= 3)
                    {
                        SaveManager.Save(CurrentGameState, slotNumber);
                        Display(controller);
                        Console.WriteLine($"Game saved to slot {slotNumber}.");
                        break;
                    }
                    
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 3, or 'cancel' to return.");
                }
            }
            else if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting to main menu...");
                Thread.Sleep(3000); // Wait for 3 seconds before returning to the main menu
                controller.SetActiveMenu("main_menu");
            }
            
            else if (input.Length == 1 && char.IsLetter(input[0]))
            {
                var guessedLetter = char.ToLower(input[0]);
                
                if (CurrentGameState.GuessedLetters.Contains(guessedLetter))
                {
                    Display(controller);
                    Console.WriteLine($"You have already guessed the letter '{guessedLetter}'. Please try a different letter.");
                    continue;
                }
                
                CurrentGameState.GuessedLetters.Add(guessedLetter);
                
                if (!CurrentGameState.Word.Contains(guessedLetter))
                {
                    CurrentGameState.AttemptsLeft--;
                }
                
                Display(controller);
                
                // Check for win condition
                if (CurrentGameState.Word.All(c => CurrentGameState.GuessedLetters.Contains(c)))
                {
                    if (_currentSaveSlot != null) // If the game was loaded from a save slot, delete that save slot upon winning
                    {
                        var slotToDelete = _currentSaveSlot.Value;
                        SaveManager.Delete(slotToDelete);
                        _currentSaveSlot = null;
                    }
                    Console.WriteLine("Congratulations! You've guessed the word!");
                    Console.WriteLine("Press any key to return to the main menu.");
                    Console.ReadKey();
                    controller.SetActiveMenu("main_menu");
                    break;
                }
                
                // Check for lose condition
                if (CurrentGameState.AttemptsLeft <= 0)
                {
                    Console.WriteLine($"Game Over! The word was '{CurrentGameState.Word}'.");
                    Console.WriteLine("Press any key to return to the main menu.");
                    Console.ReadKey();
                    controller.SetActiveMenu("main_menu");
                    break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a single letter, or type 'save' or 'exit'.");
            }
        }
    }
}