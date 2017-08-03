using ShinraCo.Spells.Role;

namespace ShinraCo.Spells.Main
{
    public class RogueSpells
    {
        public MeleeSpells Role { get; } = new MeleeSpells();

        public Spell SpinningEdge { get; } = new Spell
        {
            Name = "Spinning Edge",
            ID = 2240,
            Level = 1,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell ShadeShift { get; } = new Spell
        {
            Name = "Shade Shift",
            ID = 2241,
            Level = 2,
            GCDType = GCDType.Off,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };

        public Spell GustSlash { get; } = new Spell
        {
            Name = "Gust Slash",
            ID = 2242,
            Level = 4,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell Hide { get; } = new Spell
        {
            Name = "Hide",
            ID = 2245,
            Level = 10,
            GCDType = GCDType.Off,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };

        public Spell Assassinate { get; } = new Spell
        {
            Name = "Assassinate",
            ID = 2246,
            Level = 12,
            GCDType = GCDType.Off,
            SpellType = SpellType.Cooldown,
            CastType = CastType.Target
        };

        public Spell ThrowingDagger { get; } = new Spell
        {
            Name = "Throwing Dagger",
            ID = 2247,
            Level = 15,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell Mug { get; } = new Spell
        {
            Name = "Mug",
            ID = 2248,
            Level = 15,
            GCDType = GCDType.Off,
            SpellType = SpellType.Cooldown,
            CastType = CastType.Target
        };

        public Spell TrickAttack { get; } = new Spell
        {
            Name = "Trick Attack",
            ID = 2258,
            Level = 18,
            GCDType = GCDType.Off,
            SpellType = SpellType.Cooldown,
            CastType = CastType.Target
        };

        public Spell AeolianEdge { get; } = new Spell
        {
            Name = "Aeolian Edge",
            ID = 2255,
            Level = 26,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell Jugulate { get; } = new Spell
        {
            Name = "Jugulate",
            ID = 2256,
            Level = 30,
            GCDType = GCDType.Off,
            SpellType = SpellType.Cooldown,
            CastType = CastType.Target
        };

        public Spell ShadowFang { get; } = new Spell
        {
            Name = "Shadow Fang",
            ID = 2257,
            Level = 38,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell DeathBlossom { get; } = new Spell
        {
            Name = "Death Blossom",
            ID = 2254,
            Level = 42,
            GCDType = GCDType.On,
            SpellType = SpellType.AoE,
            CastType = CastType.Self
        };
    }

    public class NinjaSpells : RogueSpells
    {
        public Spell Ten { get; } = new Spell
        {
            Name = "Ten",
            ID = 2259,
            Level = 30,
            GCDType = GCDType.On,
            SpellType = SpellType.Mudra,
            CastType = CastType.Self
        };

        public Spell Ninjutsu { get; } = new Spell
        {
            Name = "Ninjutsu",
            ID = 2260,
            Level = 30,
            GCDType = GCDType.On,
            SpellType = SpellType.Ninjutsu,
            CastType = CastType.Target
        };

        public Spell FumaShuriken { get; } = new Spell
        {
            Name = "Fuma Shuriken",
            ID = 2265,
            Level = 30,
            GCDType = GCDType.On,
            SpellType = SpellType.Ninjutsu,
            CastType = CastType.Target
        };
    }
}