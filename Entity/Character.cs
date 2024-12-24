internal abstract class Character(string name)
{
    private readonly Random rnd = new();  // Random number generator for certain calculations
    public string Name { get; set; } = name; // Character's name
    public int MaxHealth { get; protected set; }  // Maximum health value
    public int ActHealth { get; set; }  // Current health value
    public double AD { get; set; }  // Attack Damage (AD) value
    public double AP { get; set; }  // Ability Power (AP) value
    public ArmorType Armor { get; set; }  // Armor type, affecting physical and magic damage mitigation
    public int Dodge { get; protected set; }  // Chance to dodge an attack (percentage)
    public int Parry { get; protected set; }  // Chance to parry an attack (percentage)
    public int TankSpell { get; protected set; }  // Chance to tank (block) a spell (percentage)
    public List<Spell> ListSpell { get; protected set; } = new();  // List of all spells for the character
    public List<Spell> AvailableSpell { get; protected set; } = new();  // List of spells that are currently available to cast
    public float Speed { get; set; }  // Speed of the character (for turn order)

    public int? MaxMana { get; set; } = null;  // Maximum mana (nullable, some characters may not have mana)
    public int? Mana { get; set; } = null;  // Current mana (nullable, some characters may not use mana)

    // Method to display a list of available spells for the character
    public void DisplayAttackList()
    {
        int i = 1;
        foreach (var spell in AvailableSpell)
        {
            Console.WriteLine($"{i}. {spell.Name}");
            i++;
        }
    }

    // Method to calculate the defense against incoming damage, considering armor, dodge, parry, and tanking spells
    public int DefenseMethod(int dmg, Game.DamageType typeDamage, out string? sentence)
    {
        if (dmg < 0) throw new ArgumentException("Damage must be a positive value.");

        float reduceDamage = dmg;
        sentence = null;

        // Physical damage defense
        if (typeDamage == Game.DamageType.Physical)
        {
            reduceDamage *= 1 - ((float)Armor.ADDefense / 100);  // Apply physical armor defense
            if (Dodge >= rnd.Next(1, 101))  // Check if the character dodges the attack
            {
                sentence = $"{Name} dodged the attack.";
                return 0;  // No damage if dodged
            }
            if (Parry >= rnd.Next(1, 101))  // Check if the character parries the attack
            {
                sentence = $"{Name} parried the attack.";
                reduceDamage /= 2;  // Reduce damage if parried
            }
        }
        // Magic damage defense
        else
        {
            reduceDamage *= 1 - ((float)Armor.APDefense / 100);  // Apply magic armor defense
            if (TankSpell >= rnd.Next(1, 101))  // Check if the character tanks the spell
            {
                sentence = $"{Name} tanked the attack.";
                return 0;  // No damage if tanked
            }
        }

        // Apply the calculated damage to the character's health
        ActHealth -= (int)reduceDamage;
        return (int)reduceDamage;  // Return the reduced damage
    }

    // Override ToString to display the character's stats in a readable format
    public override string ToString()
    {
        string longString = $@"{Name}
 _______________
|  HealthPoint  |    {ActHealth}/{MaxHealth}
| Physic Damage |    {AD}
|  Magic Damage |    {AP}
|   Armor-Rm    |    {Armor.ADDefense} - {Armor.APDefense}";

        // Include mana stats if applicable
        if (MaxMana != null)
        {
            longString += $"\n|   Mana-Max    |    {Mana}/{MaxMana}";
        }

        longString += "\n|_______________|";

        return longString;  // Return the formatted string with stats
    }

    // Method to handle spell cooldowns, reducing the cooldown and updating available spells
    public void ApplyCooldowns(Spell spell)
    {
        if (spell.Cooldown != 1)
        {
            spell.CooldownAct--;  // Decrease the cooldown of the spell
            if (spell.CooldownAct == 0)
            {
                spell.CooldownAct = spell.Cooldown;  // Reset cooldown when it reaches 0
            }
        }
        // Update the list of available spells (only those with no cooldown left)
        AvailableSpell = ListSpell.Where(objects => objects.CooldownAct == objects.Cooldown).ToList();
    }
}
