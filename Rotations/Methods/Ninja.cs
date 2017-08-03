using System.Threading.Tasks;
using ff14bot;
using ff14bot.Managers;
using ShinraCo.Spells.Main;

namespace ShinraCo.Rotations
{
    public sealed partial class Ninja
    {
        private NinjaSpells MySpells { get; } = new NinjaSpells();

        #region Damage

        private async Task<bool> SpinningEdge()
        {
            return await MySpells.SpinningEdge.Cast();
        }

        private async Task<bool> GustSlash()
        {
            if (ActionManager.LastSpell.Name == MySpells.SpinningEdge.Name)
            {
                return await MySpells.GustSlash.Cast();
            }
            return false;
        }

        private async Task<bool> AeolianEdge()
        {
            if (ActionManager.LastSpell.Name == MySpells.GustSlash.Name)
            {
                return await MySpells.AeolianEdge.Cast();
            }
            return false;
        }

        #endregion

        #region Cooldown

        private async Task<bool> Assassinate()
        {
            return await MySpells.Assassinate.Cast();
        }

        private async Task<bool> Mug()
        {
            return await MySpells.Mug.Cast();
        }

        #endregion

        #region Buff

        private async Task<bool> ShadeShift()
        {
            if (Core.Player.CurrentHealthPercent < 50)
            {
                return await MySpells.ShadeShift.Cast();
            }
            return false;
        }

        #endregion

        #region Role

        private async Task<bool> SecondWind()
        {
            if (Shinra.Settings.NinjaSecondWind && Core.Player.CurrentHealthPercent < Shinra.Settings.NinjaSecondWindPct)
            {
                return await MySpells.Role.SecondWind.Cast();
            }
            return false;
        }

        private async Task<bool> Invigorate()
        {
            if (Shinra.Settings.NinjaInvigorate && Core.Player.CurrentTPPercent < Shinra.Settings.NinjaInvigoratePct)
            {
                return await MySpells.Role.Invigorate.Cast();
            }
            return false;
        }

        private async Task<bool> Bloodbath()
        {
            if (Shinra.Settings.NinjaBloodbath && Core.Player.CurrentHealthPercent < Shinra.Settings.NinjaBloodbathPct)
            {
                return await MySpells.Role.Bloodbath.Cast();
            }
            return false;
        }

        private async Task<bool> TrueNorth()
        {
            if (Shinra.Settings.NinjaTrueNorth)
            {
                return await MySpells.Role.TrueNorth.Cast();
            }
            return false;
        }

        #endregion
    }
}