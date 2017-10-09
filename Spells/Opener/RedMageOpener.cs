using System.Collections.Generic;
using ShinraCo.Spells.Main;

namespace ShinraCo.Spells.Opener
{
    public class RedMageOpener
    {
        private static RedMageSpells MySpells { get; } = new RedMageSpells();

        public List<Spell> Spells = new List<Spell>
        {
            MySpells.Veraero,
            MySpells.Verthunder,
            MySpells.Verstone,
            MySpells.Veraero,
            MySpells.Fleche,
            MySpells.ContreSixte,
            MySpells.JoltII,
            MySpells.Verthunder,
            MySpells.Role.Swiftcast,
            MySpells.Verthunder,
            MySpells.CorpsACorps,
            MySpells.Embolden,
            MySpells.Impact,
            MySpells.Veraero,
            MySpells.Manafication,
            MySpells.CorpsACorps,
            MySpells.EnchantedRiposte,
            MySpells.EnchantedZwerchhau,
            MySpells.EnchantedRedoublement,
            MySpells.Verflare
        };
    }
}