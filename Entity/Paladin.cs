using System.Security.AccessControl;

internal class Paladin : Character
{
    public Paladin(string Name) : base(Name)
    {
        MaxHealth = 95;
        ActHealth = MaxHealth;
        AD = 40;
        AP = 40;
        Armor = new ArmorMeshes();
        Dodge = 5;
        Parry = 10;
        TankSpell = 20;
        ListSpell = [new("Crusader Strike", (int)AD, Game.DamageType.Physical, false, CrusaderStrike, 1, Spell.Target.SingleEnnemy, 5), new("Judgement", (int)AP, Game.DamageType.Magic, false, Judgement, 1, Spell.Target.SingleEnnemy, 10), new("Bright flash", (int)(AP * 1.25), Game.DamageType.Magic, true, Brightflash, 1, Spell.Target.SingleAlly, 25), Spell.ManaPotion];
        AvailableSpell = ListSpell;
        Speed = 75;
        MaxMana = 60;
        Mana = MaxMana;
    }

    public void CrusaderStrike(List<Character> target)
    {
        if (Mana <= 5)
        {
            Mana -= 5;
            foreach (var character in target)
            {
                int HPBeforeDmg = character.ActHealth;
                character.DefenseMethod((int)AD, Game.DamageType.Physical, out string? _);
                SpecialPassive((HPBeforeDmg - character.ActHealth) / 2);

            }
        }
    }
    public void Judgement(List<Character> target)
    {
        if (Mana <= 10)
        {
            Mana -= 10;
            foreach (var character in target)
            {
                int HPBeforeDmg = character.ActHealth;
                character.DefenseMethod((int)AP, Game.DamageType.Magic, out string? _);
                SpecialPassive((HPBeforeDmg - character.ActHealth) / 2);
            }
        }
    }
    public void Brightflash(List<Character> target)
    {
        if (Mana <= 25)
        {
            Mana -= 25;
            foreach (var character in target)
            {
                int HPBeforeDmg = character.ActHealth;
                character.ActHealth += (int)(AP * 1.25);
                if (ActHealth >= MaxHealth)
                {
                    ActHealth = MaxHealth;
                }
                SpecialPassive((HPBeforeDmg-character.ActHealth)/2);
            }
        }
    }
    private void SpecialPassive(int toHeal) {
        ActHealth += toHeal;
    }
}