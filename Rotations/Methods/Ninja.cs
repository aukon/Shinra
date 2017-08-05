using System.Threading.Tasks;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using ShinraCo.Spells.Main;
using Resource = ff14bot.Managers.ActionResourceManager.Ninja;

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

        #region DoT

        private async Task<bool> ShadowFang()
        {
            if (ActionManager.LastSpell.Name == MySpells.GustSlash.Name &&
                !Core.Player.CurrentTarget.HasAura(MySpells.ShadowFang.Name, true, 5000) && Core.Player.CurrentTarget.IsBoss())
            {
                return await MySpells.ShadowFang.Cast();
            }
            return false;
        }

        #endregion

        #region AoE

        private async Task<bool> DeathBlossom()
        {
            if (Core.Player.CurrentTPPercent > 40)
            {
                return await MySpells.DeathBlossom.Cast();
            }
            return false;
        }

        #endregion

        #region Cooldown

        private async Task<bool> Assassinate()
        {
            if (Shinra.Settings.NinjaAssassinate)
            {
                return await MySpells.Assassinate.Cast();
            }
            return false;
        }

        private async Task<bool> Mug()
        {
            if (Shinra.Settings.NinjaMug)
            {
                return await MySpells.Mug.Cast();
            }
            return false;
        }

        private async Task<bool> Jugulate()
        {
            return await MySpells.Jugulate.Cast();
        }

        #endregion

        #region Buff

        private async Task<bool> ShadeShift()
        {
            if (Shinra.Settings.NinjaShadeShift && Core.Player.CurrentHealthPercent < Shinra.Settings.NinjaShadeShiftPct)
            {
                return await MySpells.ShadeShift.Cast();
            }
            return false;
        }

        #endregion

        #region Ninjutsu

        private static bool UseNinjutsu(bool targetSelf = false, int range = 15)
        {
            return Core.Player.HasAura(496) || DataManager.GetSpellData(2240).Cooldown.TotalMilliseconds < 1500 &&
                   (targetSelf || Core.Player.CurrentTarget.CanAttack && Core.Player.TargetDistance(range, false) &&
                    Core.Player.CurrentTarget.InLineOfSight());
        }

        private async Task<bool> Raiton()
        {
            if (UseNinjutsu() && ActionManager.CanCast(MySpells.Chi.ID, null))
            {
                if (!CanNinjutsu)
                {
                    if (await MySpells.Ten.Cast())
                    {
                        await Coroutine.Wait(2000, () => CanNinjutsu);
                    }
                }
                if (LastTen)
                {
                    if (await MySpells.Chi.Cast())
                    {
                        await Coroutine.Wait(2000, () => CanNinjutsu);
                    }
                }
                if (LastChi)
                {
                    if (await MySpells.Raiton.Cast())
                    {
                        await Coroutine.Wait(2000, () => !Core.Player.HasAura(496));
                        return true;
                    }
                }
            }
            return false;
        }

        private async Task<bool> FumaShuriken()
        {
            if (UseNinjutsu() && ActionManager.CanCast(MySpells.Ten.Name, null))
            {
                if (!CanNinjutsu)
                {
                    if (await MySpells.Ten.Cast())
                    {
                        await Coroutine.Wait(2000, () => CanNinjutsu);
                    }
                }
                if (CanNinjutsu && LastTen)
                {
                    if (await MySpells.FumaShuriken.Cast())
                    {
                        await Coroutine.Wait(2000, () => !Core.Player.HasAura("Mudra"));
                        return true;
                    }
                }
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

        #region Custom

        private bool CanNinjutsu => ActionManager.CanCast(MySpells.Ninjutsu.ID, null);
        private bool LastTen => Shinra.LastSpell.ID == MySpells.Ten.ID;
        private bool LastChi => Shinra.LastSpell.ID == MySpells.Chi.ID;
        private bool LastJin => Shinra.LastSpell.ID == MySpells.Jin.ID;

        #endregion
    }
}