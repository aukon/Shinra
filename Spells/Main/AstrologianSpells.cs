﻿using ShinraCo.Spells.Role;

namespace ShinraCo.Spells.Main
{
    public class AstrologianSpells
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

        public Spell Benefic { get; } = new Spell
        {
            Name = "Benefic",
            ID = 3594,
            Level = 2,
            GCDType = GCDType.On,
            SpellType = SpellType.Heal,
            CastType = CastType.Target
        };

        public Spell Combust { get; } = new Spell
        {
            Name = "Combust",
            ID = 3599,
            Level = 4,
            GCDType = GCDType.On,
            SpellType = SpellType.DoT,
            CastType = CastType.Target
        };

        public Spell Lightspeed { get; } = new Spell
        {
            Name = "Lightspeed",
            ID = 3606,
            Level = 6,
            GCDType = GCDType.Off,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };

        public Spell Helios { get; } = new Spell
        {
            Name = "Helios",
            ID = 3600,
            Level = 10,
            GCDType = GCDType.On,
            SpellType = SpellType.Heal,
            CastType = CastType.Self
        };

        public Spell Ascend { get; } = new Spell
        {
            Name = "Ascend",
            ID = 3603,
            Level = 12,
            GCDType = GCDType.On,
            SpellType = SpellType.Heal,
            CastType = CastType.Target
        };

        public Spell EssentialDignity { get; } = new Spell
        {
            Name = "Essential Dignity",
            ID = 3614,
            Level = 15,
            GCDType = GCDType.Off,
            SpellType = SpellType.Heal,
            CastType = CastType.Target
        };

        public Spell BeneficII { get; } = new Spell
        {
            Name = "Benefic II",
            ID = 3610,
            Level = 26,
            GCDType = GCDType.On,
            SpellType = SpellType.Heal,
            CastType = CastType.Target
        };

        public Spell Draw { get; } = new Spell
        {
            Name = "Draw",
            ID = 3590,
            Level = 30,
            GCDType = GCDType.On,
            SpellType = SpellType.Card,
            CastType = CastType.Self
        };

        public Spell DiurnalSect { get; } = new Spell
        {
            Name = "Diurnal Sect",
            ID = 3604,
            Level = 30,
            GCDType = GCDType.On,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };

        public Spell UndrawSpread { get; } = new Spell
        {
            Name = "Undraw Spread",
            ID = 4646,
            Level = 40,
            GCDType = GCDType.On,
            SpellType = SpellType.Card,
            CastType = CastType.Self
        };

        public Spell Undraw { get; } = new Spell
        {
            Name = "Undraw",
            ID = 9629,
            Level = 30,
            GCDType = GCDType.On,
            SpellType = SpellType.Card,
            CastType = CastType.Self
        };

        public Spell AspectedBenefic { get; } = new Spell
        {
            Name = "Aspected Benefic",
            ID = 3595,
            Level = 34,
            GCDType = GCDType.On,
            SpellType = SpellType.Heal,
            CastType = CastType.Target
        };

        public Spell RoyalRoad { get; } = new Spell
        {
            Name = "Royal Road",
            ID = 3591,
            Level = 35,
            GCDType = GCDType.On,
            SpellType = SpellType.Card,
            CastType = CastType.Self
        };

        public Spell Spread { get; } = new Spell
        {
            Name = "Spread",
            ID = 3592,
            Level = 40,
            GCDType = GCDType.On,
            SpellType = SpellType.Card,
            CastType = CastType.Self
        };

        public Spell AspectedHelios { get; } = new Spell
        {
            Name = "Aspected Helios",
            ID = 3601,
            Level = 42,
            GCDType = GCDType.On,
            SpellType = SpellType.Heal,
            CastType = CastType.Self
        };

        public Spell Redraw { get; } = new Spell
        {
            Name = "Redraw",
            ID = 3593,
            Level = 45,
            GCDType = GCDType.On,
            SpellType = SpellType.Card,
            CastType = CastType.Self
        };

        public Spell CombustII { get; } = new Spell
        {
            Name = "Combust II",
            ID = 3608,
            Level = 46,
            GCDType = GCDType.On,
            SpellType = SpellType.DoT,
            CastType = CastType.Target
        };

        public Spell NocturnalSect { get; } = new Spell
        {
            Name = "Nocturnal Sect",
            ID = 3605,
            Level = 50,
            GCDType = GCDType.On,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };

        public Spell Synastry { get; } = new Spell
        {
            Name = "Synastry",
            ID = 3612,
            Level = 50,
            GCDType = GCDType.Off,
            SpellType = SpellType.Buff,
            CastType = CastType.Target
        };

        public Spell Gravity { get; } = new Spell
        {
            Name = "Gravity",
            ID = 3615,
            Level = 52,
            GCDType = GCDType.On,
            SpellType = SpellType.AoE,
            CastType = CastType.Target
        };

        public Spell MaleficII { get; } = new Spell
        {
            Name = "Malefic II",
            ID = 3598,
            Level = 54,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell TimeDilation { get; } = new Spell
        {
            Name = "Time Dilation",
            ID = 3611,
            Level = 56,
            GCDType = GCDType.Off,
            SpellType = SpellType.Buff,
            CastType = CastType.Target
        };

        public Spell CollectiveUnconscious { get; } = new Spell
        {
            Name = "Collective Unconscious",
            ID = 3613,
            Level = 58,
            GCDType = GCDType.Off,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };

        public Spell CelestialOpposition { get; } = new Spell
        {
            Name = "Celestial Opposition",
            ID = 3616,
            Level = 60,
            GCDType = GCDType.Off,
            SpellType = SpellType.Buff,
            CastType = CastType.Self
        };

        public Spell EarthlyStar { get; } = new Spell
        {
            Name = "Earthly Star",
            ID = 7439,
            Level = 62,
            GCDType = GCDType.On,
            SpellType = SpellType.Cooldown,
            CastType = CastType.TargetLocation
        };

        public Spell StellarDetonation { get; } = new Spell
        {
            Name = "Stellar Detonation",
            ID = 8324,
            Level = 62,
            GCDType = GCDType.Off,
            SpellType = SpellType.Cooldown,
            CastType = CastType.Self
        };

        public Spell MaleficIII { get; } = new Spell
        {
            Name = "Malefic III",
            ID = 7442,
            Level = 64,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };

        public Spell MinorArcana { get; } = new Spell
        {
            Name = "Minor Arcana",
            ID = 7443,
            Level = 66,
            GCDType = GCDType.On,
            SpellType = SpellType.Card,
            CastType = CastType.Self
        };

        public Spell LordOfCrowns { get; } = new Spell
        {
            Name = "Lord of Crowns",
            ID = 7444,
            Level = 66,
            GCDType = GCDType.On,
            SpellType = SpellType.Card,
            CastType = CastType.Target
        };

        public Spell LadyOfCrowns { get; } = new Spell
        {
            Name = "Lady of Crowns",
            ID = 7445,
            Level = 66,
            GCDType = GCDType.On,
            SpellType = SpellType.Card,
            CastType = CastType.Target
        };

        public Spell SleeveDraw { get; } = new Spell
        {
            Name = "Sleeve Draw",
            ID = 7448,
            Level = 70,
            GCDType = GCDType.On,
            SpellType = SpellType.Card,
            CastType = CastType.Self
        };
    }
}