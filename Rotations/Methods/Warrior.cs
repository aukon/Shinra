using System.Threading.Tasks;
using ff14bot;
using ff14bot.Managers;
using ShinraCo.Settings;
using ShinraCo.Spells.Main;
using Resource = ff14bot.Managers.ActionResourceManager.Warrior;

namespace ShinraCo.Rotations
{
    public sealed partial class Warrior
    {
        private WarriorSpells MySpells { get; } = new WarriorSpells();

        #region Damage

        private async Task<bool> HeavySwing()
        {
            return await MySpells.HeavySwing.Cast();
        }

        private async Task<bool> SkullSunder()
        {
            if (ActionManager.LastSpell.Name == MySpells.HeavySwing.Name)
            {
                return await MySpells.SkullSunder.Cast();
            }
            return false;
        }

        private async Task<bool> ButchersBlock()
        {
            if (ActionManager.LastSpell.Name == MySpells.SkullSunder.Name)
            {
                return await MySpells.ButchersBlock.Cast();
            }
            return false;
        }

        private async Task<bool> Maim()
        {
            if (ActionManager.LastSpell.Name == MySpells.HeavySwing.Name)
            {
                if (Shinra.Settings.TankMode == TankModes.DPS && ActionManager.HasSpell(MySpells.StormsPath.Name) ||
                    Shinra.Settings.WarriorMaim && !Core.Player.CurrentTarget.HasAura(819, false, 6000) ||
                    Shinra.Settings.WarriorStormsEye && ActionManager.HasSpell(MySpells.StormsEye.Name) &&
                    !Core.Player.HasAura(MySpells.StormsEye.Name, true, 6000))
                {
                    return await MySpells.Maim.Cast();
                }
            }
            return false;
        }

        private async Task<bool> StormsPath()
        {
            if (ActionManager.LastSpell.Name == MySpells.Maim.Name)
            {
                return await MySpells.StormsPath.Cast();
            }
            return false;
        }

        private async Task<bool> StormsEye()
        {
            if (ActionManager.LastSpell.Name == MySpells.Maim.Name && !Core.Player.HasAura(MySpells.StormsEye.Name, true, 6000))
            {
                return await MySpells.StormsEye.Cast();
            }
            return false;
        }

        private async Task<bool> InnerBeast()
        {
            if (Shinra.Settings.WarriorInnerBeast && DefianceStance && BeastValue >= 50 && !Core.Player.HasAura(MySpells.InnerBeast.Name))
            {
                return await MySpells.InnerBeast.Cast();
            }
            return false;
        }

        private async Task<bool> FellCleave()
        {
            if (Shinra.Settings.WarriorFellCleave && DeliveranceStance)
            {
                if (BeastValue >= 90 || Core.Player.CurrentTarget.HasAura(819) && Core.Player.HasAura(MySpells.StormsEye.Name))
                {
                    return await MySpells.FellCleave.Cast();
                }
            }
            return false;
        }

        #endregion

        #region AoE

        private async Task<bool> Overpower()
        {
            if (Shinra.Settings.WarriorOverpower && Core.Player.CurrentTPPercent > 30)
            {
                return await MySpells.Overpower.Cast();
            }
            return false;
        }

        private async Task<bool> SteelCyclone()
        {
            if (Shinra.Settings.WarriorSteelCyclone && DefianceStance && BeastValue >= 50)
            {
                return await MySpells.SteelCyclone.Cast();
            }
            return false;
        }

        private async Task<bool> Decimate()
        {
            if (Shinra.Settings.WarriorDecimate && DeliveranceStance && BeastValue >= 50)
            {
                return await MySpells.Decimate.Cast();
            }
            return false;
        }

        #endregion

        #region Cooldown

        private async Task<bool> Onslaught()
        {
            if (Shinra.Settings.WarriorOnslaught && Core.Player.TargetDistance(10))
            {
                return await MySpells.Onslaught.Cast(null, false);
            }
            return false;
        }

        private async Task<bool> Upheaval()
        {
            if (Shinra.Settings.WarriorUpheaval && Core.Player.CurrentHealthPercent > 70 &&
                (Core.Player.HasAura(MySpells.InnerRelease.Name) || MySpells.InnerRelease.Cooldown() > 8000) || Core.Player.ClassLevel < 70)
            {
                var count = Shinra.Settings.CustomAoE ? Shinra.Settings.CustomAoECount : 3;

                if (Shinra.Settings.RotationMode == Modes.Single ||
                    Shinra.Settings.RotationMode == Modes.Smart && Helpers.EnemiesNearTarget(5) < count)
                {
                    return await MySpells.Upheaval.Cast();
                }
            }
            return false;
        }

        #endregion

        #region Buff

