using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ff14bot;
using ff14bot.Enums;
using ShinraCo.Spells;
using ShinraCo.Spells.Opener;

namespace ShinraCo
{
    public static partial class Helpers
    {
        public static async Task<bool> ExecuteOpener()
        {
            if (Shinra.OpenerFinished || Core.Player.ClassLevel < 70)
            {
                return false;
            }

            #region GetOpener

            List<Spell> current = null;
            switch (Core.Player.CurrentJob)
            {
                case ClassJobType.Bard:
                    current = BardOpener.List;
                    break;
            }
            if (current == null)
            {
                return false;
            }

            #endregion

            var spell = current.ElementAt(Shinra.OpenerStep);
            Debug($"Executing opener step {Shinra.OpenerStep} >>> {spell.Name}");
            if (await spell.Cast(null, false) || spell.Cooldown(true) > 2500 && spell.Cooldown() > 500 && !Core.Player.IsCasting)
            {
                Shinra.OpenerStep++;
            }

            if (Shinra.OpenerStep >= current.Count)
            {
                Debug("Opener finished.");
                Shinra.OpenerFinished = true;
            }
            return true;
        }
    }
}