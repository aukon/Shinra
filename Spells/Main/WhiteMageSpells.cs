using ShinraCo.Spells.Role;

namespace ShinraCo.Spells.Main
{
    public class WhiteMageSpells
    {
        public HealerSpells Role { get; } = new HealerSpells();

        public Spell Malefic { get; } = new Spell
        {
            Name = "Malefic",
            ID = 3596,
            Level = 1,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };
    }
}