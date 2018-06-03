using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;
using ShinraCo.Settings;
using ShinraCo.Spells.Main;
using Resource = ff14bot.Managers.ActionResourceManager.Astrologian;

namespace ShinraCo.Rotations
{
    public sealed partial class Astrologian
    {
        private AstrologianSpells MySpells { get; } = new AstrologianSpells();

        #region Damage

        private async Task<bool> Malefic()
        {
            if (!ActionManager.HasSpell(MySpells.MaleficII.Name) && !StopDamage)
            {
                return await MySpells.Malefic.Cast();
            }
            return false;
        }

        private async Task<bool> MaleficII()
        {
            if (!ActionManager.HasSpell(MySpells.MaleficIII.Name) && !StopDamage)
            {
                return await MySpells.MaleficII.Cast();
            }
            return false;
        }

        private async Task<bool> MaleficIII()
        {
            if (!StopDamage)
            {
                return await MySpells.MaleficIII.Cast();
            }
            return false;
        }

        #endregion

        #region DoT

        private async Task<bool> Combust()
        {
            if (!ActionManager.HasSpell(MySpells.CombustII.Name) && !StopDots &&
                !Core.Player.CurrentTarget.HasAura(MySpells.Combust.Name, true, 4000))
            {
                return await MySpells.Combust.Cast();
            }
            return false;
        }

        private async Task<bool> CombustII()
        {
            if (!StopDots && !Core.Player.CurrentTarget.HasAura(MySpells.CombustII.Name, true, 4000))
            {
                return await MySpells.CombustII.Cast();
            }
            return false;
        }

        #endregion

        #region AoE

        private async Task<bool> Gravity()
        {
            if (!StopDamage)
            {
                if (!Core.Player.HasAura(MySpells.Lightspeed.Name))
                {
                    return await MySpells.Gravity.Cast();
                }
            }
            return false;
        }

