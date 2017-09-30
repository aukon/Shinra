using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using ShinraCo.Settings;
using ShinraCo.Spells;
using ShinraCo.Spells.Main;
using Resource = ff14bot.Managers.ActionResourceManager.Bard;

namespace ShinraCo.Rotations
{
    public sealed partial class Bard
    {
        private BardSpells MySpells { get; } = new BardSpells();

        private int _heavyCount;

        #region Damage

        private async Task<bool> HeavyShot()
        {
            if (await MySpells.HeavyShot.Cast())
            {
                if (BarrageCooldown < 500)
                {
                    _heavyCount++;
                }
                else
                {
                    _heavyCount = 0;
                }
                return true;
            }
            return false;
        }

        private async Task<bool> StraightShotBuff()
        {
            if (!Core.Player.HasAura(MySpells.StraightShot.Name, true, 4000))
            {
                return await MySpells.StraightShot.Cast();
            }
            return false;
        }

        private async Task<bool> StraightShot()
        {
            if (Core.Player.HasAura("Straighter Shot"))
            {
                return await MySpells.StraightShot.Cast();
            }
            return false;
        }

        private async Task<bool> MiserysEnd()
        {
            return await MySpells.MiserysEnd.Cast();
        }

        private async Task<bool> Bloodletter()
        {
            return await MySpells.Bloodletter.Cast();
        }

        private async Task<bool> PitchPerfect()
        {
            if (Shinra.Settings.BardPitchPerfect)
            {
                if (NumRepertoire >= Shinra.Settings.BardRepertoireCount || MinuetActive && SongTimer < 3000)
                {
                    return await MySpells.PitchPerfect.Cast();
                }
            }
            return false;
        }

        private async Task<bool> RefulgentArrow()
        {
            if (Shinra.Settings.BardBarrage && BarrageCooldown > 0 && BarrageCooldown < 5000 &&
                Core.Player.HasAura(MySpells.StraightShot.Name, true, 8000))
            {
                return false;
            }
            if (Shinra.Settings.BardBarrage && ActionManager.CanCast(MySpells.RefulgentArrow.Name, Core.Player.CurrentTarget))
            {
                if (await MySpells.Barrage.Cast(null, false))
                {
                    await Coroutine.Wait(3000, () => Core.Player.HasAura(MySpells.Barrage.Name));
                }
            }
            return await MySpells.RefulgentArrow.Cast();
        }

        #endregion

        #region DoT

        private async Task<bool> VenomousBite()
        {
            if (!Core.Player.CurrentTarget.HasAura(VenomDebuff, true, 4000))
            {
                return await MySpells.VenomousBite.Cast();
            }
            return false;
        }

        private async Task<bool> Windbite()
        {
            if (!Core.Player.CurrentTarget.HasAura(WindDebuff, true, 4000))
            {
                return await MySpells.Windbite.Cast();
            }
            return false;
        }

        private async Task<bool> IronJaws()
        {
            if (Core.Player.CurrentTarget.HasAura(VenomDebuff, true) && !Core.Player.CurrentTarget.HasAura(VenomDebuff, true, 4000) ||
                Core.Player.CurrentTarget.HasAura(WindDebuff, true) && !Core.Player.CurrentTarget.HasAura(WindDebuff, true, 4000))
            {
                return await MySpells.IronJaws.Cast();
            }
            return false;
        }

        #endregion

        #region AoE

        private async Task<bool> QuickNock()
        {
            if (Core.Player.CurrentTPPercent > 40)
            {
                if (Shinra.Settings.RotationMode == Modes.Multi || Shinra.Settings.RotationMode == Modes.Smart &&
                    Helpers.EnemiesNearTarget(5) > 2)
                {
                    return await MySpells.QuickNock.Cast();
                }
            }
            return false;
        }

        private async Task<bool> RainOfDeath()
        {
            if (Shinra.Settings.RotationMode == Modes.Multi || Shinra.Settings.RotationMode == Modes.Smart &&
                Helpers.EnemiesNearTarget(5) > 1)
            {
                return await MySpells.RainOfDeath.Cast();
            }
            return false;
        }

        #endregion

        #region Cooldown

        private async Task<bool> MagesBallad()
        {
            if (Shinra.Settings.BardSongs && !RecentSong &&
                (NoSong || PaeonActive && DataManager.GetSpellData(3559).Cooldown.TotalSeconds < 30))
            {
                return await MySpells.MagesBallad.Cast();
            }
            return false;
        }

        private async Task<bool> ArmysPaeon()
        {
            if (Shinra.Settings.BardSongs && !RecentSong && NoSong)
            {
                return await MySpells.ArmysPaeon.Cast();
            }
            return false;
        }

        private async Task<bool> WanderersMinuet()
        {
            if (Shinra.Settings.BardSongs && !RecentSong &&
                (NoSong || PaeonActive && DataManager.GetSpellData(114).Cooldown.TotalSeconds < 30))
            {
                return await MySpells.WanderersMinuet.Cast();
            }
            return false;
        }

