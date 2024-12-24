internal class Magician : Character
{
    public int Shieldboost { get; protected set; }
    public bool IsReturn = false;
    public bool IsBarrier = false;

    public Magician(string Name) : base(Name)
    {
        MaxHealth = 60;
        ActHealth = MaxHealth;
        AD = 0;
        AP = 75;
        Armor = new ArmorFabric();
        Dodge = 5;
        Parry = 5;
        TankSpell = 25;
        ListSpell = [new("Frostbolt", (int)AP, Game.DamageType.Magic, false, Frostbolt, 1, Spell.Target.SingleEnnemy, 15), new("Frost Barrier", 0, Game.DamageType.Magic, true, FrostBarrier, 2, Spell.Target.Self, 25), new("Blizzard", (int)AP / 2, Game.DamageType.Magic, false, Blizzard, 2, Spell.Target.EnnemyTeam, 25), new("Returns Spell", 0, Game.DamageType.Magic, false, ReturnsSpell, 3, Spell.Target.Self, 25), Spell.ManaPotion];
        AvailableSpell = ListSpell;
        Speed = 75;
        MaxMana = 100;
        Mana = MaxMana;
    }

    public void Frostbolt(List<Character> target)
    {
        if (Mana >= 15)
        {
            Mana -= 15;
            foreach (var character in target)
            {
                character.DefenseMethod((int)AP, Game.DamageType.Magic, out string? sentence);
                if (sentence == "Attack Parried")
                {
                    character.Speed -= (float)(character.Speed * 0.25);
                }
            }
        }
    }
    public void Blizzard(List<Character> target)
    {
        if (Mana >= 25)
        {
            Mana -= 25;
            foreach (var character in target)
            {
                character.DefenseMethod((int)(AP * 0.5), Game.DamageType.Magic, out string? sentence);
                if (sentence == "Attack Parried")
                {
                    character.Speed -= (float)(character.Speed * 0.15);
                }
            }
        }
    }
    public void ReturnsSpell(List<Character> target)
    {
        if (Mana >= 25)
        {
            Mana -= 25;
            IsReturn = true;
        }
    }
    public void FrostBarrier(List<Character> target)
    {
        if (Mana >= 25)
        {
            Mana -= 25;
            Armor.ADDefense = 60;
            Armor.ADDefense = 50;
        }
    }

}