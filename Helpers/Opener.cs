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
        public static int OpenerStep;
        public static bool OpenerFinished;

        public static async Task<bool> ExecuteOpener()
        {
            if (OpenerFinished || Core.Player.ClassLevel < 70)
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
                case ClassJobType.BlackMage:
                    current = BlackMageOpener.List;
                    break;
                case ClassJobType.Machinist:
                    current = MachinistOpener.List;
                    break;
                case ClassJobType.Paladin:
                    current = PaladinOpener.List;
                    break;
                case ClassJobType.RedMage:
                    current = RedMageOpener.List;
                    break;
                case ClassJobType.Summoner:
                    current = SummonerOpener.List;
                    break;
            }
            if (current == null)
            {
                return false;
            }

            #endregion

            var spell = current.ElementAt(OpenerStep);
            Debug($"Executing opener step {OpenerStep} >>> {spell.Name}");

            if (await spell.Cast(null, false) || spell.Cooldown(true) > 2500 && spell.Cooldown() > 500 && !Core.Player.IsCasting)
            {
                OpenerStep++;
            }

            if (OpenerStep >= current.Count)
            {
                Debug("Opener finished.");
                OpenerFinished = true;
            }
            return true;
        }

        public static void ResetOpener()
        {
            if (!Core.Player.InCombat && !Spell.RecentSpell.ContainsKey("Opener"))
            {
                OpenerStep = 0;
                OpenerFinished = false;
            }
        }
    }
}