using System.Linq;
using System.Threading.Tasks;
using ff14bot;
using ff14bot.Managers;
using ShinraCo.Spells;
using ShinraCo.Spells.Main;
using Resource = ff14bot.Managers.ActionResourceManager.Dragoon;

namespace ShinraCo.Rotations
{
    public sealed partial class Dragoon
    {
        private DragoonSpells MySpells { get; } = new DragoonSpells();

        #region Damage

        private async Task<bool> TrueThrust()
        {
            return await MySpells.TrueThrust.Cast();
        }

        private async Task<bool> VorpalThrust()
        {
            if (ActionManager.LastSpell.Name == MySpells.TrueThrust.Name)
            {
                return await MySpells.VorpalThrust.Cast();
            }
            return false;
        }

        private async Task<bool> FullThrust()
        {
            if (ActionManager.LastSpell.Name == MySpells.VorpalThrust.Name)
            {
                return await MySpells.FullThrust.Cast();
            }
            return false;
        }

        private async Task<bool> ImpulseDrive()
        {
            if (ActionManager.HasSpell(MySpells.ChaosThrust.Name) &&
                !Core.Player.CurrentTarget.HasAura(MySpells.ChaosThrust.Name, true, 6000) ||
                ActionManager.HasSpell(MySpells.Disembowel.Name) && !Core.Player.CurrentTarget.HasAura(820))
            {
                return await MySpells.ImpulseDrive.Cast();
            }
            return false;
        }

        private async Task<bool> Disembowel()
        {
            if (ActionManager.LastSpell.Name == MySpells.ImpulseDrive.Name)
            {
                return await MySpells.Disembowel.Cast();
            }
            return false;
        }

        private async Task<bool> ChaosThrust()
        {
            if (ActionManager.LastSpell.Name == MySpells.Disembowel.Name)
            {
                return await MySpells.ChaosThrust.Cast();
            }
            return false;
        }

        private async Task<bool> HeavyThrust()
        {
            if (!Core.Player.HasAura(MySpells.HeavyThrust.Name, true, 6000))
            {
                return await MySpells.HeavyThrust.Cast();
            }
            return false;
        }

        private async Task<bool> FangAndClaw()
        {
            return await MySpells.FangAndClaw.Cast();
        }

        private async Task<bool> WheelingThrust()
        {
            return await MySpells.WheelingThrust.Cast();
        }

        #endregion

        #region AoE

        private async Task<bool> DoomSpike()
        {
            if (Core.Player.CurrentTPPercent > 30 && Core.Player.HasAura(MySpells.HeavyThrust.Name) && Helpers.EnemiesNearTarget(5) > 2)
            {
                return await MySpells.DoomSpike.Cast();
            }
            return false;
        }

        private async Task<bool> SonicThrust()
        {
            if (ActionManager.LastSpell.Name == MySpells.DoomSpike.Name && Core.Player.CurrentTPPercent > 30 &&
                Core.Player.HasAura(MySpells.HeavyThrust.Name) && Helpers.EnemiesNearTarget(5) > 2)
            {
                return await MySpells.SonicThrust.Cast();
            }
            return false;
        }

        #endregion

        #region Cooldown

        private async Task<bool> Jump()
        {
            if (Shinra.Settings.DragoonJump && !MovementManager.IsMoving && !RecentJump && UseJump &&
                Core.Player.HasAura(MySpells.HeavyThrust.Name))
            {
                return await MySpells.Jump.Cast();
            }
            return false;
        }

        private async Task<bool> SpineshatterDive()
        {
            if (Shinra.Settings.DragoonSpineshatter && !MovementManager.IsMoving && !RecentJump && UseJump &&
                Core.Player.HasAura(MySpells.HeavyThrust.Name))
            {
                return await MySpells.SpineshatterDive.Cast();
            }
            return false;
        }

