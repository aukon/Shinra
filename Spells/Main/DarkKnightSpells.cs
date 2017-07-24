using ShinraCo.Spells.Role;

namespace ShinraCo.Spells.Main
{
    public class DarkKnightSpells
    {
        public TankSpells Role { get; } = new TankSpells();

        public Spell HardSlash { get; } = new Spell
        {
            Name = "Hard Slash",
            ID = 3617,
            Level = 1,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell SpinningSlash { get; } = new Spell
        {
            Name = "Spinning Slash",
            ID = 3619,
            Level = 4,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell Unleash { get; } = new Spell
        {
            Name = "Unleash",
            ID = 3621,
            Level = 6,
            GCDType = GCDType.On,
            SpellType = SpellType.AoE,
            CastType = CastType.Self
        };

        public Spell SyphonStrike { get; } = new Spell
        {
            Name = "Syphon Strike",
            ID = 3623,
            Level = 10,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell Unmend { get; } = new Spell
        {
            Name = "Unmend",
            ID = 3624,
            Level = 15,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell BloodWeapon { get; } = new Spell
        {
            Name = "Blood Weapon",
            ID = 3625,
            Level = 18,
            GCDType = GCDType.Off,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };

        public Spell PowerSlash { get; } = new Spell
        {
            Name = "Power Slash",
            ID = 3627,
            Level = 26,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell Grit { get; } = new Spell
        {
            Name = "Grit",
            ID = 3629,
            Level = 30,
            GCDType = GCDType.On,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };

        public Spell Darkside { get; } = new Spell
        {
            Name = "Darkside",
            ID = 3628,
            Level = 30,
            GCDType = GCDType.Off,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };
    }
}