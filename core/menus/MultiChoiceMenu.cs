using System;
using System.Collections.Generic;

namespace PenduSharp.Core.Menus;

public class MultiChoiceMenu : AbstractMenu
{
    
    public string Title { get; protected set; }
    public List<MenuOption> Options { get; protected set; } = new();

    protected MultiChoiceMenu(string title)
    {
        Title = title;
    }
    
    public override void Display(MenuController controller)
    {
        Console.Clear();
        var displayTitle = $"=== {Title} ===";
        Console.WriteLine(displayTitle);
        Console.WriteLine(new string('-', displayTitle.Length));
        
        for (var i = 0; i < Options.Count; i++)
        {
            var option = Options[i];
            Console.WriteLine($"{i + 1}. {option.Label}");
        }
    }
    
    public override void HandleInput(MenuController controller)
    {
        while (true)
        {
            Console.Write("Please select an option: ");
            var input = Console.ReadLine();

            if (int.TryParse(input, out var choice) && choice >= 1 && choice <= Options.Count)
            {
                Options[choice - 1].Action.Invoke(controller);
                break;
            }
            
            Console.WriteLine("Invalid input. Please enter a number corresponding to the options listed.");
        }
    }

    /**
     * Builder class for constructing MultiChoiceMenu instances in a fluent and flexible way.
     */
    public class Builder
    {
        private string _title = string.Empty;
        private readonly List<MenuOption> _options = [];
        
        public Builder WithTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));
            
            _title = title;
            return this;
        }

        /**
         * Adds one or more options to the menu.
         * Each option consists of a label and an associated action that will be executed when the option is selected.
         */
        public Builder WithOptions(params MenuOption[] options)
        {
            if (options == null || options.Length == 0)
                throw new ArgumentException("At least one option must be provided.", nameof(options));
            
            _options.AddRange(options);
            return this;
        }
        
        /**
         * Add a single option to the menu by providing a label and an action.
         */
        public Builder WithOption(string label, Action<MenuController> action)
        {
            if (string.IsNullOrWhiteSpace(label))
                throw new ArgumentException("Label cannot be null or empty.", nameof(label));
            
            if (action == null)
                throw new ArgumentNullException(nameof(action), "Action cannot be null.");
            
            _options.Add(new MenuOption(label, action));
            return this;
        }

        /**
         * Builds and returns a MultiChoiceMenu instance based on the provided title and options.
         */
        public MultiChoiceMenu Build()
        {
            if (string.IsNullOrWhiteSpace(_title))
                throw new InvalidOperationException("Title must be set before building the menu.");
            
            if (_options.Count == 0)
                throw new InvalidOperationException("At least one option must be added before building the menu.");
            
            return new MultiChoiceMenu(_title)
            {
                Options = _options
            };
        }
    }
}