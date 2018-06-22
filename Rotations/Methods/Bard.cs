using System;
using System.Collections.Generic;
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

        private static readonly Dictionary<string, Tuple<DateTime, int>> CritSnapshots = new Dictionary<string, Tuple<DateTime, int>>();
        private static readonly Dictionary<string, DateTime> DotSnapshots = new Dictionary<string, DateTime>();

        #region Damage

        private async Task<bool> HeavyShot()
        {
            return await MySpells.HeavyShot.Cast();
        }

        private async Task<bool> StraightShotBuff()
        {
            if (!Core.Player.HasAura(MySpells.StraightShot.Name, true, 6000))
            {
                return await MySpells.StraightShot.Cast();
            }
            return false;
        }

        private async Task<bool> StraightShot()
        {
            if (Core.Player.HasAura("Straighter Shot") && !ActionManager.HasSpell(MySpells.RefulgentArrow.Name))
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
                CritSnapshots.RemoveAll(t => DateTime.UtcNow > t.Item1);
                var critBuffs = CritSnapshots.ContainsKey(TargetId) ? CritSnapshots[TargetId].Item2 : 0;

                if (NumRepertoire >= Shinra.Settings.BardRepertoireCount || MinuetActive && SongTimer < 3000 ||
                    critBuffs >= 2 && NumRepertoire >= 2)
                {
                    return await MySpells.PitchPerfect.Cast();
                }
            }
            return false;
        }

        private async Task<bool> RefulgentArrow()
        {
            if (Core.Player.HasAura(122) && (!Shinra.Settings.BardBarrage || MySpells.Barrage.Cooldown() > 7000 ||
                                             !Core.Player.HasAura(MySpells.StraightShot.Name, true, 6000)))
            {
                return await MySpells.RefulgentArrow.Cast();
            }
            return false;
        }

        private async Task<bool> BarrageActive()
        {
            if (Core.Player.HasAura(MySpells.Barrage.Name))
            {
                if (await MySpells.RefulgentArrow.Cast())
                {
                    return true;
                }
                if (ActionManager.LastSpell.Name == MySpells.HeavyShot.Name)
                {
                    await Coroutine.Wait(400, () => Core.Player.HasAura(122));
                }
                if (!Core.Player.HasAura(122))
                {
                    return await MySpells.EmpyrealArrow.Cast(null, false);
                }
            }
            return false;
        }

        #endregion

        #region DoT

        private async Task<bool> VenomousBite()
        {
            if (Shinra.Settings.BardUseDots && !Core.Player.CurrentTarget.HasAura(VenomDebuff, true, 4000))
            {
                if (await MySpells.VenomousBite.Cast())
                {
                    CritSnapshots[TargetId] = Tuple.Create(DateTime.UtcNow + TimeSpan.FromSeconds(30), NumCritBuffs);
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> Windbite()
        {
            if (Shinra.Settings.BardUseDots && !Core.Player.CurrentTarget.HasAura(WindDebuff, true, 4000))
            {
                if (await MySpells.Windbite.Cast())
                {
                    CritSnapshots[TargetId] = Tuple.Create(DateTime.UtcNow + TimeSpan.FromSeconds(30), NumCritBuffs);
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> IronJaws()
        {
            if (Core.Player.CurrentTarget.HasAura(VenomDebuff, true) && !Core.Player.CurrentTarget.HasAura(VenomDebuff, true, 5000) ||
                Core.Player.CurrentTarget.HasAura(WindDebuff, true) && !Core.Player.CurrentTarget.HasAura(WindDebuff, true, 5000))
            {
                if (await MySpells.IronJaws.Cast())
                {
                    CritSnapshots[TargetId] = Tuple.Create(DateTime.UtcNow + TimeSpan.FromSeconds(30), NumCritBuffs);
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> DotSnapshot()
        {
            if (!Core.Player.CurrentTarget.HasAura(VenomDebuff, true) || !Core.Player.CurrentTarget.HasAura(WindDebuff, true))
                return false;

            DotSnapshots.RemoveAll(t => DateTime.UtcNow > t);
            if (DotSnapshots.ContainsKey(TargetId))
                return false;

            if (Core.Player.HasAura("Embolden") && EmboldenStacks == 5)
            {
                if (await MySpells.IronJaws.Cast())
                {
                    Helpers.Debug("Snapshotting now!");
                    CritSnapshots[TargetId] = Tuple.Create(DateTime.UtcNow + TimeSpan.FromSeconds(30), NumCritBuffs);
                    DotSnapshots[TargetId] = DateTime.UtcNow + TimeSpan.FromSeconds(25);
                    return true;
                }
            }

            foreach (var s in BuffList)
            {
                if (Core.Player.HasAura(s) && !Core.Player.HasAura(s, false, 4000))
                {
                    if (await MySpells.IronJaws.Cast())
                    {
                        Helpers.Debug("Snapshotting now!");
                        DotSnapshots[TargetId] = DateTime.UtcNow + TimeSpan.FromSeconds(25);
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region AoE

        private async Task<bool> QuickNock()
        {
            if (Shinra.Settings.BardUseDotsAoe && (!Core.Player.CurrentTarget.HasAura(VenomDebuff, true, 4000) ||
                !Core.Player.CurrentTarget.HasAura(WindDebuff, true, 4000)))
            {
                return false;
            }

            if (Core.Player.CurrentTPPercent > 40)
            {
                var count = Shinra.Settings.CustomAoE ? Shinra.Settings.CustomAoECount : 3;

                if (Shinra.Settings.RotationMode == Modes.Multi || Shinra.Settings.RotationMode == Modes.Smart &&
                    Helpers.EnemiesNearTarget(5) >= count)
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
            if (Shinra.Settings.BardEmpyrealArrow)
            {
                return await MySpells.EmpyrealArrow.Cast();
            }
            return false;
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
                if (MinuetActive || !ActionManager.HasSpell(MySpells.WanderersMinuet.ID))
                {
                    return await MySpells.RagingStrikes.Cast();
                }
            }
            return false;
        }

        private async Task<bool> FoeRequiem()
        {
            if (Shinra.Settings.BardFoeRequiem && !Core.Player.HasAura(MySpells.FoeRequiem.Name) && !MovementManager.IsMoving)
            {
                return await MySpells.FoeRequiem.Cast();
            }
            return false;
        }

        private async Task<bool> Barrage()
        {
            if (Shinra.Settings.BardBarrage)
            {
                if (MySpells.EmpyrealArrow.Cooldown() == 0 ||
                    Core.Player.HasAura(122) && ActionManager.HasSpell(MySpells.RefulgentArrow.Name) ||
                    !ActionManager.HasSpell(MySpells.EmpyrealArrow.Name))
                {
                    if (await MySpells.Barrage.Cast())
                    {
                        return await Coroutine.Wait(1000, () => Core.Player.HasAura(MySpells.Barrage.Name));
                    }
                }
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

        #region PVP

        private async Task<bool> StraightShotPVP()
        {
            return await MySpells.PVP.StraightShot.Cast();
        }

        private async Task<bool> StormbitePVP()
        {
            if (!Core.Player.CurrentTarget.HasAura("Caustic Bite", true, 4000) || !Core.Player.CurrentTarget.HasAura("Stormbite", true, 4000))
            {
                return await MySpells.PVP.Stormbite.Cast();
            }
            return false;
        }

        private async Task<bool> SidewinderPVP()
        {
            if (Core.Player.CurrentTarget.HasAura("Caustic Bite", true, 1000) && Core.Player.CurrentTarget.HasAura("Stormbite", true, 1000))
            {
                return await MySpells.PVP.Sidewinder.Cast();
            }
            return false;
        }

        private async Task<bool> EmpyrealArrowPVP()
        {
            return await MySpells.PVP.EmpyrealArrow.Cast();
        }

        private async Task<bool> BloodletterPVP()
        {
            if (!MinuetActive || NumRepertoire == 3 || MinuetActive && SongTimer < 3000)
            {
                return await MySpells.PVP.Bloodletter.Cast();
            }
            return false;
        }

        private async Task<bool> WanderersMinuetPVP()
        {
            if (!MinuetActive)
            {
                return await MySpells.PVP.WanderersMinuet.Cast();
            }
            return false;
        }

        private async Task<bool> ArmysPaeonPVP()
        {
            if (NoSong)
            {
                return await MySpells.PVP.ArmysPaeon.Cast();
            }
            return false;
        }

        private async Task<bool> BarragePVP()
        {
            if (Shinra.LastSpell.Name != MySpells.PVP.StraightShot.Name &&
                ActionManager.GetPvPComboCurrentActionId(MySpells.PVP.StraightShot.Combo) == MySpells.PVP.StraightShot.ID)
            {
                return await MySpells.PVP.Barrage.Cast();
            }
            return false;
        }

        private async Task<bool> TroubadourPVP()
        {
            if (MinuetActive)
            {
                return await MySpells.PVP.Troubadour.Cast();
            }
            return false;
        }

        #endregion

        #region Custom

        private static readonly string[] BuffList = { "Raging Strikes", "Brotherhood", "The Balance", "Battle Litany", "The Spear", "Chain Stratagem" };
        private static string TargetId => $"{Core.Player.CurrentTarget.ObjectId}-{Core.Player.CurrentTarget.Name}";
        private static string VenomDebuff => Core.Player.ClassLevel < 64 ? "Venomous Bite" : "Caustic Bite";
        private static string WindDebuff => Core.Player.ClassLevel < 64 ? "Windbite" : "Stormbite";
        private static double SongTimer => Resource.Timer.TotalMilliseconds;
        private static int NumRepertoire => Resource.Repertoire;
        private static int NumCritBuffs => Convert.ToInt32(Core.Player.HasAura("Battle Litany")) + Convert.ToInt32(Core.Player.HasAura("The Spear")) +
                                           Convert.ToInt32(Core.Player.CurrentTarget.HasAura("Chain Stratagem"));

        private static bool NoSong => Resource.ActiveSong == Resource.BardSong.None;
        private static bool MinuetActive => Resource.ActiveSong == Resource.BardSong.WanderersMinuet;
        private static bool PaeonActive => Resource.ActiveSong == Resource.BardSong.ArmysPaeon;

        private static bool RecentSong
        {
            get { return Spell.RecentSpell.Keys.Any(rs => rs.Contains("Minuet") || rs.Contains("Ballad") || rs.Contains("Paeon")); }
        }

        private static int EmboldenStacks
        {
            get
            {
                var aura = Core.Player.GetAuraById(1239);
                var value = aura?.Value ?? 0;
                return (int)value;
            }
        }

        #endregion
    }
}