        private async Task<bool> Berserk()
        {
            if (Shinra.Settings.WarriorBerserk && Core.Player.ClassLevel < 70)
            {
                return await MySpells.Berserk.Cast();
            }
            return false;
        }

        private async Task<bool> ThrillOfBattle()
        {
            if (Shinra.Settings.WarriorThrillOfBattle && Core.Player.CurrentHealthPercent < Shinra.Settings.WarriorThrillOfBattlePct)
            {
                return await MySpells.ThrillOfBattle.Cast();
            }
            return false;
        }

        private async Task<bool> Unchained()
        {
            if (Shinra.Settings.WarriorUnchained && DefianceStance)
            {
                return await MySpells.Unchained.Cast();
            }
            return false;
        }

        private async Task<bool> Vengeance()
        {
            if (Shinra.Settings.WarriorVengeance && Core.Player.CurrentHealthPercent < Shinra.Settings.WarriorVengeancePct)
            {
                return await MySpells.Vengeance.Cast();
            }
            return false;
        }

        private async Task<bool> Infuriate()
        {
            if (Shinra.Settings.WarriorInfuriate && BeastDeficit >= 50)
            {
                return await MySpells.Infuriate.Cast();
            }
            return false;
        }

        private async Task<bool> EquilibriumTP()
        {
            if (Shinra.Settings.WarriorEquilibriumTP && DeliveranceStance &&
                Core.Player.CurrentTPPercent < Shinra.Settings.WarriorEquilibriumTPPct)
            {
                return await MySpells.Equilibrium.Cast();
            }
            return false;
        }

        private async Task<bool> ShakeItOff()
        {
            if (Shinra.Settings.WarriorShakeItOff)
            {
                return await MySpells.ShakeItOff.Cast(null, false);
            }
            return false;
        }

        private async Task<bool> InnerRelease()
        {
            if (Shinra.Settings.WarriorInnerRelease && DeliveranceStance)
            {
                return await MySpells.InnerRelease.Cast();
            }
            return false;
        }

        #endregion

        #region Heal

        private async Task<bool> Equilibrium()
        {
            if (Shinra.Settings.WarriorEquilibrium && DefianceStance &&
                Core.Player.CurrentHealthPercent < Shinra.Settings.WarriorEquilibriumPct)
            {
                return await MySpells.Equilibrium.Cast();
            }
            return false;
        }

        #endregion

        #region Stance

        private async Task<bool> Defiance()
        {
            if (Shinra.Settings.WarriorStance == WarriorStances.Defiance || Shinra.Settings.WarriorStance == WarriorStances.Deliverance &&
                !ActionManager.HasSpell(MySpells.Deliverance.Name))
            {
                if (!DefianceStance)
                {
                    return await MySpells.Defiance.Cast();
                }
            }
            return false;
        }

        private async Task<bool> Deliverance()
        {
            if (Shinra.Settings.WarriorStance == WarriorStances.Deliverance)
            {
                if (!DeliveranceStance)
                {
                    return await MySpells.Deliverance.Cast();
                }
            }
            return false;
        }

        #endregion

        #region Role

        private async Task<bool> Rampart()
        {
            if (Shinra.Settings.WarriorRampart && Core.Player.CurrentHealthPercent < Shinra.Settings.WarriorRampartPct)
            {
                return await MySpells.Role.Rampart.Cast();
            }
            return false;
        }

        private async Task<bool> Convalescence()
        {
            if (Shinra.Settings.WarriorConvalescence && Core.Player.CurrentHealthPercent < Shinra.Settings.WarriorConvalescencePct)
            {
                return await MySpells.Role.Convalescence.Cast();
            }
            return false;
        }

        private async Task<bool> Anticipation()
        {
            if (Shinra.Settings.WarriorAnticipation && Core.Player.CurrentHealthPercent < Shinra.Settings.WarriorAnticipationPct)
            {
                return await MySpells.Role.Anticipation.Cast();
            }
            return false;
        }

        private async Task<bool> Reprisal()
        {
            if (Shinra.Settings.WarriorReprisal)
            {
                return await MySpells.Role.Reprisal.Cast();
            }
            return false;
        }

        private async Task<bool> Awareness()
        {
            if (Shinra.Settings.WarriorAwareness && Core.Player.CurrentHealthPercent < Shinra.Settings.WarriorAwarenessPct)
            {
                return await MySpells.Role.Awareness.Cast();
            }
            return false;
        }

        #endregion

        #region Custom

        private int BeastValue => Core.Player.HasAura(MySpells.InnerRelease.Name) ? 100 : Resource.BeastGauge;
        private int BeastDeficit => 100 - BeastValue;

        private bool DefianceStance => Core.Player.HasAura(MySpells.Defiance.Name);
        private bool DeliveranceStance => Core.Player.HasAura(MySpells.Deliverance.Name);

        #endregion
    }
}