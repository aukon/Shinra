using System;
using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using ShinraCo.Settings;
using ShinraCo.Spells.Main;
using Resource = ff14bot.Managers.ActionResourceManager.Samurai;

namespace ShinraCo.Rotations
{
    public sealed partial class Samurai
    {
        private SamuraiSpells MySpells { get; } = new SamuraiSpells();

        #region Damage

        private async Task<bool> Hakaze()
        {
            return await MySpells.Hakaze.Cast();
        }

        private async Task<bool> Jinpu()
        {
            if (ActionManager.LastSpell.Name == MySpells.Hakaze.Name && !GetsuActive)
            {
                return await MySpells.Jinpu.Cast();
            }
            return false;
        }

        private async Task<bool> JinpuBuff()
        {
            if (ActionManager.LastSpell.Name == MySpells.Hakaze.Name && !Core.Player.HasAura(MySpells.Jinpu.Name, true, 8000))
            {
                return await MySpells.Jinpu.Cast();
            }
            return false;
        }

        private async Task<bool> Gekko()
        {
            if (ActionManager.LastSpell.Name == MySpells.Jinpu.Name)
            {
                return await MySpells.Gekko.Cast();
            }
            return false;
        }

        private async Task<bool> Shifu()
        {
            if (ActionManager.LastSpell.Name == MySpells.Hakaze.Name && !KaActive)
            {
                return await MySpells.Shifu.Cast();
            }
            return false;
        }

        private async Task<bool> ShifuBuff()
        {
            if (ActionManager.LastSpell.Name == MySpells.Hakaze.Name && !Core.Player.HasAura(MySpells.Shifu.Name, true, 8000))
            {
                return await MySpells.Shifu.Cast();
            }
            return false;
        }

        private async Task<bool> Kasha()
        {
            if (ActionManager.LastSpell.Name == MySpells.Shifu.Name)
            {
                return await MySpells.Kasha.Cast();
            }
            return false;
        }

        private async Task<bool> Yukikaze()
        {
            if (ActionManager.LastSpell.Name == MySpells.Hakaze.Name)
            {
                return await MySpells.Yukikaze.Cast();
            }
            return false;
        }

        private async Task<bool> YukikazeDebuff()
        {
            if (ActionManager.LastSpell.Name == MySpells.Hakaze.Name && !Core.Player.CurrentTarget.HasAura(819, false, 8000))
            {
                return await MySpells.Yukikaze.Cast();
            }
            return false;
        }

        private async Task<bool> Meikyo()
        {
            if (Core.Player.HasAura(1233))
            {
                if (!GetsuActive)
                {
                    return await MySpells.Gekko.Cast();
                }
                if (!KaActive)
                {
                    return await MySpells.Kasha.Cast();
                }
                if (!SetsuActive)
                {
                    return await MySpells.Yukikaze.Cast();
                }
            }
            return false;
        }

        private async Task<bool> MidareSetsugekka()
        {
            if (NumSen == 3 && !MovementManager.IsMoving && Core.Player.CurrentTarget.CurrentHealth > Core.Player.MaxHealth / 2)
            {
                if (ActionManager.CanCast(MySpells.MidareSetsugekka.Name, Core.Player.CurrentTarget))
                {
                    if (await MySpells.HissatsuKaiten.Cast(null, false))
                    {
                        await Coroutine.Wait(3000, () => Core.Player.HasAura(1229));
                    }
                }
                return await MySpells.MidareSetsugekka.Cast();
            }
            return false;
        }

        private async Task<bool> HissatsuShinten()
        {
            if (Shinra.LastSpell.Name != MySpells.HissatsuKaiten.Name && Resource.Kenki >= 45 && !PoolKenki)
            {
                return await MySpells.HissatsuShinten.Cast();
            }
            return false;
        }

        private async Task<bool> HissatsuSeigan()
        {
            if (Shinra.LastSpell.Name != MySpells.HissatsuKaiten.Name && Resource.Kenki >= 45 && !PoolKenki)
            {
                return await MySpells.HissatsuSeigan.Cast();
            }
            return false;
        }

        #endregion

        #region DoT

        private async Task<bool> Higanbana()
        {
            if (Shinra.Settings.SamuraiHiganbana && NumSen == 1 && !MovementManager.IsMoving &&
                !Core.Player.CurrentTarget.HasAura(MySpells.Higanbana.Name, true, 5000))
            {
                if (Core.Player.CurrentTarget.IsBoss() || Core.Player.CurrentTarget.CurrentHealth > Shinra.Settings.SamuraiHiganbanaHP)
                {
                    if (ActionManager.CanCast(MySpells.Higanbana.Name, Core.Player.CurrentTarget))
                    {
                        if (await MySpells.HissatsuKaiten.Cast(null, false))
                        {
                            await Coroutine.Wait(3000, () => Core.Player.HasAura(1229));
                        }
                    }
                    return await MySpells.Higanbana.Cast();
                }
            }
            return false;
        }

        #endregion

        #region AoE

        private async Task<bool> Fuga()
        {
            if (Core.Player.HasAura(MySpells.Shifu.Name) && Core.Player.HasAura(MySpells.Jinpu.Name))
            {
                return await MySpells.Fuga.Cast();
            }
            return false;
        }

