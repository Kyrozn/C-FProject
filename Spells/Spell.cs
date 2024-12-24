using static Game;

internal class Spell
{
    public string? Name { get; protected set; }  // Name of the spell
    public int DamageQuantity { get; protected set; }  // Amount of damage the spell deals
    public DamageType TypeDamage { get; protected set; }  // Type of damage (Physical or Magic)
    public bool IsSupport { get; protected set; }  // Determines if the spell is supportive (e.g., healing)
    public Action<List<Character>> SpellMethod;  // The method that defines the effect of the spell
    public int Cooldown { get; protected set; }  // Cooldown time (in turns) before the spell can be used again
    public Target SpellTarget { get; protected set; }  // The target type of the spell (e.g., single enemy, team)
    public int ManaNeeded { get; protected set; }  // The amount of mana required to cast the spell
    public int CooldownAct { get; set; }  // The current cooldown left (decreases every turn)

    // Enum to define the possible target types for a spell
    public enum Target
    {
        SingleEnnemy,  // Targets a single enemy
        EnnemyTeam,  // Targets the entire enemy team
        SingleAlly,  // Targets a single ally
        AllyTeam,  // Targets the entire ally team
        Self,  // Targets the caster itself
    }

    // Constructor to initialize a spell
    public Spell(string Name, int DamageQuantity, DamageType typedamage, bool isSupport, Action<List<Character>> spellMethod, int cooldown, Target target, int manaNeeded = 0)
    {
        this.Name = Name;
        this.DamageQuantity = DamageQuantity;
        TypeDamage = typedamage;
        IsSupport = isSupport;
        SpellMethod = spellMethod;
        Cooldown = cooldown;
        SpellTarget = target;
        ManaNeeded = manaNeeded;
        CooldownAct = Cooldown;  // Set the cooldown to the initial value
    }

    // Predefined Mana Potion spell (a healing support spell)
    public static Spell ManaPotion = new("Mana Potion", 0, DamageType.Magic, true, ManaPotionMethod, 1, Target.Self);

    // Method that defines the effect of the Mana Potion spell
    private static void ManaPotionMethod(List<Character> target)
    {
        foreach (var character in target)
        {
            // Restore 50% of the character's max mana, without exceeding the max mana limit
            character.Mana += character.MaxMana / 2;
            if (character.Mana > character.MaxMana)
            {
                character.Mana = character.MaxMana;  // Ensure mana doesn't exceed maximum
            }
        }
    }
}
