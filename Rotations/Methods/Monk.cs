using System.Threading.Tasks;
using ff14bot;
using ff14bot.Managers;
using ShinraCo.Settings;
using ShinraCo.Spells.Main;
using Resource = ff14bot.Managers.ActionResourceManager.Monk;

namespace ShinraCo.Rotations
{
    public sealed partial class Monk
    {
        private MonkSpells MySpells { get; } = new MonkSpells();

        #region Damage

        private async Task<bool> Bootshine()
        {
            return await MySpells.Bootshine.Cast();
        }

        private async Task<bool> TrueStrike()
        {
            if (RaptorForm || BalanceActive)
            {
                return await MySpells.TrueStrike.Cast();
            }
            return false;
        }

        private async Task<bool> SnapPunch()
        {
            if (CoeurlForm || BalanceActive && Resource.GreasedLightning < 3)
            {
                return await MySpells.SnapPunch.Cast();
            }
            return false;
        }

        private async Task<bool> TwinSnakes()
        {
            if ((RaptorForm || BalanceActive) && !Core.Player.HasAura(MySpells.TwinSnakes.Name, true, 3000))
            {
                return await MySpells.TwinSnakes.Cast();
            }
            return false;
        }

        private async Task<bool> DragonKick()
        {
            if ((OpoOpoForm || BalanceActive) && !Core.Player.CurrentTarget.HasAura(821, false, 5000))
            {
                return await MySpells.DragonKick.Cast();
            }
            return false;
        }

        #endregion

        #region DoT

        private async Task<bool> Demolish()
        {
            if (Shinra.Settings.MonkDemolish && Core.Player.CurrentTarget.CurrentHealth > Shinra.Settings.MonkDemolishHP)
            {
                if ((CoeurlForm || BalanceActive) && !Core.Player.CurrentTarget.HasAura(MySpells.Demolish.Name, true, 5000))
                {
                    return await MySpells.Demolish.Cast();
                }
            }
            return false;
        }

        #endregion

        #region AoE

        private async Task<bool> Rockbreaker()
        {
            if (CoeurlForm || BalanceActive && Core.Player.HasAura(MySpells.TwinSnakes.Name))
            {
                return await MySpells.Rockbreaker.Cast();
            }
            return false;
        }

        #endregion

        #region Cooldown

        private async Task<bool> ShoulderTackle()
        {
            if (Shinra.Settings.MonkShoulderTackle && Core.Player.TargetDistance(10))
            {
                return await MySpells.ShoulderTackle.Cast();
            }
            return false;
        }

        private async Task<bool> SteelPeak()
        {
            if (Shinra.Settings.MonkSteelPeak)
            {
                return await MySpells.SteelPeak.Cast();
            }
            return false;
        }

        private async Task<bool> HowlingFist()
        {
            if (Shinra.Settings.MonkHowlingFist)
            {
                return await MySpells.HowlingFist.Cast();
            }
            return false;
        }

        private async Task<bool> ForbiddenChakra()
        {
            if (Shinra.Settings.MonkForbiddenChakra && Resource.FithChakra == 5)
            {
                return await MySpells.ForbiddenChakra.Cast();
            }
            return false;
        }

        private async Task<bool> ElixirField()
        {
            if (Shinra.Settings.MonkElixirField)
            {
                return await MySpells.ElixirField.Cast();
            }
            return false;
        }

        private async Task<bool> FireTackle()
        {
            if (Shinra.Settings.MonkFireTackle && Core.Player.TargetDistance(10))
            {
                return await MySpells.FireTackle.Cast();
            }
            return false;
        }

        #endregion

        #region Buff

        private async Task<bool> InternalRelease()
        {
            if (Shinra.Settings.MonkInternalRelease)
            {
                return await MySpells.InternalRelease.Cast();
            }
            return false;
        }

        private async Task<bool> PerfectBalance()
        {
            if (Shinra.Settings.MonkPerfectBalance)
            {
                return await MySpells.PerfectBalance.Cast();
            }
            return false;
        }

        private async Task<bool> RiddleOfFire()
        {
            if (Shinra.Settings.MonkRiddleOfFire)
            {
                return await MySpells.RiddleOfFire.Cast();
            }
            return false;
        }

        private async Task<bool> Brotherhood()
        {
            if (Shinra.Settings.MonkBrotherhood)
            {
                return await MySpells.Brotherhood.Cast();
            }
            return false;
        }

        #endregion

        #region Fists

        private async Task<bool> FistsOfEarth()
        {
            if (Shinra.Settings.MonkFist == MonkFists.Earth ||
                Shinra.Settings.MonkFist == MonkFists.Wind && !ActionManager.HasSpell(MySpells.FistsOfWind.Name) ||
                Shinra.Settings.MonkFist == MonkFists.Fire && !ActionManager.HasSpell(MySpells.FistsOfFire.Name))
            {
                if (!Core.Player.HasAura(MySpells.FistsOfEarth.Name))
                {
                    return await MySpells.FistsOfEarth.Cast();
                }
            }
            return false;
        }

        private async Task<bool> FistsOfWind()
        {
            if (Shinra.Settings.MonkFist == MonkFists.Wind && !Core.Player.HasAura(MySpells.FistsOfWind.Name))
            {
                return await MySpells.FistsOfWind.Cast();
            }
            return false;
        }

        private async Task<bool> FistsOfFire()
        {
            if (Shinra.Settings.MonkFist == MonkFists.Fire && !Core.Player.HasAura(MySpells.FistsOfFire.Name))
            {
                return await MySpells.FistsOfFire.Cast();
            }
            return false;
        }

        #endregion

        #region Role

        private async Task<bool> SecondWind()
        {
            if (Shinra.Settings.MonkSecondWind && Core.Player.CurrentHealthPercent < Shinra.Settings.MonkSecondWindPct)
            {
                return await MySpells.Role.SecondWind.Cast();
            }
            return false;
        }

        private async Task<bool> Invigorate()
        {
            if (Shinra.Settings.MonkInvigorate && Core.Player.CurrentTPPercent < Shinra.Settings.MonkInvigoratePct)
            {
                return await MySpells.Role.Invigorate.Cast();
            }
            return false;
        }

        private async Task<bool> Bloodbath()
        {
            if (Shinra.Settings.MonkBloodbath && Core.Player.CurrentHealthPercent < Shinra.Settings.MonkBloodbathPct)
            {
                return await MySpells.Role.Bloodbath.Cast();
            }
            return false;
        }

        private async Task<bool> TrueNorth()
        {
            if (Shinra.Settings.MonkTrueNorth && Core.Player.TargetDistance(5, false))
            {
                return await MySpells.Role.TrueNorth.Cast();
            }
            return false;
        }

        #endregion

        #region Custom

        private static bool OpoOpoForm => Core.Player.HasAura(107);
        private static bool RaptorForm => Core.Player.HasAura(108);
        private static bool CoeurlForm => Core.Player.HasAura(109);
        private static bool BalanceActive => Core.Player.HasAura(110);

        #endregion
    }
}