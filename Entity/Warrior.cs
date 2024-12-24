internal class Warrior : Character
{
    public Warrior(string Name) : base(Name)
    {
        MaxHealth = 100;
        ActHealth = MaxHealth;
        AD = 50;
        AP = 0;
        Armor = new ArmorPlates();
        Dodge = 5;
        Parry = 25;
        TankSpell = 10; 
        ListSpell = [new("Heroic Strike", (int)AD, Game.DamageType.Physical, false, HeroicStrike, 1, Spell.Target.SingleEnnemy), new("Battle Howl", 0, Game.DamageType.Physical, true, BattleHowl, 2, Spell.Target.AllyTeam), new("Tornado", (int)AD * (33 / 100), Game.DamageType.Physical, false, Tornado, 2, Spell.Target.EnnemyTeam)];
        AvailableSpell = ListSpell;
        Speed = 50;
    }
    public void HeroicStrike(List<Character> target)
    {
        foreach (var character in target)
        {
            character.DefenseMethod((int)AD, Game.DamageType.Physical, out _);
        }
    }
    public void Tornado(List<Character> target)
    {
        foreach (var character in target)
        {
            character.DefenseMethod((int)(0.33 *AD), Game.DamageType.Physical, out _);
        }
    }

    public void BattleHowl(List<Character> target)
    {
        foreach (var character in target)
        {
            character.AD += 25;
        }
    }
    new public int DefenseMethod(int dmg, Game.DamageType TypeDamage, out string? sentence)
    {
        int reduceDamage = dmg;
        int toReturn = 0;
        sentence = null;
        Random rnd = new Random();
        if (TypeDamage == Game.DamageType.Physical)
        {
            reduceDamage *= 1 - (Armor.ADDefense / 100);
            if (Dodge >= rnd.Next(1, 101))
            {
                Console.WriteLine($"{Name} Dodge the Attack");
                sentence = "Attack Dodged";
                return 0;
            }
            if (Parry >= rnd.Next(1, 101))
            {
                Console.WriteLine($"{Name} Parry the Attack");
                toReturn = (int)(reduceDamage * 1.5);
                sentence = "Attack Parried";
                reduceDamage /= 2;
            }
            if (25 >= rnd.Next(1, 101) && toReturn == 0)
            {
                toReturn = reduceDamage / 2;
            }
        }
        else
        {
            reduceDamage *= 1 - (Armor.APDefense / 100);
            if (TankSpell >= rnd.Next(1, 101))
            {
                Console.WriteLine($"{Name} Tank the Attack");
                sentence = "Attack Tanked";
                return 0;
            }
        }
        ActHealth -= reduceDamage;
        return toReturn;
    }

}