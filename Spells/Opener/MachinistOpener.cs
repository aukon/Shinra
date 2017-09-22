using System.Collections.Generic;
using ShinraCo.Spells.Main;

namespace ShinraCo.Spells.Opener
{
    public class MachinistOpener
    {
        private static MachinistSpells MySpells { get; } = new MachinistSpells();

        public List<Spell> Spells = new List<Spell>
        {
            MySpells.HotShot,
            MySpells.GaussRound,
            MySpells.SplitShot,
            MySpells.Reload,
            MySpells.SlugShot,
            MySpells.Flamethrower,
            MySpells.SplitShot,
            MySpells.Reassemble,
            MySpells.Wildfire,
            MySpells.CleanShot,
            MySpells.QuickReload,
            MySpells.RapidFire,
            MySpells.SlugShot,
            MySpells.GaussRound,
            MySpells.CleanShot,
            MySpells.Ricochet,
            MySpells.Cooldown,
            MySpells.RookOverdrive,
            MySpells.Cooldown
        };
    }
}