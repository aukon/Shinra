using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using ShinraCo.Spells;
using ShinraCo.Spells.Main;

namespace ShinraCo.Rotations
{
    public sealed partial class Bard
    {
        private BardSpells MySpells { get; } = new BardSpells();

        #region  Damage

        private async Task<bool> HeavyShot()
        {
            return await MySpells.HeavyShot.Cast();
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
            if (NumRepertoire == 3 || MinuetActive && SongTimer < 3000)
            {
                return await MySpells.PitchPerfect.Cast();
            }
            return false;
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
            if (Core.Player.CurrentTPPercent > 40 && Helpers.EnemiesNearTarget(5) > 2)
            {
                return await MySpells.QuickNock.Cast();
            }
            return false;
        }

        private async Task<bool> RainOfDeath()
        {
            if (Helpers.EnemiesNearTarget(5) > 1)
            {
                return await MySpells.RainOfDeath.Cast();
            }
            return false;
        }

        #endregion

        #region Cooldown

        private async Task<bool> MagesBallad()
        {
            if (!RecentSong && (NoSong || PaeonActive && DataManager.GetSpellData(3559).Cooldown.TotalSeconds < 30))
            {
                return await MySpells.MagesBallad.Cast();
            }
            return false;
        }

        private async Task<bool> ArmysPaeon()
        {
            if (!RecentSong && NoSong)
            {
                return await MySpells.ArmysPaeon.Cast();
            }
            return false;
        }

        private async Task<bool> WanderersMinuet()
        {
            if (!RecentSong && (NoSong || PaeonActive && DataManager.GetSpellData(114).Cooldown.TotalSeconds < 30))
            {
                return await MySpells.WanderersMinuet.Cast();
            }
            return false;
        }

        private async Task<bool> EmpyrealArrow()
        {
            if (!ActionManager.HasSpell("Refulgent Arrow") && ActionManager.CanCast(MySpells.EmpyrealArrow.Name, Core.Player.CurrentTarget))
            {
                if (await MySpells.Barrage.Cast())
                {
                    await Coroutine.Wait(3000, () => ActionManager.CanCast(MySpells.EmpyrealArrow.Name, Core.Player.CurrentTarget));
                }
            }
            return await MySpells.EmpyrealArrow.Cast();
        }

        private async Task<bool> Sidewinder()
        {
            if (Core.Player.CurrentTarget.HasAura(VenomDebuff, true) && Core.Player.CurrentTarget.HasAura(WindDebuff, true))
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
            if (!ActionManager.HasSpell(MySpells.EmpyrealArrow.Name))
            {
                return await MySpells.Barrage.Cast();
            }
            return false;
        }

        private async Task<bool> BattleVoice()
        {
            return await MySpells.BattleVoice.Cast();
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
                MovementManager.IsMoving)
            {
                return await MySpells.Role.Peloton.Cast();
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
            if (Shinra.Settings.BardTactician && Core.Player.CurrentTPPercent < Shinra.Settings.BardTacticianPct)
            {
                return await MySpells.Role.Tactician.Cast();
            }
            return false;
        }

        #endregion

        #region Custom

        private static string VenomDebuff => Core.Player.ClassLevel < 64 ? "Venomous Bite" : "Caustic Bite";
        private static string WindDebuff => Core.Player.ClassLevel < 64 ? "Windbite" : "Storm Bite";
        private static bool NoSong => ActionResourceManager.Bard.ActiveSong == ActionResourceManager.Bard.BardSong.None;
        private static bool MinuetActive => ActionResourceManager.Bard.ActiveSong == ActionResourceManager.Bard.BardSong.WanderersMinuet;
        private static bool PaeonActive => ActionResourceManager.Bard.ActiveSong == ActionResourceManager.Bard.BardSong.ArmysPaeon;
        private static double SongTimer => ActionResourceManager.Bard.Timer.TotalMilliseconds;
        private static int NumRepertoire => ActionResourceManager.Bard.Repertoire;

        private static bool RecentSong
        {
            get { return Spell.RecentSpell.Keys.Any(rs => rs.Contains("Minuet") || rs.Contains("Ballad") || rs.Contains("Paeon")); }
        }

        #endregion
    }
}