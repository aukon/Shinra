using System.Collections.Generic;
using ShinraCo.Spells.Main;

namespace ShinraCo.Spells.Opener
{
    public class PaladinOpener
    {
        private static PaladinSpells MySpells { get; } = new PaladinSpells();

        public List<Spell> Spells = new List<Spell>
        {
            MySpells.FastBlade,
            MySpells.RiotBlade,
            MySpells.FightOrFlight,
            MySpells.GoringBlade,
            MySpells.CircleOfScorn,
            MySpells.SpiritsWithin,
            MySpells.FastBlade,
            MySpells.RiotBlade,
            MySpells.RoyalAuthority,
            MySpells.FastBlade,
            MySpells.RiotBlade,
            MySpells.RoyalAuthority,
            MySpells.FastBlade,
            MySpells.RiotBlade,
            MySpells.GoringBlade,
            MySpells.Requiescat,
            MySpells.HolySpirit,
            MySpells.CircleOfScorn,
            MySpells.HolySpirit,
            MySpells.HolySpirit,
            MySpells.HolySpirit,
            MySpells.SpiritsWithin,
            MySpells.HolySpirit
        };
    }
}