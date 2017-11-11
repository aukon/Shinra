using System.Collections.Generic;
using ShinraCo.Spells.Main;

namespace ShinraCo.Spells.Opener
{
    public class SummonerOpener
    {
        private static SummonerSpells MySpells { get; } = new SummonerSpells();

        public List<Spell> Spells = new List<Spell>
        {
            MySpells.RuinIII,
            MySpells.RuinII,
            MySpells.TriDisaster,
            MySpells.RuinII,
            MySpells.Fester,
            MySpells.Role.Swiftcast,
            MySpells.SummonIII,
            MySpells.Painflare,
            MySpells.Aetherpact,
            MySpells.RuinII,
            MySpells.Fester,
            MySpells.DreadwyrmTrance,
            MySpells.RuinIII,
            MySpells.TriDisaster,
            MySpells.Rouse,
            MySpells.RuinIII,
            MySpells.ShadowFlare,
            MySpells.Deathflare,
            MySpells.RuinII,
            MySpells.Aetherflow,
            MySpells.Enkindle,
            MySpells.RuinII,
            MySpells.Fester,
            MySpells.RuinIII,
            MySpells.RuinII,
            MySpells.Fester,
            MySpells.RuinIII,
            MySpells.RuinII,
            MySpells.Fester,
            MySpells.RuinII,
            MySpells.DreadwyrmTrance,
            MySpells.TriDisaster
        };
    }
}