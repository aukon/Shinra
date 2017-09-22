using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using ShinraCo.Settings;
using ShinraCo.Spells.Main;
using ShinraCo.Spells.Opener;
using Resource = ff14bot.Managers.ActionResourceManager.Machinist;

namespace ShinraCo.Rotations
{
    public sealed partial class Machinist
    {
        private MachinistSpells MySpells { get; } = new MachinistSpells();
        private MachinistOpener MyOpener { get; } = new MachinistOpener();

        #region Damage

        private async Task<bool> SplitShot()
        {
            return await MySpells.SplitShot.Cast();
        }

        private async Task<bool> SlugShot()
        {
            if (Core.Player.HasAura("Enhanced Slug Shot"))
            {
                return await MySpells.SlugShot.Cast();
            }
            return false;
        }

        private async Task<bool> CleanShot()
        {
            if (Core.Player.HasAura("Cleaner Shot"))
            {
                return await MySpells.CleanShot.Cast();
            }
            return false;
        }

        private async Task<bool> HotShot()
        {
            if (!Core.Player.HasAura(MySpells.HotShot.Name, true, 6000))
            {
                return await MySpells.HotShot.Cast();
            }
            return false;
        }

        #endregion

        #region AoE

        private async Task<bool> SpreadShot()
        {
            if (Core.Player.CurrentTPPercent > 40)
            {
                if (Shinra.Settings.RotationMode == Modes.Multi || Shinra.Settings.RotationMode == Modes.Smart &&
                    Helpers.EnemiesNearTarget(5) > 2)
                {
                    return await MySpells.SpreadShot.Cast();
                }
            }
            return false;
        }

        #endregion

        #region Cooldown

        private async Task<bool> Heartbreak()
        {
            return await MySpells.Heartbreak.Cast();
        }

        private async Task<bool> Wildfire()
        {
            if (UseWildfire && (!Shinra.Settings.MachinistSyncOverheat || Overheated))
            {
                return await MySpells.Wildfire.Cast();
            }
            return false;
        }

        private async Task<bool> GaussRound()
        {
            return await MySpells.GaussRound.Cast();
        }

        private async Task<bool> Ricochet()
        {
            if (Shinra.Settings.MachinistRicochet)
            {
                if (!Shinra.Settings.MachinistSyncWildfire || Core.Player.CurrentTarget.HasAura(MySpells.Wildfire.Name, true))
                {
                    return await MySpells.Ricochet.Cast();
                }
            }
            return false;
        }

        private async Task<bool> Cooldown()
        {
            if (Shinra.Settings.MachinistCooldown)
            {
                if (Overheated && !Core.Player.HasAura("Enhanced Slug Shot") && !Core.Player.HasAura("Cleaner Shot") && Resource.Ammo < 2)
                {
                    return await MySpells.Cooldown.Cast();
                }
                if (!Overheated && Resource.Heat >= 90 && (!ActionManager.CanCast(MySpells.BarrelStabilizer.Name, Core.Player) ||
                                                           !UseWildfire || WildfireCooldown > 3000))
                {
                    return await MySpells.Cooldown.Cast();
                }
            }
            return false;
        }

        private async Task<bool> Flamethrower()
        {
            if (Shinra.Settings.MachinistFlamethrower && Resource.Heat < 100 && !MovementManager.IsMoving)
            {
                if (BarrelCooldown < 30000 && UseWildfire && WildfireCooldown < 3000 || UseFlamethrower)
                {
                    if (await MySpells.Flamethrower.Cast())
                    {
                        return await Coroutine.Wait(3000, () => Core.Player.HasAura(MySpells.Flamethrower.Name));
                    }
                }
            }
            return false;
        }

