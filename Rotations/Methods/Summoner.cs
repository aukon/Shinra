using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using ShinraCo.Settings;
using ShinraCo.Spells;
using ShinraCo.Spells.Main;

namespace ShinraCo.Rotations
{
    public sealed partial class Summoner
    {
        private SummonerSpells MySpells { get; } = new SummonerSpells();

        #region Damage

        private async Task<bool> Ruin()
        {
            return await MySpells.Ruin.Cast();
        }

        private async Task<bool> RuinII()
        {
            if (MovementManager.IsMoving || UseBane || UseFester || UsePainflare || UsePet)
            {
                return await MySpells.RuinII.Cast();
            }
            return false;
        }

        private async Task<bool> RuinIII()
        {
            if (Core.Player.CurrentManaPercent > 80 || Core.Player.HasAura(MySpells.DreadwyrmTrance.Name))
            {
                return await MySpells.RuinIII.Cast();
            }
            return false;
        }

        #endregion

        #region DoT

        private async Task<bool> Bio()
        {
            if (!ActionManager.HasSpell(MySpells.BioII.Name) && !Core.Player.CurrentTarget.HasAura(MySpells.Bio.Name, true, 3000))
            {
                return await MySpells.Bio.Cast();
            }
            return false;
        }

        private async Task<bool> BioII()
        {
            if (!ActionManager.HasSpell(MySpells.BioIII.Name) && !RecentDoT &&
                !Core.Player.CurrentTarget.HasAura(MySpells.BioII.Name, true, 3000))
            {
                return await MySpells.BioII.Cast();
            }
            return false;
        }

        private async Task<bool> BioIII()
        {
            if (!RecentDoT && !Core.Player.CurrentTarget.HasAura(MySpells.BioIII.Name, true, 3000))
            {
                return await MySpells.BioIII.Cast();
            }
            return false;
        }

        private async Task<bool> Miasma()
        {
            if (!ActionManager.HasSpell(MySpells.MiasmaIII.Name) && !RecentDoT &&
                !Core.Player.CurrentTarget.HasAura(MySpells.Miasma.Name, true, 4000))
            {
                return await MySpells.Miasma.Cast();
            }
            return false;
        }

        private async Task<bool> MiasmaIII()
        {
            if (!RecentDoT && !Core.Player.CurrentTarget.HasAura(MySpells.MiasmaIII.Name, true, 4000))
            {
                return await MySpells.MiasmaIII.Cast();
            }
            return false;
        }

        #endregion

        #region AoE

        private async Task<bool> Bane()
        {
            if (UseBane)
            {
                return await MySpells.Bane.Cast();
            }
            return false;
        }

        private async Task<bool> Painflare()
        {
            if (UsePainflare)
            {
                return await MySpells.Painflare.Cast();
            }
            return false;
        }

        #endregion

        #region Cooldown

        private async Task<bool> EnergyDrain()
        {
            if (!ActionManager.HasSpell(MySpells.Fester.Name) && !AetherLow)
            {
                return await MySpells.EnergyDrain.Cast();
            }
            return false;
        }

        private async Task<bool> Fester()
        {
            if (UseFester)
            {
                return await MySpells.Fester.Cast();
            }
            return false;
        }

        private async Task<bool> ShadowFlare()
        {
            if (!MovementManager.IsMoving)
            {
                return await MySpells.ShadowFlare.Cast();
            }
            return false;
        }

        private async Task<bool> Enkindle()
        {
            if (PetExists)
            {
                return await MySpells.Enkindle.Cast();
            }
            return false;
        }

        private async Task<bool> TriDisaster()
        {
            if (!Core.Player.HasAura(BioDebuff, true, 5000) || !Core.Player.HasAura(MiasmaDebuff, true, 6000))
            {
                return await MySpells.TriDisaster.Cast(null, false);
            }
            return false;
        }

        private async Task<bool> Deathflare()
        {
            if (!Core.Player.HasAura(MySpells.DreadwyrmTrance.Name, true, 3000))
            {
                return await MySpells.Deathflare.Cast(null, false);
            }
            return false;
        }

        #endregion

        #region Buff

        private async Task<bool> Aetherflow()
        {
            if (AetherCount == 0)
            {
                return await MySpells.Aetherflow.Cast(null, false);
            }
            return false;
        }

        private async Task<bool> Rouse()
        {
            if (PetExists)
            {
                return await MySpells.Rouse.Cast();
            }
            return false;
        }

        private async Task<bool> DreadwyrmTrance()
        {
            return await MySpells.DreadwyrmTrance.Cast(null, false);
        }

        #endregion

        #region Heal

        private async Task<bool> Physick()
        {
            if (Shinra.Settings.SummonerPhysick && Core.Player.CurrentHealthPercent < Shinra.Settings.SummonerPhysickPct)
            {
                var target = Core.Player;

                if (target != null)
                {
                    return await MySpells.Physick.Cast(target);
                }
            }
            return false;
        }

        #endregion

        #region Pet

