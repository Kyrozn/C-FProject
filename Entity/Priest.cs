internal class Priest : Character
{
    public Priest(string Name) : base(Name)
    {
        MaxHealth = 70;
        ActHealth = MaxHealth;
        AD = 0;
        AP = 65;
        Armor = new ArmorFabric();
        Dodge = 10;
        Parry = 0;
        TankSpell = 20;
        ListSpell = [new("Smite", (int)AP, Game.DamageType.Magic, false, Smite, 1, Spell.Target.SingleEnnemy, 15), new("Healing Zone", 0, Game.DamageType.Magic, true, HealingZone, 2, Spell.Target.AllyTeam, 30), new("Mana Burn", (int)AP, Game.DamageType.Magic, true, ManaBurn, 3, Spell.Target.SingleEnnemy, 20), Spell.ManaPotion];
        AvailableSpell = ListSpell;
        Speed = 70;
        MaxMana = 70;
        Mana = MaxMana;
    }

    public void Smite(List<Character> target)
    {
        if (Mana < 15)
        {
            Mana -= 15;
            foreach (var character in target)
            {
                if (character.GetType() == typeof(Paladin) || character.GetType() == typeof(Priest))
                {
                    character.DefenseMethod((int)(AD * 0.75), Game.DamageType.Magic, out string? _);
                    continue;
                }
                character.DefenseMethod((int)(AD * 1.5), Game.DamageType.Magic, out string? _);
            }
        }
    }
    public void HealingZone(List<Character> target)
    {
        if (Mana < 30)
        {
            Mana -= 30;
            foreach (var character in target)
            {
                character.ActHealth += (int)(AP * 0.75);
            }
        }
    }
    public void ManaBurn(List<Character> target)
    {
        if (Mana < 20)
        {
            Mana -= 20;
            foreach (var character in target)
            {
                character.Mana /= 2;
            }
        }
    }
}