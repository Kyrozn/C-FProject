internal class Game
{
    public enum DamageType
    {
        Physical,
        Magic
    }
    public List<Character> FirstPlayer;
    public List<Character> SecondPlayer;
    public List<(Character Attacker, Spell spell, float speed, List<Character> target)> Spells = new();
    public Game()
    {
        FirstPlayer = [];
        SecondPlayer = [];
        StartGame();

        while (FirstPlayer.Sum(objects => objects.ActHealth) > 0 && SecondPlayer.Sum(objects => objects.ActHealth) > 0)
        {
            Spells = [];
            Console.WriteLine("First Player Turn :");
            PromptAtkChoice(FirstPlayer, SecondPlayer);
            Console.WriteLine("Second Player Turn :");
            PromptAtkChoice(SecondPlayer, FirstPlayer);
            Console.Clear();
            FightScene(Spells);
            Console.WriteLine("Stats of 1st Player : ");
            foreach (var character in FirstPlayer)
            {
                Console.WriteLine(character.ToString());
            }
            Console.WriteLine("Stats of 2nd Player : ");
            foreach (var character in SecondPlayer)
            {
                Console.WriteLine(character.ToString());
            }
            Thread.Sleep(2000);
            GameStatus(FirstPlayer);
            GameStatus(SecondPlayer);
        }
        Console.Clear();
        if (FirstPlayer.Sum(objects => objects.ActHealth) > 0) {
            Console.WriteLine("First Player Win!");
            return;
        }
        Console.WriteLine("Second Player Win!");
    }

    private void PromptAtkChoice(List<Character> Attacker, List<Character> Defender)
    {
        foreach (var atkPlayer in Attacker)
        {
            while (true)
            {
                Console.WriteLine($"{atkPlayer.Name}'s turn, choose an action:");
                atkPlayer.DisplayAttackList();

                if (!int.TryParse(Console.ReadLine(), out int spellChoice) || spellChoice <= 0 || spellChoice > atkPlayer.ListSpell.Count)
                {
                    Console.WriteLine("Invalid spell choice. Please try again.");
                    continue;
                }

                var selectedSpell = atkPlayer.AvailableSpell[spellChoice - 1];
                Spell.Target targetType = selectedSpell.SpellTarget;
                List<Character>? targets = null;

                switch (targetType)
                {
                    case Spell.Target.SingleEnnemy:
                        targets = ChooseTarget("enemy", Defender);
                        break;
                    case Spell.Target.SingleAlly:
                        targets = ChooseTarget("ally", Attacker);
                        break;
                    case Spell.Target.Self:
                        targets = [atkPlayer];
                        break;
                    case Spell.Target.AllyTeam:
                        targets = Attacker;
                        break;
                    case Spell.Target.EnnemyTeam:
                        targets = Defender;
                        break;
                }

                if (targets != null)
                {
                    Spells.Add((atkPlayer, selectedSpell, atkPlayer.Speed, targets));
                    break;
                }
            }
        }

        Spells = Spells.OrderBy(p => p.speed).ToList();
    }

    private static List<Character>  ChooseTarget(string targetType, List<Character> candidates)
    {
        Console.WriteLine($"Choose a {targetType} target:");
        for (int i = 0; i < candidates.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {candidates[i].Name}");
        }

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= candidates.Count)
            {
                return new List<Character> { candidates[choice - 1] };
            }

            Console.WriteLine("Invalid choice. Please select a valid target.");
        }
    }


    public void StartGame() // Function for initializing the teams and showing them
    {
        Console.WriteLine("Welcome To 1vs1 Fighting.");

        FirstPlayer = BuildTeam("First Player");
        Console.WriteLine("Team Player 1:");
        DisplayTeam(FirstPlayer);

        SecondPlayer = BuildTeam("Second Player");
        Console.WriteLine("\nTeam Player 2:");
        DisplayTeam(SecondPlayer);
    }

    private List<Character> BuildTeam(string playerName)
    {
        var team = new List<Character>();
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"{playerName}, choose your {i + 1} champion: \n1. Warrior\n2. Magician\n3. Paladin\n4. Thief\n5. Priest");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                i--;
                continue;
            }

            var character = CreateCharacter(choice);
            if (character == null)
            {
                Console.WriteLine("Invalid choice. Please select a valid champion.");
                i--;
                continue;
            }

            team.Add(character);
        }
        return team;
    }


    private static Character CreateCharacter(int choice)
    {
#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
        return choice switch
        {
            1 => new Warrior("Warrior"),
            2 => new Magician("Magician"),
            3 => new Paladin("Paladin"),
            4 => new Thief("Thief"),
            5 => new Priest("Priest"),
            _ => null
        };
#pragma warning restore CS8603 // Existence possible d'un retour de référence null.
    }

    private static void DisplayTeam(List<Character> team)
    {
        foreach (var character in team)
        {
            Console.Write($"{character.Name} ");
        }
        Console.WriteLine();
    }
    public static void FightScene(List<(Character Attacker, Spell spell, float speed, List<Character> target)> Spells)
    {
        foreach (var list in Spells)
        {
            if (list.Attacker.ActHealth > 0)
            {
                Console.WriteLine($"{list.Attacker.Name} uses {list.spell.Name}");
                list.spell.SpellMethod(list.target);

                var fallenTargets = new List<Character>();

                foreach (var item in list.target)
                {
                    if (item.ActHealth <= 0)
                    {
                        Console.WriteLine($"{item.Name} has fallen!");
                        fallenTargets.Add(item);
                    }
                }

                foreach (var fallen in fallenTargets)
                {
                    list.target.Remove(fallen);
                }
                list.Attacker.ApplyCooldowns(list.spell);
                Thread.Sleep(500);   
            }
        }
    }

    public void GameStatus(List<Character> ListCharacter)
    {
        for (int i = ListCharacter.Count - 1; i >= 0; i--)
        {
            if (ListCharacter[i].ActHealth <= 0)
            {
                ListCharacter.RemoveAt(i);
            }
            
        }
    }
}