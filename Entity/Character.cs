internal abstract class Character(string name)
{
    private readonly Random rnd = new();
    public string Name { get; set; } = name;
    public int MaxHealth { get; protected set; }
    public int ActHealth { get; set; }
    public double AD { get; set; }
    public double AP { get; set; }
    public ArmorType Armor { get; set; }
    public int Dodge { get; protected set; }
    public int Parry { get; protected set; }
    public int TankSpell { get; protected set; }
    public List<Spell> ListSpell { get; protected set; } = new();
    public List<Spell> AvailableSpell { get; protected set; } = new();
    public float Speed { get; set; }

    public int? MaxMana { get; set; } = null;
    public int? Mana { get; set; } = null;

    public void DisplayAttackList()
    {
        int i = 1;
        foreach (var spell in AvailableSpell)
        {
            Console.WriteLine($"{i}. {spell.Name}");
            i++;
        }
    }

    public int DefenseMethod(int dmg, Game.DamageType typeDamage, out string? sentence)
    {
        if (dmg < 0) throw new ArgumentException("Damage must be a positive value.");

        float reduceDamage = dmg;
        sentence = null;

        if (typeDamage == Game.DamageType.Physical)
        {
            reduceDamage *= 1 - ((float)Armor.ADDefense / 100);
            if (Dodge >= rnd.Next(1, 101))
            {
                sentence = $"{Name} dodged the attack.";
                return 0;
            }
            if (Parry >= rnd.Next(1, 101))
            {
                sentence = $"{Name} parried the attack.";
                reduceDamage /= 2;
            }
        }
        else
        {
            reduceDamage *= 1 - ((float)Armor.APDefense / 100);
            if (TankSpell >= rnd.Next(1, 101))
            {
                sentence = $"{Name} tanked the attack.";
                return 0;
            }
        }

        ActHealth -= (int)reduceDamage;
        return (int)reduceDamage;
    }

    public override string ToString()
    {
        string longString = $@"{Name}
 _______________
|  HealthPoint  |    {ActHealth}/{MaxHealth}
| Physic Damage |    {AD}
|  Magic Damage |    {AP}
|   Armor-Rm    |    {Armor.ADDefense} - {Armor.APDefense}";

        if (MaxMana != null)
        {
            longString += $"\n|   Mana-Max    |    {Mana}/{MaxMana}";
        }

        longString += "\n|_______________|";

        return longString;
    }

    public void ApplyCooldowns(Spell spell)
    {
        if (spell.Cooldown != 1)
        {
            spell.CooldownAct--;
            if (spell.CooldownAct == 0)
            {
                spell.CooldownAct = spell.Cooldown;
            }
        }
        AvailableSpell = ListSpell.Where(objects => objects.CooldownAct == objects.Cooldown).ToList();
    }
}
