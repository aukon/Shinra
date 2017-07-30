using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ShinraCo.Spells.Main;
using Resource = ff14bot.Managers.ActionResourceManager.WhiteMage;

namespace ShinraCo.Rotations
{
    public sealed partial class WhiteMage
    {
        private WhiteMageSpells MySpells { get; } = new WhiteMageSpells();

        #region Damage

        private async Task<bool> Stone()
        {
            if (!ActionManager.HasSpell(MySpells.StoneII.Name))
            {
                return await MySpells.Stone.Cast();
            }
            return false;
        }

        private async Task<bool> StoneII()
        {
            if (!ActionManager.HasSpell(MySpells.StoneIII.Name))
            {
                return await MySpells.StoneII.Cast();
            }
            return false;
        }

        private async Task<bool> StoneIII()
        {
            if (!ActionManager.HasSpell(MySpells.StoneIV.Name))
            {
                return await MySpells.StoneIII.Cast();
            }
            return false;
        }

        private async Task<bool> StoneIV()
        {
            return await MySpells.StoneIV.Cast();
        }

        #endregion

        #region DoT

        private async Task<bool> Aero()
        {
            if (!ActionManager.HasSpell(MySpells.AeroII.Name) && !Core.Player.CurrentTarget.HasAura(MySpells.Aero.Name, true, 3000))
            {
                return await MySpells.Aero.Cast();
            }
            return false;
        }

        private async Task<bool> AeroII()
        {
            if (!Core.Player.CurrentTarget.HasAura(MySpells.AeroII.Name, true, 3000))
            {
                return await MySpells.AeroII.Cast();
            }
            return false;
        }

        private async Task<bool> AeroIII()
        {
            if (!Core.Player.CurrentTarget.HasAura(MySpells.AeroIII.Name, true, 4000))
            {
                return await MySpells.AeroIII.Cast();
            }
            return false;
        }

        #endregion

        #region AoE

        private async Task<bool> Holy()
        {
            if (Core.Player.CurrentManaPercent > 40 || Core.Player.HasAura(MySpells.ThinAir.Name))
            {
                if (Shinra.Settings.WhiteMageThinAir && ActionManager.CanCast(MySpells.Holy.Name, Core.Player))
                {
                    if (await MySpells.ThinAir.Cast(null, false))
                    {
                        await Coroutine.Wait(3000, () => Core.Player.HasAura(MySpells.ThinAir.Name));
                    }
                }
                return await MySpells.Holy.Cast();
            }
            return false;
        }

        #endregion

        #region Buff

        private async Task<bool> PresenceOfMind()
        {
            if (Shinra.Settings.WhiteMagePresenceOfMind && Core.Player.CurrentManaPercent > 20)
            {
                return await MySpells.PresenceOfMind.Cast(null, false);
            }
            return false;
        }

        #endregion

        #region Heal

        private async Task<bool> Cure()
        {
            if (Shinra.Settings.WhiteMageCure)
            {
                var target = Shinra.Settings.WhiteMagePartyHeal
                    ? Helpers.HealManager.FirstOrDefault(hm => hm.CurrentHealthPercent < Shinra.Settings.WhiteMageCurePct)
                    : Core.Player.CurrentHealthPercent < Shinra.Settings.WhiteMageCurePct ? Core.Player : null;

                if (target != null)
                {
                    if (Shinra.Settings.WhiteMageCureII && Core.Player.HasAura(155))
                    {
                        return await MySpells.CureII.Cast(target);
                    }
                    return await MySpells.Cure.Cast(target);
                }
            }
            return false;
        }

        private async Task<bool> CureII()
        {
            if (Shinra.Settings.WhiteMageCureII)
            {
                var target = Shinra.Settings.WhiteMagePartyHeal
                    ? Helpers.HealManager.FirstOrDefault(hm => hm.CurrentHealthPercent < Shinra.Settings.WhiteMageCureIIPct)
                    : Core.Player.CurrentHealthPercent < Shinra.Settings.WhiteMageCureIIPct ? Core.Player : null;

                if (target != null)
                {
                    return await MySpells.CureII.Cast(target);
                }
            }
            return false;
        }

        private async Task<bool> Tetragrammaton()
        {
            if (Shinra.Settings.WhiteMageTetragrammaton)
            {
                var target = Shinra.Settings.WhiteMagePartyHeal
                    ? Helpers.HealManager.FirstOrDefault(hm => hm.CurrentHealthPercent < Shinra.Settings.WhiteMageTetragrammatonPct)
                    : Core.Player.CurrentHealthPercent < Shinra.Settings.WhiteMageTetragrammatonPct ? Core.Player : null;

                if (target != null)
                {
                    return await MySpells.Tetragrammaton.Cast(target, false);
                }
            }
            return false;
        }

        private async Task<bool> Benediction()
        {
            if (Shinra.Settings.WhiteMageBenediction)
            {
                var target = Shinra.Settings.WhiteMagePartyHeal
                    ? Helpers.HealManager.FirstOrDefault(hm => hm.CurrentHealthPercent < Shinra.Settings.WhiteMageBenedictionPct)
                    : Core.Player.CurrentHealthPercent < Shinra.Settings.WhiteMageBenedictionPct ? Core.Player : null;

                if (target != null)
                {
                    return await MySpells.Benediction.Cast(target, false);
                }
            }
            return false;
        }

        private async Task<bool> Regen()
        {
            if (Shinra.Settings.WhiteMageRegen)
            {
                var target = Shinra.Settings.WhiteMagePartyHeal
                    ? Helpers.HealManager.FirstOrDefault(hm => hm.CurrentHealthPercent < Shinra.Settings.WhiteMageRegenPct &&
                                                               !hm.HasAura(MySpells.Regen.Name))
                    : Core.Player.CurrentHealthPercent < Shinra.Settings.WhiteMageRegenPct && !Core.Player.HasAura(MySpells.Regen.Name)
                        ? Core.Player : null;

                if (target != null)
                {
                    return await MySpells.Regen.Cast(target);
                }
            }
            return false;
        }

