using ShinraCo.Spells.Role;

namespace ShinraCo.Spells.Main
{
    public class MachinistSpells
    {
        public RangedSpells Role { get; } = new RangedSpells();

        public Spell SplitShot { get; } = new Spell
        {
            Name = "Split Shot",
            ID = 2866,
            Level = 1,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell SlugShot { get; } = new Spell
        {
            Name = "Slug Shot",
            ID = 2868,
            Level = 2,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell Reload { get; } = new Spell
        {
            Name = "Reload",
            ID = 2867,
            Level = 4,
            GCDType = GCDType.Off,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };

        public Spell Heartbreak { get; } = new Spell
        {
            Name = "Heartbreak",
            ID = 2875,
            Level = 6,
            GCDType = GCDType.Off,
            SpellType = SpellType.Cooldown,
            CastType = CastType.Target
        };

        public Spell Reassemble { get; } = new Spell
        {
            Name = "Reassemble",
            ID = 2876,
            Level = 10,
            GCDType = GCDType.Off,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };

        public Spell Blank { get; } = new Spell
        {
            Name = "Blank",
            ID = 2888,
            Level = 15,
            GCDType = GCDType.Off,
            SpellType = SpellType.Cooldown,
            CastType = CastType.Target
        };

        public Spell SpreadShot { get; } = new Spell
        {
            Name = "Spread Shot",
            ID = 2870,
            Level = 18,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell QuickReload { get; } = new Spell
        {
            Name = "Quick Reload",
            ID = 2879,
            Level = 26,
            GCDType = GCDType.Off,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };

        public Spell HotShot { get; } = new Spell
        {
            Name = "Hot Shot",
            ID = 2872,
            Level = 30,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell RapidFire { get; } = new Spell
        {
            Name = "Rapid Fire",
            ID = 2881,
            Level = 30,
            GCDType = GCDType.Off,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };

        public Spell CleanShot { get; } = new Spell
        {
            Name = "Clean Shot",
            ID = 2873,
            Level = 35,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell Wildfire { get; } = new Spell
        {
            Name = "Wildfire",
            ID = 2878,
            Level = 38,
            GCDType = GCDType.Off,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };

        public Spell RookAutoturret { get; } = new Spell
        {
            Name = "Rook Autoturret",
            ID = 2864,
            Level = 40,
            GCDType = GCDType.Off,
            SpellType = SpellType.Cooldown,
            CastType = CastType.TargetLocation
        };

        public Spell TurretRetrieval { get; } = new Spell
        {
            Name = "Turret Retrieval",
            ID = 3487,
            Level = 40,
            GCDType = GCDType.Off,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };

        public Spell BishopAutoturret { get; } = new Spell
        {
            Name = "Bishop Autoturret",
            ID = 2865,
            Level = 50,
            GCDType = GCDType.Off,
            SpellType = SpellType.Cooldown,
            CastType = CastType.TargetLocation
        };
    }
}