        private async Task<bool> FlamethrowerBuff()
        {
            if (Core.Player.HasAura(MySpells.Flamethrower.Name) &&
                (!Shinra.Settings.MachinistFlamethrower || Resource.Heat < 100 || UseFlamethrower))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Buff

        private async Task<bool> Reload()
        {
            if (Shinra.Settings.MachinistReload)
            {
                if (!Shinra.Settings.MachinistSyncWildfire || Core.Player.CurrentTarget.HasAura(MySpells.Wildfire.Name, true) ||
                    WildfireCooldown > 25000)
                {
                    if (Resource.Ammo == 0 && !Core.Player.HasAura("Enhanced Slug Shot") && !Core.Player.HasAura("Cleaner Shot") &&
                        (Core.Player.HasAura(MySpells.HotShot.Name, true, 10000) || !ActionManager.HasSpell(MySpells.HotShot.Name)))
                    {
                        return await MySpells.Reload.Cast();
                    }
                }
            }
            return false;
        }

        private async Task<bool> Reassemble()
        {
            if (Shinra.Settings.MachinistReassemble)
            {
                if (!Shinra.Settings.MachinistSyncWildfire || Core.Player.CurrentTarget.HasAura(MySpells.Wildfire.Name, true))
                {
                    if (Core.Player.HasAura("Cleaner Shot") || !ActionManager.HasSpell(MySpells.CleanShot.Name) &&
                        Core.Player.HasAura("Enhanced Slug Shot"))
                    {
                        return await MySpells.Reassemble.Cast();
                    }
                }
            }
            return false;
        }

        private async Task<bool> QuickReload()
        {
            if (!Shinra.Settings.MachinistSyncWildfire || Core.Player.CurrentTarget.HasAura(MySpells.Wildfire.Name, true) ||
                WildfireCooldown > 10000)
            {
                if (Resource.Ammo < 3 && !Core.Player.HasAura("Enhanced Slug Shot") && !Core.Player.HasAura("Cleaner Shot"))
                {
                    return await MySpells.QuickReload.Cast();
                }
            }
            return false;
        }

        private async Task<bool> QuickReloadPre()
        {
            if (Resource.Ammo < 2 && Shinra.LastSpell.Name != MySpells.HotShot.Name && Shinra.LastSpell.Name != MySpells.GaussRound.Name)
            {
                return await MySpells.QuickReload.Cast(null, false);
            }
            return false;
        }

        private async Task<bool> RapidFire()
        {
            if (Shinra.Settings.MachinistRapidFire)
            {
                if (!Shinra.Settings.MachinistSyncWildfire || Core.Player.CurrentTarget.HasAura(MySpells.Wildfire.Name, true))
                {
                    return await MySpells.RapidFire.Cast();
                }
            }
            return false;
        }

        private async Task<bool> GaussBarrel()
        {
            if (Shinra.Settings.MachinistGaussBarrel && !Resource.GaussBarrel)
            {
                return await MySpells.GaussBarrel.Cast(null, false);
            }
            return false;
        }

        private async Task<bool> Hypercharge()
        {
            if (Shinra.Settings.MachinistHypercharge && TurretExists)
            {
                return await MySpells.Hypercharge.Cast();
            }
            return false;
        }

        private async Task<bool> BarrelStabilizer()
        {
            if (Shinra.Settings.MachinistBarrelStabilizer)
            {
                if (Resource.Heat < 30 && (!Shinra.Settings.MachinistFlamethrower || !ActionManager.HasSpell(MySpells.Flamethrower.Name) ||
                                           FlamethrowerCooldown > 3000))
                {
                    return await MySpells.BarrelStabilizer.Cast();
                }
            }
            return false;
        }

        #endregion

        #region Turret

        private async Task<bool> RookAutoturret()
        {
            if (Shinra.Settings.MachinistTurret == MachinistTurrets.Rook || Shinra.Settings.MachinistTurret == MachinistTurrets.Bishop &&
                !ActionManager.HasSpell(MySpells.BishopAutoturret.Name))
            {
                if (PetManager.ActivePetType != PetType.Rook_Autoturret || TurretDistance > 20)
                {
                    return await MySpells.RookAutoturret.Cast();
                }
            }
            return false;
        }

        private async Task<bool> BishopAutoturret()
        {
            if (Shinra.Settings.MachinistTurret == MachinistTurrets.Bishop)
            {
                if (PetManager.ActivePetType != PetType.Bishop_Autoturret || TurretDistance > 20)
                {
                    return await MySpells.BishopAutoturret.Cast();
                }
            }
            return false;
        }

        #endregion

        #region Opener

        private async Task<bool> Opener()
        {
            if (!Shinra.Settings.MachinistOpener || Shinra.OpenerFinished || Core.Player.ClassLevel < 70)
            {
                return false;
            }

            if (Core.Player.HasAura(MySpells.Flamethrower.Name) && !Overheated)
            {
                return true;
            }

            if (PetManager.ActivePetType != PetType.Rook_Autoturret || TurretDistance > 20)
            {
                if (await MySpells.RookAutoturret.Cast(null, false))
                {
                    return true;
                }
            }

            if (TurretExists)
            {
                if (await MySpells.Hypercharge.Cast(null, false))
                {
                    return true;
                }
            }

            var spell = MyOpener.Spells.ElementAt(Shinra.OpenerStep);
            Helpers.Debug($"Executing opener step {Shinra.OpenerStep} >>> {spell.Name}");
            if (await spell.Cast(null, false) || spell.Cooldown(true) > 2500 && spell.Cooldown() > 0)
            {
                Shinra.OpenerStep++;
                if (spell.Name == MySpells.Flamethrower.Name)
                {
                    await Coroutine.Wait(3000, () => Core.Player.HasAura(MySpells.Flamethrower.Name));
                }
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

        private async Task<bool> SecondWind()
        {
            if (Shinra.Settings.MachinistSecondWind && Core.Player.CurrentHealthPercent < Shinra.Settings.MachinistSecondWindPct)
            {
                return await MySpells.Role.SecondWind.Cast();
            }
            return false;
        }

        private async Task<bool> Peloton()
        {
            if (Shinra.Settings.MachinistPeloton && !Core.Player.HasAura(MySpells.Role.Peloton.Name) && !Core.Player.HasTarget &&
                MovementManager.IsMoving)
            {
                return await MySpells.Role.Peloton.Cast(null, false);
            }
            return false;
        }

        private async Task<bool> Invigorate()
        {
            if (Shinra.Settings.MachinistInvigorate && Core.Player.CurrentTPPercent < Shinra.Settings.MachinistInvigoratePct)
            {
                return await MySpells.Role.Invigorate.Cast();
            }
            return false;
        }

        private async Task<bool> Tactician()
        {
            if (Shinra.Settings.MachinistTactician && Core.Player.CurrentTPPercent < Shinra.Settings.MachinistTacticianPct)
            {
                return await MySpells.Role.Tactician.Cast();
            }
            return false;
        }

        #endregion

        #region Custom

        private static double FlamethrowerCooldown => DataManager.GetSpellData(7418).Cooldown.TotalMilliseconds;
        private static double WildfireCooldown => DataManager.GetSpellData(2878).Cooldown.TotalMilliseconds;
        private static double BarrelCooldown => DataManager.GetSpellData(7414).Cooldown.TotalMilliseconds;
        private static bool Overheated => Resource.Heat == 100 && Resource.Timer.TotalMilliseconds > 0;
        private static bool UseFlamethrower => Shinra.Settings.RotationMode == Modes.Multi ||
                                               Shinra.Settings.RotationMode == Modes.Smart && Helpers.EnemiesNearTarget(5) > 2;

        private static bool UseWildfire => Shinra.Settings.MachinistWildfire &&
                                           (Core.Player.CurrentTarget.IsBoss() ||
                                            Core.Player.CurrentTarget.CurrentHealth > Shinra.Settings.MachinistWildfireHP);

        private static bool TurretExists => Core.Player.Pet != null;
        private static float TurretDistance => TurretExists && Core.Player.HasTarget && Core.Player.CurrentTarget.CanAttack
            ? Core.Player.Pet.Distance2D(Core.Player.CurrentTarget) - Core.Player.CurrentTarget.CombatReach : 0;

        #endregion
    }
}