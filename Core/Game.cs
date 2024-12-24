internal class Game
{
    // Enum to define damage types
    public enum DamageType
    {
        Physical,
        Magic
    }

    public List<Character> FirstPlayer;
    public List<Character> SecondPlayer;
    public List<(Character Attacker, Spell spell, float speed, List<Character> target)> Spells = new();

    // Constructor to initialize the game
    public Game()
    {
        FirstPlayer = new List<Character>(); // Fixing initialization of lists
        SecondPlayer = new List<Character>();
        StartGame();

        // Main game loop
        while (FirstPlayer.Sum(objects => objects.ActHealth) > 0 && SecondPlayer.Sum(objects => objects.ActHealth) > 0)
        {
            Spells = new List<(Character, Spell, float, List<Character>)>(); // Clear spells each turn
            Console.WriteLine("First Player Turn :");
            PromptAtkChoice(FirstPlayer, SecondPlayer); // Get attack choices for first player
            Console.WriteLine("Second Player Turn :");
            PromptAtkChoice(SecondPlayer, FirstPlayer); // Get attack choices for second player
            Console.Clear();
            FightScene(Spells); // Apply spells and effects
            Console.WriteLine("Stats of 1st Player : ");
            foreach (var character in FirstPlayer)
            {
                Console.WriteLine(character.ToString()); // Display stats of first player
            }
            Console.WriteLine("Stats of 2nd Player : ");
            foreach (var character in SecondPlayer)
            {
                Console.WriteLine(character.ToString()); // Display stats of second player
            }
            Thread.Sleep(2000);
            GameStatus(FirstPlayer); // Remove fallen characters from first player's team
            GameStatus(SecondPlayer); // Same for the second player
        }

        // Determine the winner
        Console.Clear();
        if (FirstPlayer.Sum(objects => objects.ActHealth) > 0)
        {
            Console.WriteLine("First Player Win!");
            return;
        }
        Console.WriteLine("Second Player Win!");
    }

    // Prompt attacker to choose an attack option
    private void PromptAtkChoice(List<Character> Attacker, List<Character> Defender)
    {
        foreach (var atkPlayer in Attacker)
        {
            while (true)
            {
                Console.WriteLine($"{atkPlayer.Name}'s turn, choose an action:");
                atkPlayer.DisplayAttackList(); // Display available attacks

                // Validate the user's choice
                if (!int.TryParse(Console.ReadLine(), out int spellChoice) || spellChoice <= 0 || spellChoice > atkPlayer.ListSpell.Count)
                {
                    Console.WriteLine("Invalid spell choice. Please try again.");
                    continue;
                }

                var selectedSpell = atkPlayer.AvailableSpell[spellChoice - 1]; // Get the selected spell
                Spell.Target targetType = selectedSpell.SpellTarget; // Get target type for the spell
                List<Character>? targets = null;

                // Determine the target based on the spell's type
                switch (targetType)
                {
                    case Spell.Target.SingleEnnemy:
                        targets = ChooseTarget("enemy", Defender);
                        break;
                    case Spell.Target.SingleAlly:
                        targets = ChooseTarget("ally", Attacker);
                        break;
                    case Spell.Target.Self:
                        targets = new List<Character> { atkPlayer }; // Self-targeting spell
                        break;
                    case Spell.Target.AllyTeam:
                        targets = Attacker; // Target all allies
                        break;
                    case Spell.Target.EnnemyTeam:
                        targets = Defender; // Target all enemies
                        break;
                }

                // If a valid target is selected, add the spell to the list
                if (targets != null)
                {
                    Spells.Add((atkPlayer, selectedSpell, atkPlayer.Speed, targets));
                    break;
                }
            }
        }

        // Sort spells by speed (fastest goes first)
        Spells = Spells.OrderBy(p => p.speed).ToList();
    }

    // Prompt the user to choose a target from the list
    private static List<Character> ChooseTarget(string targetType, List<Character> candidates)
    {
        Console.WriteLine($"Choose a {targetType} target:");
        for (int i = 0; i < candidates.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {candidates[i].Name}");
        }

        // Validate the user's choice
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= candidates.Count)
            {
                return new List<Character> { candidates[choice - 1] };
            }

            Console.WriteLine("Invalid choice. Please select a valid target.");
        }
    }

    // Initialize the game and display the teams
    public void StartGame()
    {
        Console.WriteLine("Welcome To 1vs1 Fighting.");

        FirstPlayer = BuildTeam("First Player");
        Console.WriteLine("Team Player 1:");
        DisplayTeam(FirstPlayer);

        SecondPlayer = BuildTeam("Second Player");
        Console.WriteLine("\nTeam Player 2:");
        DisplayTeam(SecondPlayer);
    }

    // Build the player's team by allowing them to choose champions
    private List<Character> BuildTeam(string playerName)
    {
        var team = new List<Character>();
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"{playerName}, choose your {i + 1} champion: \n1. Warrior\n2. Magician\n3. Paladin\n4. Thief\n5. Priest");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                i--; // Retry if input is invalid
                continue;
            }

            var character = CreateCharacter(choice); // Create character based on choice
            if (character == null)
            {
                Console.WriteLine("Invalid choice. Please select a valid champion.");
                i--; // Retry if character creation fails
                continue;
            }

            team.Add(character);
        }
        return team;
    }

    // Create a character based on the user's choice
    private static Character CreateCharacter(int choice)
    {
#pragma warning disable CS8603 // Possible return of null reference
        return choice switch
        {
            1 => new Warrior("Warrior"),
            2 => new Magician("Magician"),
            3 => new Paladin("Paladin"),
            4 => new Thief("Thief"),
            5 => new Priest("Priest"),
            _ => null
        };
#pragma warning restore CS8603 // Possible return of null reference
    }

    // Display the names of the characters in the team
    private static void DisplayTeam(List<Character> team)
    {
        foreach (var character in team)
        {
            Console.Write($"{character.Name} ");
        }
        Console.WriteLine();
    }

    // Simulate the fight scene by applying the spells
    public static void FightScene(List<(Character Attacker, Spell spell, float speed, List<Character> target)> Spells)
    {
        foreach (var list in Spells)
        {
            if (list.Attacker.ActHealth > 0)
            {
                Console.WriteLine($"{list.Attacker.Name} uses {list.spell.Name}");
                list.spell.SpellMethod(list.target); // Apply the spell effect

                var fallenTargets = new List<Character>();

                // Check if any target has fallen
                foreach (var item in list.target)
                {
                    if (item.ActHealth <= 0)
                    {
                        Console.WriteLine($"{item.Name} has fallen!");
                        fallenTargets.Add(item);
                    }
                }

                // Remove fallen targets from the list
                foreach (var fallen in fallenTargets)
                {
                    list.target.Remove(fallen);
                }
                list.Attacker.ApplyCooldowns(list.spell); // Apply spell cooldown
                Thread.Sleep(500); // Pause after each spell
            }
        }
    }

    // Update the game status by removing fallen characters from the list
    public void GameStatus(List<Character> ListCharacter)
    {
        // Remove characters with zero health
        for (int i = ListCharacter.Count - 1; i >= 0; i--)
        {
            if (ListCharacter[i].ActHealth <= 0)
            {
                ListCharacter.RemoveAt(i);
            }
        }
    }
}
