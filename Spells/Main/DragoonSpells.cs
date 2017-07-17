using ShinraCo.Spells.Role;

namespace ShinraCo.Spells.Main
{
    public class LancerSpells
    {
        public MeleeSpells Role { get; } = new MeleeSpells();

        public Spell TrueThrust { get; } = new Spell
        {
            Name = "True Thrust",
            ID = 75,
            Level = 1,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell VorpalThrust { get; } = new Spell
        {
            Name = "Vorpal Thrust",
            ID = 78,
            Level = 4,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell ImpulseDrive { get; } = new Spell
        {
            Name = "Impulse Drive",
            ID = 81,
            Level = 6,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell HeavyThrust { get; } = new Spell
        {
            Name = "Heavy Thrust",
            ID = 79,
            Level = 10,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell PiercingTalon { get; } = new Spell
        {
            Name = "Piercing Talon",
            ID = 90,
            Level = 15,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell LifeSurge { get; } = new Spell
        {
            Name = "Life Surge",
            ID = 83,
            Level = 18,
            GCDType = GCDType.Off,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };

        public Spell FullThrust { get; } = new Spell
        {
            Name = "Full Thrust",
            ID = 84,
            Level = 26,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell BloodForBlood { get; } = new Spell
        {
            Name = "Blood for Blood",
            ID = 85,
            Level = 30,
            GCDType = GCDType.Off,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };

        // Disembowel
        // Chaos Thrust
    }

    public class DragoonSpells : LancerSpells
    {
        
    }
}