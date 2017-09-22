using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using ShinraCo.Settings;
using ShinraCo.Spells.Main;
using ShinraCo.Spells.Opener;
using Resource = ff14bot.Managers.ActionResourceManager.Paladin;

namespace ShinraCo.Rotations
{
    public sealed partial class Paladin
    {
        private PaladinSpells MySpells { get; } = new PaladinSpells();
        private PaladinOpener MyOpener { get; } = new PaladinOpener();

        #region Damage

        private async Task<bool> FastBlade()
        {
            return await MySpells.FastBlade.Cast();
        }

        private async Task<bool> SavageBlade()
        {
            if (ActionManager.LastSpell.Name == MySpells.FastBlade.Name)
            {
                return await MySpells.SavageBlade.Cast();
            }
            return false;
        }

        private async Task<bool> RageOfHalone()
        {
            if (ActionManager.LastSpell.Name == MySpells.SavageBlade.Name)
            {
                return await MySpells.RageOfHalone.Cast();
            }
            return false;
        }

        private async Task<bool> RiotBlade()
        {
            if (ActionManager.LastSpell.Name == MySpells.FastBlade.Name)
            {
                if (Shinra.Settings.TankMode == TankModes.DPS && ActionManager.HasSpell(MySpells.RoyalAuthority.Name) ||
                    Shinra.Settings.PaladinGoringBlade && ActionManager.HasSpell(MySpells.GoringBlade.Name) &&
                    !Core.Player.CurrentTarget.HasAura(MySpells.GoringBlade.Name, true, 4000) || Core.Player.CurrentManaPercent < 40)
                {
                    return await MySpells.RiotBlade.Cast();
                }
            }
            return false;
        }

        private async Task<bool> GoringBlade()
        {
            if (ActionManager.LastSpell.Name == MySpells.RiotBlade.Name &&
                !Core.Player.CurrentTarget.HasAura(MySpells.GoringBlade.Name, true, 4000))
            {
                return await MySpells.GoringBlade.Cast();
            }
            return false;
        }

        private async Task<bool> RoyalAuthority()
        {
            if (ActionManager.LastSpell.Name == MySpells.RiotBlade.Name)
            {
                return await MySpells.RoyalAuthority.Cast();
            }
            return false;
        }

        private async Task<bool> HolySpirit()
        {
            if (Shinra.Settings.TankMode == TankModes.DPS)
            {
                if (!MovementManager.IsMoving && Core.Player.HasAura(MySpells.Requiescat.Name, true, 2000))
                {
                    return await MySpells.HolySpirit.Cast();
                }
            }
            return false;
        }

        private async Task<bool> ShieldLob()
        {
            if (Core.Player.TargetDistance(10))
            {
                return await MySpells.ShieldLob.Cast();
            }
            return false;
        }

        #endregion

        #region AoE

        private async Task<bool> Flash()
        {
            if (Shinra.Settings.PaladinFlash && Core.Player.CurrentManaPercent > 40)
            {
                return await MySpells.Flash.Cast();
            }
            return false;
        }

        private async Task<bool> TotalEclipse()
        {
            if (Shinra.Settings.PaladinTotalEclipse && Core.Player.CurrentTPPercent > 40)
            {
                return await MySpells.TotalEclipse.Cast();
            }
            return false;
        }

        #endregion

        #region Cooldown

        private async Task<bool> ShieldSwipe()
        {
            if (Shinra.Settings.PaladinShieldSwipe)
            {
                return await MySpells.ShieldSwipe.Cast();
            }
            return false;
        }

        private async Task<bool> SpiritsWithin()
        {
            if (Shinra.Settings.PaladinSpiritsWithin)
            {
                return await MySpells.SpiritsWithin.Cast();
            }
            return false;
        }

        private async Task<bool> CircleOfScorn()
        {
            if (Shinra.Settings.PaladinCircleOfScorn)
            {
                if (Core.Player.TargetDistance(5, false))
                {
                    return await MySpells.CircleOfScorn.Cast();
                }
            }
            return false;
        }

        private async Task<bool> Requiescat()
        {
            if (Shinra.Settings.PaladinRequiescat)
            {
                if (!MovementManager.IsMoving && Core.Player.CurrentManaPercent > 80 &&
                    Core.Player.CurrentTarget.HasAura(MySpells.GoringBlade.Name, true, 4000) &&
                    !Core.Player.HasAura(MySpells.FightOrFlight.Name))
                {
                    return await MySpells.Requiescat.Cast();
                }
            }
            return false;
        }

        #endregion

        #region Buff

        private async Task<bool> FightOrFlight()
        {
            if (Shinra.Settings.PaladinFightOrFlight)
            {
                if (!Core.Player.HasAura(MySpells.Requiescat.Name) && Core.Player.TargetDistance(5, false))
                {
                    return await MySpells.FightOrFlight.Cast();
                }
            }
            return false;
        }