        private async Task<bool> EmpyrealArrow()
        {
            if (Shinra.Settings.BardBarrage && ActionManager.CanCast(MySpells.EmpyrealArrow.Name, Core.Player.CurrentTarget) &&
                (!ActionManager.HasSpell(MySpells.RefulgentArrow.Name) || _heavyCount > 3))
            {
                if (await MySpells.Barrage.Cast(null, false))
                {
                    await Coroutine.Wait(3000, () => Core.Player.HasAura(MySpells.Barrage.Name));
                }
            }
            return await MySpells.EmpyrealArrow.Cast(null, !Core.Player.HasAura(MySpells.Barrage.Name));
        }

        private async Task<bool> Sidewinder()
        {
            if (Shinra.Settings.BardSidewinder && Core.Player.CurrentTarget.HasAura(VenomDebuff, true) &&
                Core.Player.CurrentTarget.HasAura(WindDebuff, true))
            {
                return await MySpells.Sidewinder.Cast();
            }
            return false;
        }

        #endregion

        #region Buff

        private async Task<bool> RagingStrikes()
        {
            if (Shinra.Settings.BardRagingStrikes)
            {
                return await MySpells.RagingStrikes.Cast();
            }
            return false;
        }

        private async Task<bool> Barrage()
        {
            if (Shinra.Settings.BardBarrage && !ActionManager.HasSpell(MySpells.EmpyrealArrow.Name))
            {
                return await MySpells.Barrage.Cast();
            }
            return false;
        }

        private async Task<bool> BattleVoice()
        {
            if (Shinra.Settings.BardBattleVoice)
            {
                return await MySpells.BattleVoice.Cast();
            }
            return false;
        }

        #endregion

        #region Role

        private async Task<bool> SecondWind()
        {
            if (Shinra.Settings.BardSecondWind && Core.Player.CurrentHealthPercent < Shinra.Settings.BardSecondWindPct)
            {
                return await MySpells.Role.SecondWind.Cast();
            }
            return false;
        }

        private async Task<bool> Peloton()
        {
            if (Shinra.Settings.BardPeloton && !Core.Player.HasAura(MySpells.Role.Peloton.Name) && !Core.Player.HasTarget &&
                (MovementManager.IsMoving || BotManager.Current.EnglishName == "DeepDive"))
            {
                return await MySpells.Role.Peloton.Cast(null, false);
            }
            return false;
        }

        private async Task<bool> Invigorate()
        {
            if (Shinra.Settings.BardInvigorate && Core.Player.CurrentTPPercent < Shinra.Settings.BardInvigoratePct)
            {
                return await MySpells.Role.Invigorate.Cast();
            }
            return false;
        }

        private async Task<bool> Tactician()
        {
            if (Shinra.Settings.BardTactician)
            {
                var target = Core.Player.CurrentTPPercent < Shinra.Settings.BardTacticianPct ? Core.Player
                    : Helpers.GoadManager.FirstOrDefault(gm => gm.CurrentTPPercent < Shinra.Settings.BardTacticianPct);

                if (target != null)
                {
                    return await MySpells.Role.Tactician.Cast();
                }
            }
            return false;
        }

        private async Task<bool> Refresh()
        {
            if (Shinra.Settings.BardRefresh)
            {
                var target = Helpers.HealManager.FirstOrDefault(hm => hm.CurrentManaPercent < Shinra.Settings.BardRefreshPct &&
                                                                      hm.IsHealer());

                if (target != null)
                {
                    return await MySpells.Role.Refresh.Cast();
                }
            }
            return false;
        }

        private async Task<bool> Palisade()
        {
            if (Shinra.Settings.BardPalisade)
            {
                var target = Helpers.HealManager.FirstOrDefault(hm => hm.CurrentHealthPercent < Shinra.Settings.BardPalisadePct &&
                                                                      hm.IsTank());

                if (target != null)
                {
                    return await MySpells.Role.Palisade.Cast(target);
                }
            }
            return false;
        }

        #endregion

        #region Custom

        private static string VenomDebuff => Core.Player.ClassLevel < 64 ? "Venomous Bite" : "Caustic Bite";
        private static string WindDebuff => Core.Player.ClassLevel < 64 ? "Windbite" : "Storm Bite";
        private static bool NoSong => Resource.ActiveSong == Resource.BardSong.None;
        private static bool MinuetActive => Resource.ActiveSong == Resource.BardSong.WanderersMinuet;
        private static bool PaeonActive => Resource.ActiveSong == Resource.BardSong.ArmysPaeon;
        private static double SongTimer => Resource.Timer.TotalMilliseconds;
        private static double BarrageCooldown => DataManager.GetSpellData(107).Cooldown.TotalMilliseconds;
        private static int NumRepertoire => Resource.Repertoire;

        private static bool RecentSong
        {
            get { return Spell.RecentSpell.Keys.Any(rs => rs.Contains("Minuet") || rs.Contains("Ballad") || rs.Contains("Paeon")); }
        }

        #endregion
    }
}