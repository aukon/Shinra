using System.Threading.Tasks;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using ShinraCo.Settings;
using ShinraCo.Spells.Main;

namespace ShinraCo.Rotations
{
    public sealed partial class BasicClass
    {
        private BasicClassSpells MySpells { get; } = new BasicClassSpells();

        #region Arcanist

        private async Task<bool> Ruin()
        {
            return await MySpells.Arcanist.Ruin.Cast();
        }

        private async Task<bool> Bio()
        {
            if (!ActionManager.HasSpell(MySpells.Arcanist.BioII.Name) &&
                !Core.Player.CurrentTarget.HasAura(MySpells.Arcanist.Bio.Name, true, 3000))
            {
                return await MySpells.Arcanist.Bio.Cast();
            }
            return false;
        }

        private async Task<bool> Summon()
        {
            if (Shinra.Settings.SummonerPet == SummonerPets.None || Shinra.Settings.SummonerPet == SummonerPets.Titan &&
                ActionManager.HasSpell(MySpells.Arcanist.SummonII.Name))
            {
                return false;
            }

            if (PetManager.ActivePetType != PetType.Emerald_Carbuncle)
            {
                if (Shinra.Settings.SummonerSwiftcast && ActionManager.CanCast(MySpells.Arcanist.Summon.Name, Core.Player))
                {
                    if (await MySpells.Arcanist.Role.Swiftcast.Cast(null, false))
                    {
                        await Coroutine.Wait(3000, () => Core.Player.HasAura(MySpells.Arcanist.Role.Swiftcast.Name));
                    }
                }
                return await MySpells.Arcanist.Summon.Cast();
            }
            return false;
        }

        private async Task<bool> Physick()
        {
            if (Shinra.Settings.SummonerPhysick)
            {
                if (Core.Player.CurrentHealthPercent < Shinra.Settings.SummonerPhysickPct)
                {
                    return await MySpells.Arcanist.Physick.Cast();
                }
                if (PetExists && Core.Player.Pet.CurrentHealthPercent < Shinra.Settings.SummonerPhysickPct)
                {
                    return await MySpells.Arcanist.Physick.Cast(Core.Player.Pet);
                }
            }
            return false;
        }

        private async Task<bool> Aetherflow()
        {
            return await MySpells.Arcanist.Aetherflow.Cast();
        }

        private async Task<bool> EnergyDrain()
        {
            return await MySpells.Arcanist.EnergyDrain.Cast();
        }

        private async Task<bool> Miasma()
        {
            if (!Core.Player.CurrentTarget.HasAura(MySpells.Arcanist.Miasma.Name, true, 4000))
            {
                return await MySpells.Arcanist.Miasma.Cast();
            }
            return false;
        }

        private async Task<bool> SummonII()
        {
            if (Shinra.Settings.SummonerPet == SummonerPets.Titan && PetManager.ActivePetType != PetType.Topaz_Carbuncle)
            {
                if (Shinra.Settings.SummonerSwiftcast && ActionManager.CanCast(MySpells.Arcanist.SummonII.Name, Core.Player))
                {
                    if (await MySpells.Arcanist.Role.Swiftcast.Cast(null, false))
                    {
                        await Coroutine.Wait(3000, () => ActionManager.CanCast(MySpells.Arcanist.SummonII.Name, Core.Player));
                    }
                }
                return await MySpells.Arcanist.SummonII.Cast();
            }
            return false;
        }

        private async Task<bool> BioII()
        {
            if (!Core.Player.CurrentTarget.HasAura(MySpells.Arcanist.BioII.Name, true, 3000))
            {
                return await MySpells.Arcanist.BioII.Cast();
            }
            return false;
        }

        #endregion

        #region Archer

        private async Task<bool> HeavyShot()
        {
            return await MySpells.Archer.HeavyShot.Cast();
        }

        private async Task<bool> StraightShot()
        {
            if (!Core.Player.HasAura(MySpells.Archer.StraightShot.Name, true, 4000) || Core.Player.HasAura("Straighter Shot"))
            {
                return await MySpells.Archer.StraightShot.Cast();
            }
            return false;
        }

        private async Task<bool> RagingStrikes()
        {
            if (Shinra.Settings.BardRagingStrikes)
            {
                return await MySpells.Archer.RagingStrikes.Cast();
            }
            return false;
        }

        private async Task<bool> VenomousBite()
        {
            if (!Core.Player.CurrentTarget.HasAura(MySpells.Archer.VenomousBite.Name, true, 4000))
            {
                return await MySpells.Archer.VenomousBite.Cast();
            }
            return false;
        }

        private async Task<bool> MiserysEnd()
        {
            return await MySpells.Archer.MiserysEnd.Cast();
        }

        private async Task<bool> Bloodletter()
        {
            return await MySpells.Archer.Bloodletter.Cast();
        }

