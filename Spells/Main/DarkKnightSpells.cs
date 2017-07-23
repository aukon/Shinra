using ShinraCo.Spells.Role;

namespace ShinraCo.Spells.Main
{
    public class DarkKnightSpells
    {
        public TankSpells Role { get; } = new TankSpells();

        public Spell FastBlade { get; } = new Spell
        {
            Name = "Fast Blade",
            ID = 9,
            Level = 1,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };
    }
}