        private async Task<bool> Summon()
        {
            if (Shinra.Settings.SummonerPet == SummonerPets.None ||
                Shinra.Settings.SummonerPet == SummonerPets.Titan && ActionManager.HasSpell(MySpells.SummonII.Name) ||
                Shinra.Settings.SummonerPet == SummonerPets.Ifrit && ActionManager.HasSpell(MySpells.SummonIII.Name))
            {
                return false;
            }

            if (PetManager.ActivePetType != PetType.Emerald_Carbuncle && PetManager.ActivePetType != PetType.Garuda_Egi)
            {
                if (Shinra.Settings.SummonerSwiftcast && ActionManager.CanCast(MySpells.Summon.Name, Core.Player))
                {
                    if (await MySpells.Role.Swiftcast.Cast(null, false))
                    {
                        await Coroutine.Wait(3000, () => Core.Player.HasAura(MySpells.Role.Swiftcast.Name));
                    }
                }
                return await MySpells.Summon.Cast();
            }
            return false;
        }

        private async Task<bool> SummonII()
        {
            if (Shinra.Settings.SummonerPet == SummonerPets.Titan && PetManager.ActivePetType != PetType.Topaz_Carbuncle &&
                PetManager.ActivePetType != PetType.Titan_Egi)
            {
                if (Shinra.Settings.SummonerSwiftcast && ActionManager.CanCast(MySpells.SummonII.Name, Core.Player))
                {
                    if (await MySpells.Role.Swiftcast.Cast(null, false))
                    {
                        await Coroutine.Wait(3000, () => ActionManager.CanCast(MySpells.SummonII.Name, Core.Player));
                    }
                }
                return await MySpells.SummonII.Cast();
            }
            return false;
        }

        private async Task<bool> SummonIII()
        {
            if (Shinra.Settings.SummonerPet == SummonerPets.Ifrit && PetManager.ActivePetType != PetType.Ifrit_Egi)
            {
                if (Shinra.Settings.SummonerSwiftcast && ActionManager.CanCast(MySpells.SummonIII.Name, Core.Player))
                {
                    if (await MySpells.Role.Swiftcast.Cast(null, false))
                    {
                        await Coroutine.Wait(3000, () => ActionManager.CanCast(MySpells.SummonIII.Name, Core.Player));
                    }
                }
                return await MySpells.SummonIII.Cast();
            }
            return false;
        }

        #endregion

        #region Role

        private async Task<bool> Drain()
        {
            if (Shinra.Settings.SummonerDrain && Core.Player.CurrentHealthPercent < Shinra.Settings.SummonerDrainPct)
            {
                return await MySpells.Role.Drain.Cast();
            }
            return false;
        }

        private async Task<bool> LucidDreaming()
        {
            if (Shinra.Settings.SummonerLucidDreaming && Core.Player.CurrentManaPercent < Shinra.Settings.SummonerLucidDreamingPct)
            {
                return await MySpells.Role.LucidDreaming.Cast();
            }
            return false;
        }

        #endregion

        #region Custom

        private static string BioDebuff => Core.Player.ClassLevel >= 66 ? "Bio III" : Core.Player.ClassLevel >= 26 ? "Bio II" : "Bio";
        private static string MiasmaDebuff => Core.Player.ClassLevel >= 66 ? "Miasma III" : "Miasma";
        private static int AetherCount => ActionResourceManager.Arcanist.Aetherflow;
        private static bool PetExists => Core.Player.Pet != null;
        private static bool AetherLow => AetherCount == 1 && DataManager.GetSpellData(166).Cooldown.TotalMilliseconds > 8000;
        private static bool RecentDoT { get { return Spell.RecentSpell.Keys.Any(key => key.Contains("Tri-disaster")); } }

        private bool UseBane => Shinra.Settings.RotationMode != Modes.Single && Helpers.EnemiesNearTarget(5) > 1 &&
                                ActionManager.CanCast(MySpells.Bane.Name, Core.Player.CurrentTarget) &&
                                Core.Player.CurrentTarget.HasAura(BioDebuff, true, 20000) &&
                                Core.Player.CurrentTarget.HasAura(MiasmaDebuff, true, 14000);

        private bool UseFester => (Shinra.Settings.RotationMode == Modes.Single || Helpers.EnemiesNearTarget(5) <= 1) &&
                                  ActionManager.CanCast(MySpells.Fester.Name, Core.Player.CurrentTarget) && !AetherLow &&
                                  Core.Player.CurrentTarget.HasAura(BioDebuff, true) &&
                                  Core.Player.CurrentTarget.HasAura(MiasmaDebuff, true);

        private bool UsePainflare => Shinra.Settings.RotationMode != Modes.Single && Helpers.EnemiesNearTarget(5) > 1 &&
                                     ActionManager.CanCast(MySpells.Painflare.Name, Core.Player.CurrentTarget) && !AetherLow;

        private bool UsePet => PetExists && (ActionManager.CanCast(MySpells.Rouse.Name, Core.Player) ||
                                             ActionManager.CanCast(MySpells.Enkindle.Name, Core.Player.CurrentTarget));

        #endregion
    }
}