        #endregion

        #region Gladiator

        private async Task<bool> FastBlade()
        {
            return await MySpells.Gladiator.FastBlade.Cast();
        }

        private async Task<bool> FightOrFlight()
        {
            if (Shinra.Settings.PaladinFightOrFlight)
            {
                if (Core.Player.TargetDistance(5, false))
                {
                    return await MySpells.Gladiator.FightOrFlight.Cast();
                }
            }
            return false;
        }

        private async Task<bool> SavageBlade()
        {
            if (ActionManager.LastSpell.Name == MySpells.Gladiator.FastBlade.Name)
            {
                return await MySpells.Gladiator.SavageBlade.Cast();
            }
            return false;
        }

        private async Task<bool> Flash()
        {
            if (Shinra.Settings.RotationMode != Modes.Single && Shinra.Settings.PaladinFlash && Core.Player.CurrentManaPercent > 40)
            {
                return await MySpells.Gladiator.Flash.Cast();
            }
            return false;
        }

        private async Task<bool> RiotBlade()
        {
            if (ActionManager.LastSpell.Name == MySpells.Gladiator.FastBlade.Name && Core.Player.CurrentManaPercent < 40)
            {
                return await MySpells.Gladiator.RiotBlade.Cast();
            }
            return false;
        }

        private async Task<bool> RageOfHalone()
        {
            if (ActionManager.LastSpell.Name == MySpells.Gladiator.SavageBlade.Name)
            {
                return await MySpells.Gladiator.RageOfHalone.Cast();
            }
            return false;
        }

        #endregion

        #region Lancer

        private async Task<bool> TrueThrust()
        {
            return await MySpells.Lancer.TrueThrust.Cast();
        }

        private async Task<bool> VorpalThrust()
        {
            if (ActionManager.LastSpell.Name == MySpells.Lancer.TrueThrust.Name)
            {
                return await MySpells.Lancer.VorpalThrust.Cast();
            }
            return false;
        }

        private async Task<bool> HeavyThrust()
        {
            if (ActionManager.LastSpell.Name != MySpells.Lancer.TrueThrust.Name &&
                !Core.Player.HasAura(MySpells.Lancer.HeavyThrust.Name, true, 5000))
            {
                return await MySpells.Lancer.HeavyThrust.Cast();
            }
            return false;
        }

        private async Task<bool> LifeSurge()
        {
            if (ActionManager.LastSpell.Name == MySpells.Lancer.VorpalThrust.Name)
            {
                return await MySpells.Lancer.LifeSurge.Cast();
            }
            return false;
        }

        private async Task<bool> FullThrust()
        {
            if (ActionManager.LastSpell.Name == MySpells.Lancer.VorpalThrust.Name)
            {
                return await MySpells.Lancer.FullThrust.Cast();
            }
            return false;
        }

        private async Task<bool> BloodForBlood()
        {
            return await MySpells.Lancer.BloodForBlood.Cast();
        }

        #endregion

        #region Marauder

        private async Task<bool> HeavySwing()
        {
            return await MySpells.Marauder.HeavySwing.Cast();
        }

        private async Task<bool> SkullSunder()
        {
            if (ActionManager.LastSpell.Name == MySpells.Marauder.HeavySwing.Name)
            {
                return await MySpells.Marauder.SkullSunder.Cast();
            }
            return false;
        }

        private async Task<bool> Berserk()
        {
            return await MySpells.Marauder.Berserk.Cast();
        }

        private async Task<bool> Overpower()
        {
            if (Shinra.Settings.RotationMode != Modes.Single && Core.Player.CurrentTPPercent > 30)
            {
                return await MySpells.Marauder.Overpower.Cast();
            }
            return false;
        }

        private async Task<bool> Maim()
        {
            if (ActionManager.LastSpell.Name == MySpells.Marauder.HeavySwing.Name && !Core.Player.CurrentTarget.HasAura(819))
            {
                return await MySpells.Marauder.Maim.Cast();
            }
            return false;
        }

        private async Task<bool> ButchersBlock()
        {
            if (ActionManager.LastSpell.Name == MySpells.Marauder.SkullSunder.Name)
            {
                return await MySpells.Marauder.ButchersBlock.Cast();
            }
            return false;
        }

        private async Task<bool> StormsPath()
        {
            if (ActionManager.LastSpell.Name == MySpells.Marauder.Maim.Name)
            {
                return await MySpells.Marauder.StormsPath.Cast();
            }
            return false;
        }

        #endregion

        #region Role

        private async Task<bool> SecondWind()
        {
            if (Core.Player.CurrentHealthPercent < 50)
            {
                return await MySpells.Lancer.Role.SecondWind.Cast();
            }
            return false;
        }

        #endregion

        #region Custom

        private static bool PetExists => Core.Player.Pet != null;

        #endregion
    }
}