        private async Task<bool> Sentinel()
        {
            if (Shinra.Settings.PaladinSentinel && Core.Player.CurrentHealthPercent < Shinra.Settings.PaladinSentinelPct)
            {
                return await MySpells.Sentinel.Cast();
            }
            return false;
        }

        private async Task<bool> Bulwark()
        {
            if (Shinra.Settings.PaladinBulwark && Core.Player.CurrentHealthPercent < Shinra.Settings.PaladinBulwarkPct)
            {
                return await MySpells.Bulwark.Cast();
            }
            return false;
        }

        private async Task<bool> HallowedGround()
        {
            if (Shinra.Settings.PaladinHallowedGround && Core.Player.CurrentHealthPercent < Shinra.Settings.PaladinHallowedGroundPct)
            {
                return await MySpells.HallowedGround.Cast(null, false);
            }
            return false;
        }

        private async Task<bool> Sheltron()
        {
            if (Shinra.Settings.PaladinSheltron && !Core.Player.HasAura(MySpells.Sheltron.Name))
            {
                if (OathValue == 100 || OathValue > 50 && Core.Player.CurrentManaPercent < 70)
                {
                    return await MySpells.Sheltron.Cast();
                }
            }
            return false;
        }

        private async Task<bool> PassageOfArms()
        {
            if (Core.Player.HasAura(MySpells.PassageOfArms.Name))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Heal

        private async Task<bool> Clemency()
        {
            if (Shinra.Settings.PaladinClemency && Core.Player.CurrentHealthPercent < Shinra.Settings.PaladinClemencyPct)
            {
                if (Core.Player.CurrentManaPercent > 40 && !MovementManager.IsMoving)
                {
                    var target = Core.Player;

                    if (target != null)
                    {
                        return await MySpells.Clemency.Cast(target);
                    }
                }
            }
            return false;
        }

        #endregion

        #region Oath

        private async Task<bool> ShieldOath()
        {
            if (Shinra.Settings.PaladinOath == PaladinOaths.Shield || Shinra.Settings.PaladinOath == PaladinOaths.Sword &&
                !ActionManager.HasSpell(MySpells.SwordOath.Name))
            {
                if (!Core.Player.HasAura(MySpells.ShieldOath.Name))
                {
                    return await MySpells.ShieldOath.Cast();
                }
            }
            return false;
        }

        private async Task<bool> SwordOath()
        {
            if (Shinra.Settings.PaladinOath == PaladinOaths.Sword)
            {
                if (!Core.Player.HasAura(MySpells.SwordOath.Name))
                {
                    return await MySpells.SwordOath.Cast();
                }
            }
            return false;
        }

        #endregion

        #region Opener

        private async Task<bool> Opener()
        {
            if (!Shinra.Settings.PaladinOpener || Shinra.OpenerFinished || Core.Player.ClassLevel < 70)
            {
                return false;
            }

            var spell = MyOpener.Spells.ElementAt(Shinra.OpenerStep);
            Helpers.Debug($"Executing opener step {Shinra.OpenerStep} >>> {spell.Name}");
            if (await spell.Cast(null, false) || spell.Cooldown(true) > 2500 && spell.Cooldown() > 0 && !Core.Player.IsCasting)
            {
                Shinra.OpenerStep++;
            }

            if (Shinra.OpenerStep >= MyOpener.Spells.Count)
            {
                Helpers.Debug("Opener finished.");
                Shinra.OpenerFinished = true;
            }
            return true;
        }

        #endregion

        #region Role

        private async Task<bool> Rampart()
        {
            if (Shinra.Settings.PaladinRampart && Core.Player.CurrentHealthPercent < Shinra.Settings.PaladinRampartPct)
            {
                return await MySpells.Role.Rampart.Cast();
            }
            return false;
        }

        private async Task<bool> Convalescence()
        {
            if (Shinra.Settings.PaladinConvalescence && Core.Player.CurrentHealthPercent < Shinra.Settings.PaladinConvalescencePct)
            {
                return await MySpells.Role.Convalescence.Cast();
            }
            return false;
        }

        private async Task<bool> Anticipation()
        {
            if (Shinra.Settings.PaladinAnticipation && Core.Player.CurrentHealthPercent < Shinra.Settings.PaladinAnticipationPct)
            {
                return await MySpells.Role.Anticipation.Cast();
            }
            return false;
        }

        private async Task<bool> Reprisal()
        {
            if (Shinra.Settings.PaladinReprisal)
            {
                return await MySpells.Role.Reprisal.Cast();
            }
            return false;
        }

        private async Task<bool> Awareness()
        {
            if (Shinra.Settings.PaladinAwareness && Core.Player.CurrentHealthPercent < Shinra.Settings.PaladinAwarenessPct)
            {
                return await MySpells.Role.Awareness.Cast();
            }
            return false;
        }

        #endregion

        #region Custom

        private static int OathValue => Resource.Oath;

        #endregion
    }
}