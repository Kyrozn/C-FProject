using static Game;

internal class Spell
{
    public string? Name { get; protected set; }
    public int DamageQuantity { get; protected set; }
    public DamageType TypeDamage { get; protected set; }
    public bool IsSupport { get; protected set; }
    public Action<List<Character>> SpellMethod;
    public int Cooldown { get; protected set; }
    public Target SpellTarget { get; protected set; }
    public int ManaNeeded { get; protected set; }
    public int CooldownAct {get; set; }
    public enum Target
    {
        SingleEnnemy,
        EnnemyTeam,
        SingleAlly,
        AllyTeam,
        Self,
    }

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
        CooldownAct = Cooldown;
    }
    public static Spell ManaPotion = new("Mana Potion", 0, DamageType.Magic, true, ManaPotionMethod, 1, Target.Self);
    private static void ManaPotionMethod(List<Character> target)
    {
        foreach (var character in target)
        {
            character.Mana += character.MaxMana / 2;
            if (character.Mana > character.MaxMana)
            {
                character.Mana = character.MaxMana;
            }
        }
    }
}