        private async Task<bool> DragonfireDive()
        {
            if (Shinra.Settings.DragoonDragonfire && !MovementManager.IsMoving && !RecentJump &&
                Core.Player.HasAura(MySpells.HeavyThrust.Name))
            {
                return await MySpells.DragonfireDive.Cast();
            }
            return false;
        }

        private async Task<bool> Geirskogul()
        {
            if (Shinra.Settings.DragoonGeirskogul && Core.Player.HasAura(MySpells.HeavyThrust.Name))
            {
                if (NumEyes == 4 || JumpCooldown > 25 && SpineCooldown > 25 || Core.Player.ClassLevel < 70)
                {
                    return await MySpells.Geirskogul.Cast();
                }
            }
            return false;
        }

        private async Task<bool> MirageDive()
        {
            if (Shinra.Settings.DragoonMirage && !MovementManager.IsMoving && !RecentJump && Core.Player.HasAura(MySpells.HeavyThrust.Name))
            {
                return await MySpells.MirageDive.Cast();
            }
            return false;
        }

        #endregion

        #region Buff

        private async Task<bool> LifeSurge()
        {
            if (Shinra.Settings.DragoonLifeSurge && ActionManager.LastSpell.Name == MySpells.VorpalThrust.Name)
            {
                return await MySpells.LifeSurge.Cast();
            }
            return false;
        }

        private async Task<bool> BloodForBlood()
        {
            if (Shinra.Settings.DragoonBloodForBlood)
            {
                return await MySpells.BloodForBlood.Cast();
            }
            return false;
        }

        private async Task<bool> BattleLitany()
        {
            if (Shinra.Settings.DragoonBattleLitany)
            {
                return await MySpells.BattleLitany.Cast();
            }
            return false;
        }

        private async Task<bool> BloodOfTheDragon()
        {
            if (Shinra.Settings.DragoonBloodOfTheDragon && BloodTimer == 0 &&
                (ActionManager.LastSpell.Name == MySpells.VorpalThrust.Name || ActionManager.LastSpell.Name == MySpells.Disembowel.Name))
            {
                return await MySpells.BloodOfTheDragon.Cast();
            }
            return false;
        }

        #endregion

        #region Role

        private async Task<bool> SecondWind()
        {
            if (Shinra.Settings.DragoonSecondWind && Core.Player.CurrentHealthPercent < Shinra.Settings.DragoonSecondWindPct)
            {
                return await MySpells.Role.SecondWind.Cast();
            }
            return false;
        }

        private async Task<bool> Invigorate()
        {
            if (Shinra.Settings.DragoonInvigorate && Core.Player.CurrentTPPercent < Shinra.Settings.DragoonInvigoratePct)
            {
                return await MySpells.Role.Invigorate.Cast();
            }
            return false;
        }

        private async Task<bool> Bloodbath()
        {
            if (Shinra.Settings.DragoonBloodbath && Core.Player.CurrentHealthPercent < Shinra.Settings.DragoonBloodbathPct)
            {
                return await MySpells.Role.Bloodbath.Cast();
            }
            return false;
        }

        private async Task<bool> TrueNorth()
        {
            if (Shinra.Settings.DragoonTrueNorth)
            {
                return await MySpells.Role.TrueNorth.Cast();
            }
            return false;
        }

        #endregion

        #region Custom

        private static bool RecentJump { get { return Spell.RecentSpell.Keys.Any(rs => rs.Contains("Dive") || rs.Contains("Jump")); } }
        private static double BloodTimer => Resource.Timer.TotalMilliseconds;
        private static double JumpCooldown => DataManager.GetSpellData(92).Cooldown.TotalSeconds;
        private static double SpineCooldown => DataManager.GetSpellData(95).Cooldown.TotalSeconds;
        private static int NumEyes => Resource.DragonGaze;

        private bool UseJump => BloodTimer > 500 || !ActionManager.HasSpell(MySpells.BloodOfTheDragon.Name);

        #endregion
    }
}