        private async Task<bool> Medica()
        {
            if (Shinra.Settings.WhiteMageMedica && Shinra.Settings.WhiteMagePartyHeal && Shinra.LastSpell.Name != MySpells.MedicaII.Name)
            {
                var count = Helpers.HealManager.Count(hm => hm.Distance2D(Core.Player) - hm.CombatReach - Core.Player.CombatReach < 15 &&
                                                            hm.CurrentHealthPercent < Shinra.Settings.WhiteMageMedicaPct);

                if (count > 2)
                {
                    return await MySpells.Medica.Cast();
                }
            }
            return false;
        }

        private async Task<bool> MedicaII()
        {
            if (Shinra.Settings.WhiteMageMedicaII && Shinra.Settings.WhiteMagePartyHeal && Shinra.LastSpell.Name != MySpells.Medica.Name &&
                !Core.Player.HasAura(MySpells.MedicaII.Name, true))
            {
                var count = Helpers.HealManager.Count(hm => hm.Distance2D(Core.Player) - hm.CombatReach - Core.Player.CombatReach < 15 &&
                                                            hm.CurrentHealthPercent < Shinra.Settings.WhiteMageMedicaIIPct);

                if (count > 2)
                {
                    return await MySpells.MedicaII.Cast();
                }
            }
            return false;
        }

        private async Task<bool> Assize()
        {
            if (Shinra.Settings.WhiteMageAssize && Shinra.Settings.WhiteMagePartyHeal && Core.Player.CurrentManaPercent < 85)
            {
                var count = Helpers.HealManager.Count(hm => hm.Distance2D(Core.Player) - hm.CombatReach - Core.Player.CombatReach < 15 &&
                                                            hm.CurrentHealthPercent < Shinra.Settings.WhiteMageAssizePct);

                if (count > 2)
                {
                    return await MySpells.Assize.Cast(null, false);
                }
            }
            return false;
        }

        private async Task<bool> PlenaryIndulgence()
        {
            if (Shinra.Settings.WhiteMagePlenary && Shinra.Settings.WhiteMagePartyHeal)
            {
                var count = Helpers.HealManager.Count(hm => hm.Distance2D(Core.Player) - hm.CombatReach - Core.Player.CombatReach < 15 &&
                                                            hm.CurrentHealthPercent < Shinra.Settings.WhiteMagePlenaryPct);

                if (count > 2)
                {
                    return await MySpells.PlenaryIndulgence.Cast(null, false);
                }
            }
            return false;
        }

        private async Task<bool> Raise()
        {
            if (Shinra.Settings.WhiteMageRaise &&
                (Shinra.Settings.WhiteMageSwiftcast && ActionManager.CanCast(MySpells.Role.Swiftcast.Name, Core.Player) ||
                 !Helpers.HealManager.Any(hm => hm.CurrentHealthPercent < Shinra.Settings.WhiteMageCurePct)))
            {
                var target = Helpers.HealManager.FirstOrDefault(hm => hm.IsDead && hm.Type == GameObjectType.Pc &&
                                                                      !hm.HasAura(MySpells.Raise.Name));

                if (target != null)
                {
                    if (Shinra.Settings.WhiteMageSwiftcast && ActionManager.CanCast(MySpells.Raise.Name, target))
                    {
                        if (await MySpells.Role.Swiftcast.Cast(null, false))
                        {
                            await Coroutine.Wait(3000, () => Core.Player.HasAura(MySpells.Role.Swiftcast.Name));
                        }
                    }
                    return await MySpells.Raise.Cast(target);
                }
            }
            return false;
        }

        #endregion

        #region Role

        private async Task<bool> ClericStance()
        {
            if (Shinra.Settings.WhiteMageClericStance)
            {
                return await MySpells.Role.ClericStance.Cast();
            }
            return false;
        }

        private async Task<bool> Protect()
        {
            if (Shinra.Settings.WhiteMageProtect)
            {
                var target = Shinra.Settings.WhiteMagePartyHeal
                    ? Helpers.HealManager.FirstOrDefault(hm => !hm.HasAura(MySpells.Role.Protect.Name) && hm != ChocoboManager.Object)
                    : !Core.Player.HasAura(MySpells.Role.Protect.Name) ? Core.Player : null;

                if (target != null)
                {
                    if (Shinra.Settings.WhiteMageSwiftcast && ActionManager.CanCast(MySpells.Role.Protect.Name, target))
                    {
                        if (await MySpells.Role.Swiftcast.Cast(null, false))
                        {
                            await Coroutine.Wait(3000, () => Core.Player.HasAura(MySpells.Role.Swiftcast.Name));
                        }
                    }
                    return await MySpells.Role.Protect.Cast(target);
                }
            }
            return false;
        }

        private async Task<bool> Esuna()
        {
            if (Shinra.Settings.WhiteMageEsuna)
            {
                var target = Helpers.HealManager.FirstOrDefault(hm => hm.HasDispellable());

                if (target != null)
                {
                    return await MySpells.Role.Esuna.Cast(target);
                }
            }
            return false;
        }

        private async Task<bool> LucidDreaming()
        {
            if (Shinra.Settings.WhiteMageLucidDreaming && Core.Player.CurrentManaPercent < Shinra.Settings.WhiteMageLucidDreamingPct)
            {
                return await MySpells.Role.LucidDreaming.Cast();
            }
            return false;
        }

        #endregion

        #region Custom

        private static int LilyCount => Resource.Lily;

        #endregion
    }
}