        private async Task<bool> SpeedGravity()
        {
            if (!StopDamage)
            {
                if (Core.Player.HasAura(MySpells.Lightspeed.Name))
                {
                    if (ActionManager.CanCast(MySpells.Gravity.Name, Core.Player.CurrentTarget))
                    {
                        return await MySpells.Gravity.Cast(null, false);
                    }
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> EarthlyStarMulti()
        {
            if (Shinra.Settings.AstrologianEarthlyStar && !MovementManager.IsMoving )
            {
                var count = Shinra.Settings.CustomAoE ? Shinra.Settings.CustomAoECount : 4;

                if (Helpers.EnemiesNearTarget(8) > count)
                {
                    return await MySpells.EarthlyStar.Cast();
                }
            }
            return false;
        }

        #endregion

        #region Buff

        private async Task<bool> Lightspeed()
        {
            if (UseLightspeed)
            {
                return await MySpells.Lightspeed.Cast(null, false);
            }
            return false;
        }

        private async Task<bool> Synastry()
        {
            if (Shinra.Settings.AstrologianPartyHeal)
            {
                if (Helpers.HealManager.FirstOrDefault(hm => hm.IsTank() && hm.CurrentHealthPercent < 40) != null ||
                    Helpers.HealManager.Count(hm => hm.CurrentHealthPercent < 60) > 2)
                {
                    var target = Helpers.HealManager.FirstOrDefault();

                    if (target != null)
                    {
                        return await MySpells.Synastry.Cast(target, false);
                    }
                }
            }
            return false;
        }

        private async Task<bool> CelestialOpposition()
        {
            if (UseBuffsTimer && !MovementManager.IsMoving)
            {
                return await MySpells.CelestialOpposition.Cast(null, false);
            }
            return false;
        }

        private async Task<bool> TimeDilation()
        {
            var target = Helpers.HealManager.FirstOrDefault(hm => hm.IsDPS() && (hm.HasAura(829) || hm.HasAura(831) || hm.HasAura(832)) ||
                                                                  hm.IsTank() && hm.HasAura(830));

            if (target != null)
            {
                return await MySpells.TimeDilation.Cast(target, false);
            }
            return false;
        }

        #endregion

        #region Heal

        private async Task<bool> UpdateHealing()
        {
            if (Shinra.Settings.AstrologianPartyHeal || Shinra.Settings.AstrologianStyle == AstrologianStyles.Party)
            {
                if (!await Helpers.UpdateHealManager())
                {
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> StopCasting()
        {
            if (Shinra.Settings.AstrologianInterruptOverheal && Core.Player.IsCasting && !Core.Player.HasAura(815))
            {
                var target = GameObjectManager.GetObjectByObjectId(Core.Player.SpellCastInfo.TargetId);
                var spellName = Core.Player.SpellCastInfo.Name;

                if (target != null)
                {
                    if (spellName == MySpells.Benefic.Name && target.CurrentHealthPercent >= Shinra.Settings.AstrologianBeneficPct + 10 ||
                        spellName == MySpells.BeneficII.Name && target.CurrentHealthPercent >= Shinra.Settings.AstrologianBeneficIIPct + 10)
                    {
                        var debugSetting = spellName == MySpells.Benefic.Name ? Shinra.Settings.AstrologianBeneficPct
                            : Shinra.Settings.AstrologianBeneficIIPct;
                        Helpers.Debug($@"Target HP: {target.CurrentHealthPercent}, Setting: {debugSetting}, Adjusted: {debugSetting + 10}");

                        Logging.Write(Colors.Yellow, $@"[Shinra] Interrupting >>> {spellName}");
                        ActionManager.StopCasting();
                        await Coroutine.Wait(500, () => !Core.Player.IsCasting);
                    }
                }
            }
            return false;
        }

        private async Task<bool> Benefic()
        {
            if (Shinra.Settings.AstrologianBenefic && !Core.Player.HasAura(815))
            {
                var target = Shinra.Settings.AstrologianPartyHeal
                    ? Helpers.HealManager.FirstOrDefault(hm => hm.CurrentHealthPercent < Shinra.Settings.AstrologianBeneficPct)
                    : Core.Player.CurrentHealthPercent < Shinra.Settings.AstrologianBeneficPct ? Core.Player : null;

                if (target != null)
                {
                    return await MySpells.Benefic.Cast(target);
                }
            }
            return false;
        }

        private async Task<bool> BeneficII()
        {
            if (Shinra.Settings.AstrologianBeneficII)
            {
                var target = Shinra.Settings.AstrologianPartyHeal
                    ? Helpers.HealManager.FirstOrDefault(hm => hm.CurrentHealthPercent < Shinra.Settings.AstrologianBeneficIIPct)
                    : Core.Player.CurrentHealthPercent < Shinra.Settings.AstrologianBeneficIIPct ? Core.Player : null;

                if (target != null)
                {
                    return await MySpells.BeneficII.Cast(target);
                }
            }

            if (Shinra.Settings.AstrologianBeneficII && Core.Player.HasAura(815))
            {
                var target = Shinra.Settings.AstrologianPartyHeal
                    ? Helpers.HealManager.FirstOrDefault(hm => hm.CurrentHealthPercent < 80)
                    : Core.Player.CurrentHealthPercent < 80 ? Core.Player : null;

                if (target != null)
                {
                    return await MySpells.BeneficII.Cast(target);
                }
            }
            return false;
        }

        private async Task<bool> EssentialDignity()
        {
            if (Shinra.Settings.AstrologianEssDignity && Shinra.Settings.AstrologianPartyHeal)
            {
                var target = Helpers.HealManager.FirstOrDefault(hm => (hm.IsHealer() || hm.IsTank()) && 
                                                                       hm.CurrentHealthPercent <= Shinra.Settings.AstrologianEssDignityPct ||
                                                                       hm.IsDPS() && hm.CurrentHealthPercent < Shinra.Settings.AstrologianEssDignityPct && 
                                                                       hm.IsTank() && hm.CurrentHealthPercent > 70);

                if (target != null)
                {
                    return await MySpells.EssentialDignity.Cast(target, false);
                }
            }
            return false;
        }

        private async Task<bool> AspectedBenefic()
        {
            if (Shinra.Settings.AstrologianAspBenefic && Shinra.Settings.AstrologianPartyHeal)
            {
                if (Core.Player.HasAura(MySpells.DiurnalSect.Name))
                {
                    var target = Shinra.Settings.AstrologianPartyHeal
                        ? Helpers.HealManager.FirstOrDefault(hm => !hm.HasAura(835, true) && hm.IsTank() && hm.CurrentHealthPercent < Shinra.Settings.AstrologianAspBeneficPct ||
                                                                   !hm.HasAura(835, true) && hm.IsHealer() && hm.CurrentHealthPercent < Shinra.Settings.AstrologianAspBeneficPct ||
                                                                   !hm.HasAura(835, true) && hm.IsDPS() && hm.CurrentHealthPercent < 50)
                        : Core.Player.CurrentHealthPercent < Shinra.Settings.AstrologianAspBeneficPct &&
                          !Core.Me.HasAura(835, true) ? Core.Player : null;

                    if (target != null)
                    {
                        return await MySpells.AspectedBenefic.Cast(target, false);
                    }
                }

                if (Core.Player.HasAura(MySpells.NocturnalSect.Name))
                {
                    var target = Shinra.Settings.AstrologianPartyHeal
                        ? Helpers.HealManager.FirstOrDefault(hm => !hm.HasAura(837, true) && hm.IsTank() && hm.CurrentHealthPercent < Shinra.Settings.AstrologianAspBeneficPct ||
                                                                   !hm.HasAura(837, true) && hm.IsHealer() && hm.CurrentHealthPercent < Shinra.Settings.AstrologianAspBeneficPct ||
                                                                   !hm.HasAura(837, true) && hm.IsDPS() && hm.CurrentHealthPercent < 50)
                        : Core.Player.CurrentHealthPercent < Shinra.Settings.AstrologianAspBeneficPct &&
                          !Core.Me.HasAura(837, true) ? Core.Player : null;

                    if (target != null)
                    {
                        return await MySpells.AspectedBenefic.Cast(target, false);
                    }
                }
            }
            return false;
        }

        private async Task<bool> Helios()
        {
            if (Shinra.Settings.AstrologianHelios && Shinra.Settings.AstrologianPartyHeal)
            {
                if (UseAoEHeals)
                {
                    var count = Helpers.FriendsNearPlayer(Shinra.Settings.AstrologianHeliosPct);

                    if (count > 2)
                    {
                        if (await MySpells.Helios.Cast())
                        {
                            await Coroutine.Wait(3000, () => ActionManager.CanCast(MySpells.Helios.Name, Core.Player));
                        }
                    }
                }
            }
            return false;
        }

        private async Task<bool> AspectedHelios()
        {
            if (Shinra.Settings.AstrologianAspHelios && Shinra.Settings.AstrologianPartyHeal)
            {
                if (UseAoEHeals)
                {
                    if (Core.Player.HasAura(MySpells.DiurnalSect.Name) && !Core.Me.HasAura(836, true))
                    {
                        var count = Helpers.FriendsNearPlayer(Shinra.Settings.AstrologianAspHeliosPct);

                        if (count > 2)
                        {
                            if (await MySpells.AspectedHelios.Cast())
                            {
                                await Coroutine.Wait(3000, () => ActionManager.CanCast(MySpells.AspectedHelios.Name, Core.Player));
                            }
                        }
                    }

                    if (Core.Player.HasAura(MySpells.NocturnalSect.Name) && !Core.Me.HasAura(837, true))
                    {
                        var count = Helpers.FriendsNearPlayer(Shinra.Settings.AstrologianAspHeliosPct);

                        if (count > 2)
                        {
                            if (await MySpells.AspectedHelios.Cast())
                            {
                                await Coroutine.Wait(3000, () => ActionManager.CanCast(MySpells.AspectedHelios.Name, Core.Player));
                            }
                        }
                    }
                }
            }
            return false;
        }

        private async Task<bool> Ascend()
        {
            if (Shinra.Settings.AstrologianAscend)
            {
                var target = Helpers.RessManager.FirstOrDefault(pm => !pm.HasAura("Raise", true) && 
                            !Helpers.HealManager.Any(hm => hm.CurrentHealthPercent < Shinra.Settings.AstrologianBeneficPct));

                if (target != null)
                {
                    if (Core.Player.InCombat)
                    {
                        if (Shinra.Settings.AstrologianSwiftcast && ActionManager.CanCast(MySpells.Role.Swiftcast.Name, Core.Player))
                        {
                            if (await MySpells.Role.Swiftcast.Cast(null, false))
                            {
                                await Coroutine.Wait(3000, () => ActionManager.CanCast(MySpells.Ascend.Name, target) &&
                                                                 Core.Player.HasAura(MySpells.Role.Swiftcast.Name, true));
                            }
                            return await MySpells.Ascend.Cast(target);
                        }
                    }

                    if (!Core.Player.InCombat)
                    {
                        if (Shinra.Settings.AstrologianSwiftcast && ActionManager.CanCast(MySpells.Ascend.Name, target))
                        {
                            if (await MySpells.Role.Swiftcast.Cast(null, false))
                            {
                                await Coroutine.Wait(3000, () => ActionManager.CanCast(MySpells.Ascend.Name, target) &&
                                                                 Core.Player.HasAura(MySpells.Role.Swiftcast.Name, true));
                            }
                        }
                        return await MySpells.Ascend.Cast(target);
                    }

                }
            }
            return false;
        }

        private async Task<bool> EarthlyStar()
        {
            if (Shinra.Settings.AstrologianEarthlyStar && Core.Me.HasTarget && !MovementManager.IsMoving)
            {
                var target = Helpers.HealManager.FirstOrDefault(hm => hm.IsTank() && hm.CurrentHealthPercent < 60);

                if (target != null)
                {
                    return await MySpells.EarthlyStar.Cast(target, false);
                }
            }
            return false;
        }

        private async Task<bool> StellarDetonation()
        {
            if (Shinra.Settings.AstrologianEarthlyStar)
            {
                if (UseStellar)
                {
                    return await MySpells.StellarDetonation.Cast(null, false);
                }
            }
            return false;
        }

        private async Task<bool> CollectiveUnconscious()
        {
            if (Core.Player.HasAura(MySpells.CollectiveUnconscious.Name))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Card

        private async Task<bool> RoyalRoad()
        {
            if (Shinra.Settings.AstrologianDraw)
            {
                if (Shinra.Settings.AstrologianStyle == AstrologianStyles.Party)
                {
                    if (!BuffShared && (CardEwer || CardSpire))
                    {
                        return await MySpells.RoyalRoad.Cast();
                    }
                }
                else
                {
                    if (!BuffPotency && CardBole)
                    {
                        return await MySpells.RoyalRoad.Cast();
                    }
                }
            }
            return false;
        }

        private async Task<bool> Redraw()
        {
            if (Shinra.Settings.AstrologianDraw)
            {
                if (HeldOffensive && !BuffShared && !CardBalance || CardSupport)
                {
                    return await MySpells.Redraw.Cast();
                }
            }
            return false;
        }

        private async Task<bool> Spread()
        {
            if (Shinra.Settings.AstrologianDraw)
            {
                if (!BalanceHeld && CardBalance)
                {
                    return await MySpells.Spread.Cast();
                }

                if (AuraOffensive && UseDraw)
                {
                    if (ArrowHeld || SpearHeld)
                    {
                        var target = Helpers.HealManager.FirstOrDefault(hm => hm.IsDPS());

                        if (target != null)
                        {
                            return await MySpells.Spread.Cast(target);
                        }
                    }
                    else if (BuffShared && BalanceHeld)
                    {
                        if (Helpers.HealManager.Count(hm => hm.Distance2D(Core.Player) - hm.CombatReach -
                                                            Core.Player.CombatReach < 20 && hm.IsDPS()) >= 2)
                        {
                            var target = Helpers.HealManager.FirstOrDefault(hm => hm.IsDPS());

                            if (target != null)
                            {
                                return await MySpells.Spread.Cast(target);
                            }
                        }
                    }
                }
            }
            return false;
        }

        private async Task<bool> MinorArcana()
        {
            if (Shinra.Settings.AstrologianDraw)
            {
                if (Shinra.Settings.AstrologianStyle == AstrologianStyles.Party)
                {
                    if (!HasArcana && BuffShared && CardSupport)
                    {
                        return await MySpells.MinorArcana.Cast();
                    }
                }
                else
                {
                    if (!HasArcana && CardSupport)
                    {
                        return await MySpells.MinorArcana.Cast();
                    }
                }
            }
            return false;
        }

        private async Task<bool> Undraw()
        {
            if (Shinra.Settings.AstrologianDraw)
            {
                if (Shinra.Settings.AstrologianStyle == AstrologianStyles.Party)
                {
                    if (MySpells != null && !ActionManager.CanCast(MySpells.Redraw.Name, Core.Player))
                    {
                        if (!ActionManager.HasSpell(MySpells.MinorArcana.Name) && BuffShared && CardSupport)
                        {
                            return await MySpells.Undraw.Cast();
                        }
                    }
                }
                else
                {
                    if (!ActionManager.HasSpell(MySpells.MinorArcana.Name) && (CardEwer || CardSpire))
                    {
                        return await MySpells.Undraw.Cast();
                    }
                }
            }
            return false;
        }

        private async Task<bool> UndrawSpread()
        {
            if (Shinra.Settings.AstrologianDraw)
            {
                if (HeldSupport && BuffShared)
                {
                    return await MySpells.UndrawSpread.Cast();
                }
            }
            return false;
        }

        private async Task<bool> Draw()
        {
            if (Shinra.Settings.AstrologianDraw)
            {
                if (Shinra.Settings.AstrologianStyle == AstrologianStyles.Party)
                {
                    if (AuraOffensive && UseDraw)
                    {
                        if (BuffShared && (CardBalance || CardArrow || CardSpear))
                        {
                            if (Helpers.HealManager.Count(hm => hm.Distance2D(Core.Player) - hm.CombatReach -
                                                                Core.Player.CombatReach < 20 && hm.IsDPS()) >= 2)
                            {
                                var target = Helpers.HealManager.FirstOrDefault(hm => hm.IsDPS());

                                if (target != null)
                                {
                                    return await MySpells.Draw.Cast(target);
                                }
                            }
                        }
                        else if (!BuffShared && (HasSpread && CardBalance || CardArrow || CardSpear))
                        {
                            var target = Helpers.HealManager.FirstOrDefault(hm => hm.IsDPS());

                            if (target != null)
                            {
                                return await MySpells.Draw.Cast(target);
                            }
                        }
                    }

                    if (!HasCard)
                    {
                        return await MySpells.Draw.Cast();
                    }
                }
                else
                {
                    return await MySpells.Draw.Cast();
                }
            }
            return false;
        }

        private async Task<bool> LordOfCrowns()
        {
            if (Shinra.Settings.AstrologianDraw && CardLord)
            {
                return await MySpells.LordOfCrowns.Cast();
            }
            return false;
        }

        private async Task<bool> LadyOfCrowns()
        {
            if (Shinra.Settings.AstrologianDraw && CardLady)
            {
                var target = Shinra.Settings.AstrologianPartyHeal ? Helpers.HealManager.FirstOrDefault(hm => hm.CurrentHealthPercent < 80)
                    : Core.Player.CurrentHealthPercent < 80 ? Core.Player : null;

                if (target != null)
                {
                    return await MySpells.LadyOfCrowns.Cast(target);
                }
            }
            return false;
        }

        private async Task<bool> SleeveDraw()
        {
            if (Shinra.Settings.AstrologianSleeveDraw)
            {
                if (!HasCard && !HasSpread && UseDraw)
                {
                    return await MySpells.SleeveDraw.Cast();
                }
            }
            return false;
        }

        private async Task<bool> DrawnSupport()
        {
            if (Shinra.Settings.AstrologianDraw)
            {
                if (UseDraw && !BuffShared)
                {
                    if (CardBole)
                    {
                        var target = Helpers.HealManager.FirstOrDefault(hm => hm.IsTank());

                        if (target != null)
                        {
                            return await MySpells.Draw.Cast(target);
                        }
                    }
                    else if (CardEwer && !BalanceHeld)
                    {
                        if (Core.Player.CurrentManaPercent < 40)
                        {
                            return await MySpells.Draw.Cast();
                        }
                    }
                    else if (CardSpire && !BalanceHeld && !ArrowHeld && !SpearHeld)
                    {
                        var target = Helpers.GoadManager.FirstOrDefault(gm => gm.CurrentTP < 400);

                        if (target != null)
                        {
                            return await MySpells.Draw.Cast(target);
                        }
                    }
                }
            }
            return false;
        }

        private async Task<bool> SpreadSupport()
        {
            if (Shinra.Settings.AstrologianDraw)
            {
                if (UseDraw && !BuffShared)
                {
                    if (BoleHeld)
                    {
                        var target = Helpers.HealManager.FirstOrDefault(hm => hm.IsTank());

                        if (target != null)
                        {
                            return await MySpells.Spread.Cast(target);
                        }
                    }
                    else if (EwerHeld)
                    {
                        return await MySpells.Spread.Cast();
                    }
                    else if (SpireHeld)
                    {
                        var target = Helpers.GoadManager.FirstOrDefault();

                        if (target != null)
                        {
                            return await MySpells.Spread.Cast(target);
                        }
                    }
                }
            }
            return false;
        }

        #endregion

        #region Card PreCombat

        private async Task<bool> DrawPreCombat()
        {
            if (Shinra.Settings.AstrologianDraw && !Core.Me.InCombat)
            {
                if (!HasCard && (!BuffShared || !HasSpread || !HasArcana))
                {
                    return await MySpells.Draw.Cast();
                }
            }
            return false;
        }

        private async Task<bool> RoyalRoadPreCombat()
        {
            if (Shinra.Settings.AstrologianDraw && !Core.Me.InCombat)
            {
                if (!BuffShared && (CardEwer || CardSpire))
                {
                    return await MySpells.RoyalRoad.Cast();
                }
            }
            return false;
        }

        private async Task<bool> SpreadPreCombat()
        {
            if (Shinra.Settings.AstrologianDraw && !Core.Me.InCombat)
            {
                if (!HasSpread && CardBalance)
                {
                    return await MySpells.Spread.Cast();
                }
            }
            return false;
        }

        private async Task<bool> RedrawPreCombat()
        {
            if (Shinra.Settings.AstrologianDraw && !Core.Me.InCombat)
            {
                if (!BuffShared && !CardEwer && !CardSpire || !BalanceHeld && !CardBalance)
                {
                    return await MySpells.Redraw.Cast();
                }
            }
            return false;
        }

        private async Task<bool> MinorArcanaCombat()
        {
            if (Shinra.Settings.AstrologianDraw && !Core.Me.InCombat)
            {
                if (!HasArcana && (!CardEwer && !CardBalance && !CardSpire || BuffShared && BalanceHeld))
                {
                    return await MySpells.MinorArcana.Cast();
                }
            }
            return false;
        }

        private async Task<bool> UndrawPreCombat()
        {
            if (Shinra.Settings.AstrologianDraw && !Core.Me.InCombat)
            {
                if (MySpells != null && !ActionManager.CanCast(MySpells.Redraw.Name, Core.Me))
                {
                    if (!BuffShared && !CardEwer && !CardSpire || !BalanceHeld && !CardBalance)
                    {
                        return await MySpells.Undraw.Cast();
                    }
                }
            }
            return false;
        }

        private async Task<bool> UndrawSpreadPreCombat()
        {
            if (Shinra.Settings.AstrologianDraw && !Core.Me.InCombat)
            {
                if (!BalanceHeld && !DutyManager.InInstance || HeldSupport && DutyManager.InInstance)
                {
                    return await MySpells.UndrawSpread.Cast();
                }
            }
            return false;
        }

        #endregion

        #region Sect

        private async Task<bool> DiurnalSect()
        {
            if (Shinra.Settings.AstrologianSect == AstrologianSects.Diurnal ||
                Shinra.Settings.AstrologianSect == AstrologianSects.Nocturnal && !ActionManager.HasSpell(MySpells.NocturnalSect.Name))
            {
                if (!Core.Player.HasAura(MySpells.DiurnalSect.Name))
                {
                    return await MySpells.DiurnalSect.Cast();
                }
            }
            return false;
        }

        private async Task<bool> NocturnalSect()
        {
            if (Shinra.Settings.AstrologianSect == AstrologianSects.Nocturnal && !Core.Player.HasAura(MySpells.NocturnalSect.Name))
            {
                return await MySpells.NocturnalSect.Cast();
            }
            return false;
        }

        #endregion

        #region Role

        private async Task<bool> ClericStance()
        {
            if (Shinra.Settings.AstrologianClericStance)
            {
                return await MySpells.Role.ClericStance.Cast();
            }
            return false;
        }

        private async Task<bool> Protect()
        {
            if (Shinra.Settings.AstrologianProtect)
            {
                if (!Helpers.HealManager.Any(hm => hm.CurrentHealthPercent <= Shinra.Settings.AstrologianBeneficPct || hm.IsDead))
                {
                    var target = Shinra.Settings.AstrologianPartyHeal
                        ? Helpers.HealManager.FirstOrDefault(hm => !hm.HasAura(MySpells.Role.Protect.Name) && hm.Type == GameObjectType.Pc)
                        : !Core.Player.HasAura(MySpells.Role.Protect.Name) ? Core.Player : null;

                    if (target != null)
                    {
                        if (Core.Player.InCombat)
                        {
                            if (Shinra.Settings.AstrologianSwiftcast && ActionManager.CanCast(MySpells.Role.Swiftcast.Name, Core.Player))
                            {
                                if (await MySpells.Role.Swiftcast.Cast(null, false))
                                {
                                    await Coroutine.Wait(3000, () => ActionManager.CanCast(MySpells.Role.Protect.Name, target) &&
                                                                     Core.Player.HasAura(MySpells.Role.Swiftcast.Name, true));
                                }
                                return await MySpells.Role.Protect.Cast(target);
                            }
                        }

                        if (!Core.Player.InCombat)
                        {
                            if (Shinra.Settings.AstrologianSwiftcast && ActionManager.CanCast(MySpells.Role.Protect.Name, target))
                            {
                                if (await MySpells.Role.Swiftcast.Cast(null, false))
                                {
                                    await Coroutine.Wait(3000, () => ActionManager.CanCast(MySpells.Role.Protect.Name, target) &&
                                                                     Core.Player.HasAura(MySpells.Role.Swiftcast.Name, true));
                                }
                            }
                            return await MySpells.Role.Protect.Cast(target);
                        }
                    }
                }
            }
            return false;
        }

        private async Task<bool> Esuna()
        {
            if (Shinra.Settings.AstrologianEsuna)
            {
                var target = Shinra.Settings.AstrologianPartyHeal ? Helpers.HealManager.FirstOrDefault(hm => hm.HasDispellable())
                    : Core.Player.HasDispellable() ? Core.Player : null;

                if (target != null)
                {
                    return await MySpells.Role.Esuna.Cast(target);
                }
            }
            return false;
        }

        private async Task<bool> LucidDreaming()
        {
            if (Shinra.Settings.AstrologianLucidDreaming && Core.Player.CurrentManaPercent < Shinra.Settings.AstrologianLucidDreamingPct)
            {
                return await MySpells.Role.LucidDreaming.Cast();
            }
            return false;
        }

        private async Task<bool> EyeForAnEye()
        {
            if (Shinra.Settings.AstrologianPartyHeal)
            {
                var target = Helpers.HealManager.FirstOrDefault(hm =>
                    hm.IsTank() && hm.CurrentHealthPercent < 60);

                if (target != null)
                {
                    return await MySpells.Role.EyeForAnEye.Cast(target, false);
                }
            }
            return false;
        }

        private async Task<bool> Swiftcast()
        {
            if (Shinra.Settings.AstrologianSwiftcast)
            {
                return await MySpells.Role.Swiftcast.Cast();
            }
            return false;
        }

        private async Task<bool> Largesse()
        {
            if (Shinra.Settings.AstrologianPartyHeal)
            {
                if (Helpers.HealManager.Count(hm => hm.CurrentHealthPercent < 60) > 2)
                {
                    return await MySpells.Role.Largesse.Cast(null, false);
                }
            }
            return false;
        }

        #endregion

        #region Custom

        private static bool StopDamage => Shinra.Settings.AstrologianStopDamage && Core.Player.CurrentManaPercent <= Shinra.Settings.AstrologianStopDamagePct;
        private static bool StopDots => Shinra.Settings.AstrologianStopDots && Core.Player.CurrentManaPercent <= Shinra.Settings.AstrologianStopDotsPct;

        private bool UseAoEHeals => Shinra.LastSpell.Name != MySpells.Helios.Name && Shinra.LastSpell.Name != MySpells.AspectedHelios.Name;

        private bool UseDraw => Shinra.LastSpell.Name != MySpells.Draw.Name && Shinra.LastSpell.Name != MySpells.Spread.Name;

        private static bool AuraOffensive => !Core.Player.HasAura(829, true) && !Core.Player.HasAura(831, true) && !Core.Player.HasAura(832, true);

        private static bool UseStellar => Shinra.Settings.RotationMode == Modes.Multi || Core.Player.HasAura(1248, true) ||
                                          Core.Target.CurrentHealthPercent < 8 && Core.Target.MaxHealth > 95000 ||
                                          Core.Target.CurrentHealthPercent < 30 && Core.Target.MaxHealth < 95000 ||
                                          Helpers.HealManager.FirstOrDefault(hm => hm.IsTank() && hm.CurrentHealthPercent < 60) != null;

        private static bool UseBuffsTimer => Core.Player.HasAura("Lucid Dreaming") || Core.Player.HasAura("The Ewer") && Core.Player.CurrentManaPercent < 70 ||
                                             Core.Player.HasAura("Lightspeed") && Helpers.EnemiesNearTarget(5) > 4 ||
                                             Helpers.HealManager.Count(hm => hm.Distance2D(Core.Player) - hm.CombatReach -
                                             Core.Player.CombatReach <= 10 && (hm.HasAura(829) && !hm.HasAura(829, true, 10000) && Core.Player.InCombat ||
                                            (hm.HasAura("Aspected Helios") || hm.HasAura("Nocturnal Field") || Core.Player.HasAura("Synastry") || 
                                             hm.HasAura("Wheel of Fortune")) && hm.CurrentHealthPercent < 70)) > 2;

        private static bool UseLightspeed => Helpers.HealManager.Count(hm => hm.CurrentHealthPercent < 50) > 2 ||
                                             Helpers.EnemiesNearTarget(5) > 4 && Core.Player.CurrentManaPercent > Shinra.Settings.AstrologianStopDamagePct ||
                                             Helpers.HealManager.FirstOrDefault(hm => hm.IsTank() && hm.CurrentHealthPercent < 30) != null;


        #region Buff Royal Road

        private static bool BuffPotency => Core.Player.HasAura(816, true);
        private static bool BuffShared => Core.Player.HasAura(817, true);

        #endregion

        #region Card Spread

        private static bool HasSpread => Resource.Cards[1] != Resource.AstrologianCard.None;
        private static bool HeldOffensive => Core.Player.HasAura(920) || Core.Player.HasAura(922) || Core.Player.HasAura(923);
        private static bool HeldSupport => Core.Player.HasAura(921) || Core.Player.HasAura(924) || Core.Player.HasAura(925);

        private static bool BalanceHeld => Core.Player.HasAura(920);
        private static bool BoleHeld => Core.Player.HasAura(921);
        private static bool ArrowHeld => Core.Player.HasAura(922);
        private static bool SpearHeld => Core.Player.HasAura(923);
        private static bool EwerHeld => Core.Player.HasAura(924);
        private static bool SpireHeld => Core.Player.HasAura(925);

        #endregion

        #region Card Drawn

        private static bool HasCard => Core.Player.HasAura(913) || Core.Player.HasAura(914) || Core.Player.HasAura(915) ||
                                       Core.Player.HasAura(916) || Core.Player.HasAura(917) || Core.Player.HasAura(918);
        private static bool CardSupport => Core.Player.HasAura(914) || Core.Player.HasAura(917) || Core.Player.HasAura(918);
        private static bool CardBalance => Core.Player.HasAura(913);
        private static bool CardBole => Core.Player.HasAura(914);
        private static bool CardArrow => Core.Player.HasAura(915);
        private static bool CardSpear => Core.Player.HasAura(916);
        private static bool CardEwer => Core.Player.HasAura(917);
        private static bool CardSpire => Core.Player.HasAura(918);

        #endregion

        #region Card Minor Arcana
        private static bool HasArcana => Resource.Arcana != Resource.AstrologianCard.None;
        private static bool CardLord => Resource.Arcana == Resource.AstrologianCard.LordofCrowns;
        private static bool CardLady => Resource.Arcana == Resource.AstrologianCard.LadyofCrowns;

        #endregion

        #endregion
    }
}