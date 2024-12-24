internal class Thief : Character
{
    public Thief(string Name) : base(Name)
    {
        MaxHealth = 80;
        ActHealth = MaxHealth;
        AD = 55;
        AP = 0;
        Armor = new ArmorLeather();
        Dodge = 15;
        Parry = 25;
        TankSpell = 25;
        ListSpell = [new("Low blow", (int)AD, Game.DamageType.Physical, false, LowBlow, 1, Spell.Target.SingleEnnemy), new("Escape", (int)AD, Game.DamageType.Physical, true, Escape, 1, Spell.Target.Self)];
        AvailableSpell = ListSpell;
        Speed = 100;
        //In first the damage Spell, in second support Spell in last other.
    }

    public void LowBlow(List<Character> target)
    {
        foreach (var character in target)
        {
            if (character.ActHealth >= character.MaxHealth / 2)
            {
                character.DefenseMethod((int)AD, Game.DamageType.Physical, out string? _);
                continue;
            }
            character.DefenseMethod((int)(AD*1.5), Game.DamageType.Physical, out string? _);
        }
    }
    public void Escape(List<Character> target)
    {
        
    }
    new public int DefenseMethod(int dmg, Game.DamageType TypeDamage, out string? sentence)
    {
        float reduceDamage = dmg;
        sentence = null;
        Random rnd = new Random();
        if (TypeDamage == Game.DamageType.Physical)
        {
            reduceDamage *= 1 - ((float)Armor.ADDefense / 100);
            if (Dodge >= rnd.Next(1, 101))
            {
                Console.WriteLine($"{Name} Dodge the Attack");
                sentence = "Attack Dodged";
                return 15;
            }
            if (Parry >= rnd.Next(1, 101))
            {
                Console.WriteLine("Attack Parried");
                sentence = "Attack Parried";
                reduceDamage /= 2;
            }
        }
        else
        {
            reduceDamage *= 1 - ((float)Armor.APDefense / 100);
            if (TankSpell >= rnd.Next(1, 101))
            {
                Console.WriteLine($"{Name} Tank the Attack");
                sentence = "Attack Tanked";
                return 0;
            }
        }
        ActHealth -= (int)reduceDamage;
        return 0;
    }
}