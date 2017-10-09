using System.Collections.Generic;
using ShinraCo.Spells.Main;

namespace ShinraCo.Spells.Opener
{
    public class BardOpener
    {
        private static BardSpells MySpells { get; } = new BardSpells();

        public List<Spell> Spells = new List<Spell>
        {
            MySpells.Windbite,
            MySpells.Bloodletter,
            MySpells.RagingStrikes,
            MySpells.VenomousBite,
            MySpells.WanderersMinuet,
            MySpells.EmpyrealArrow,
            MySpells.StraightShot,
            MySpells.BattleVoice,
            MySpells.IronJaws,
            MySpells.Sidewinder
        };
    }
}