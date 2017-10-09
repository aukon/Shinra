using System.Collections.Generic;
using ShinraCo.Spells.Main;

namespace ShinraCo.Spells.Opener
{
    public class BlackMageOpener
    {
        private static BlackMageSpells MySpells { get; } = new BlackMageSpells();

        public List<Spell> Spells = new List<Spell>
        {
            MySpells.BlizzardIII,
            MySpells.Enochian,
            MySpells.ThunderIII,
            MySpells.BlizzardIV,
            MySpells.Thundercloud,
            MySpells.LeyLines,
            MySpells.FireIII,
            MySpells.Triplecast,
            MySpells.FireIV,
            MySpells.FireIV,
            MySpells.Role.Swiftcast,
            MySpells.FireIV,
            MySpells.FireIV,
            MySpells.Convert,
            MySpells.Fire
        };
    }
}