        private async Task<bool> Mangetsu()
        {
            if (ActionManager.LastSpell.Name == MySpells.Fuga.Name && !GetsuActive)
            {
                return await MySpells.Mangetsu.Cast();
            }
            return false;
        }

        private async Task<bool> Oka()
        {
            if (ActionManager.LastSpell.Name == MySpells.Fuga.Name && !KaActive)
            {
                return await MySpells.Oka.Cast();
            }
            return false;
        }

        private async Task<bool> TenkaGoken()
        {
            if (NumSen == 2 && !MovementManager.IsMoving && Helpers.EnemiesNearTarget(5) > 2)
            {
                if (ActionManager.CanCast(MySpells.TenkaGoken.Name, Core.Player.CurrentTarget))
                {
                    if (await MySpells.HissatsuKaiten.Cast(null, false))
                    {
                        await Coroutine.Wait(3000, () => Core.Player.HasAura(1229));
                    }
                }
                return await MySpells.TenkaGoken.Cast();
            }
            return false;
        }

        private async Task<bool> HissatsuKyuten()
        {
            if (Shinra.Settings.RotationMode != Modes.Single)
            {
                if (Shinra.LastSpell.Name != MySpells.HissatsuKaiten.Name && Resource.Kenki >= 45 && !PoolKenki)
                {
                    return await MySpells.HissatsuKyuten.Cast();
                }
            }
            return false;
        }

        #endregion

        #region Cooldown

        private async Task<bool> Ageha()
        {
            return await MySpells.Ageha.Cast();
        }

        private async Task<bool> HissatsuGuren()
        {
            if (Shinra.Settings.SamuraiGuren)
            {
                return await MySpells.HissatsuGuren.Cast();
            }
            return false;
        }

        #endregion

        #region Buff

        private async Task<bool> MeikyoShisui()
        {
            if (Shinra.Settings.SamuraiMeikyo && Core.Player.TargetDistance(5, false))
            {
                if (ActionManager.LastSpell.Name == MySpells.Gekko.Name || ActionManager.LastSpell.Name == MySpells.Kasha.Name ||
                    ActionManager.LastSpell.Name == MySpells.Yukikaze.Name)
                {
                    return await MySpells.MeikyoShisui.Cast();
                }
            }
            return false;
        }

        private async Task<bool> Meditate()
        {
            if (Core.Player.HasAura(MySpells.Meditate.Name))
            {
                return true;
            }
            return false;
        }

        private async Task<bool> Hagakure()
        {
            if (Shinra.Settings.SamuraiHagakure)
            {
                if (ActionManager.LastSpell.Name == MySpells.Jinpu.Name && GetsuActive ||
                    ActionManager.LastSpell.Name == MySpells.Shifu.Name && KaActive)
                {
                    return await MySpells.Hagakure.Cast();
                }
                if (NumSen == 3 && (Helpers.EnemiesNearPlayer(5) > 2 || MovementManager.IsMoving))
                {
                    return await MySpells.Hagakure.Cast();
                }
            }
            return false;
        }

        #endregion

        #region Heal

        private async Task<bool> MercifulEyes()
        {
            if (Core.Player.CurrentHealthPercent < 60)
            {
                return await MySpells.MercifulEyes.Cast();
            }
            return false;
        }

        #endregion

        #region Role

        private async Task<bool> SecondWind()
        {
            if (Shinra.Settings.SamuraiSecondWind && Core.Player.CurrentHealthPercent < Shinra.Settings.SamuraiSecondWindPct)
            {
                return await MySpells.Role.SecondWind.Cast();
            }
            return false;
        }

        private async Task<bool> Invigorate()
        {
            if (Shinra.Settings.SamuraiInvigorate && Core.Player.CurrentTPPercent < Shinra.Settings.SamuraiInvigoratePct)
            {
                return await MySpells.Role.Invigorate.Cast();
            }
            return false;
        }

        private async Task<bool> Bloodbath()
        {
            if (Shinra.Settings.SamuraiBloodbath && Core.Player.CurrentHealthPercent < Shinra.Settings.SamuraiBloodbathPct)
            {
                return await MySpells.Role.Bloodbath.Cast();
            }
            return false;
        }

        private async Task<bool> TrueNorth()
        {
            if (Shinra.Settings.SamuraiTrueNorth && Core.Player.TargetDistance(5, false))
            {
                return await MySpells.Role.TrueNorth.Cast();
            }
            return false;
        }

        #endregion

        #region Custom

        private static bool GetsuActive => Resource.Sen.HasFlag(Resource.Iaijutsu.Getsu);
        private static bool KaActive => Resource.Sen.HasFlag(Resource.Iaijutsu.Ka);
        private static bool SetsuActive => Resource.Sen.HasFlag(Resource.Iaijutsu.Setsu);
        private static bool PoolKenki => Shinra.Settings.SamuraiGuren && ActionManager.HasSpell(7496) &&
                                         DataManager.GetSpellData(7496).Cooldown.TotalMilliseconds < 6000;

        private static int NumSen
        {
            get { return Enum.GetValues(typeof(Resource.Iaijutsu)).Cast<Enum>().Count(value => Resource.Sen.HasFlag(value)); }
        }

        #endregion
    }
}