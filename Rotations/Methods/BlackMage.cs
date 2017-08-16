using System;
using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using ShinraCo.Spells;
using ShinraCo.Spells.Main;
using Resource = ff14bot.Managers.ActionResourceManager.BlackMage;

namespace ShinraCo.Rotations
{
    public sealed partial class BlackMage
    {
        private BlackMageSpells MySpells { get; } = new BlackMageSpells();

        #region Damage

        private async Task<bool> Blizzard()
        {
            if (!ActionManager.HasSpell(MySpells.BlizzardIII.Name) && !UmbralIce)
            {
                return await MySpells.Blizzard.Cast();
            }
            return false;
        }

        private async Task<bool> BlizzardIII()
        {
            if (!UmbralIce)
            {
                return await MySpells.BlizzardIII.Cast();
            }
            return false;
        }

        private async Task<bool> BlizzardIV()
        {
            if (UmbralIce && Resource.UmbralHearts < 3)
            {
                return await MySpells.BlizzardIV.Cast();
            }
            return false;
        }

        private async Task<bool> Fire()
        {
            if (Core.Player.CurrentManaPercent > 10)
            {
                if (AstralFire || Core.Player.ClassLevel < 34 && Core.Player.CurrentManaPercent > 80)
                {
                    return await MySpells.Fire.Cast();
                }
            }
            return false;
        }

        private async Task<bool> FireIII()
        {
            if (!AstralFire && Core.Player.CurrentManaPercent > 80 || AstralFire && Core.Player.HasAura("Firestarter"))
            {
                return await MySpells.FireIII.Cast();
            }
            return false;
        }

        private async Task<bool> FireIV()
        {
            if (Resource.StackTimer.TotalMilliseconds > 6000)
            {
                if (ActionManager.CanCast(MySpells.FireIV.Name, Core.Player.CurrentTarget))
                {
                    if (Shinra.Settings.BlackMageTriplecast && ActionManager.LastSpell.Name == MySpells.FireIII.Name)
                    {
                        if (await MySpells.Triplecast.Cast(null, false))
                        {
                            await Coroutine.Wait(3000, () => Core.Player.HasAura(MySpells.Triplecast.Name));
                        }
                    }
                    if (Shinra.Settings.BlackMageSwiftcast && !Core.Player.HasAura(MySpells.Triplecast.Name))
                    {
                        if (await MySpells.Role.Swiftcast.Cast(null, false))
                        {
                            await Coroutine.Wait(3000, () => Core.Player.HasAura(MySpells.Role.Swiftcast.Name));
                        }
                    }
                }
                return await MySpells.FireIV.Cast();
            }
            return false;
        }

        private async Task<bool> Foul()
        {
            if (UmbralIce)
            {
                return await MySpells.Foul.Cast();
            }
            return false;
        }

        private async Task<bool> Scathe()
        {
            if (Shinra.Settings.BlackMageScathe && MovementManager.IsMoving && Core.Player.CurrentManaPercent > 20)
            {
                if (Resource.StackTimer.TotalMilliseconds > 8000 || Resource.StackTimer.TotalMilliseconds == 0)
                {
                    return await MySpells.Scathe.Cast();
                }
            }
            return false;
        }

        #endregion

        #region DoT

        private async Task<bool> Thunder()
        {
            if (!ActionManager.HasSpell(MySpells.ThunderIII.Name))
            {
                if (UmbralIce && !Core.Player.CurrentTarget.HasAura(MySpells.Thunder.Name, true, 6000) ||
                    Core.Player.HasAura("Thundercloud"))
                {
                    return await MySpells.Thunder.Cast();
                }
            }
            return false;
        }

        private async Task<bool> ThunderIII()
        {
            if (UmbralIce && !Core.Player.CurrentTarget.HasAura(MySpells.ThunderIII.Name, true, 8000) ||
                !Resource.Enochian && Core.Player.HasAura("Thundercloud"))
            {
                return await MySpells.ThunderIII.Cast();
            }
            return false;
        }

        #endregion

        #region AoE

        private async Task<bool> BlizzardMulti()
        {
            if (!AstralFire && !UmbralIce || !ActionManager.HasSpell(MySpells.Flare.Name))
            {
                return await MySpells.Blizzard.Cast();
            }
            return false;
        }

        private async Task<bool> FireMulti()
        {
            if (Core.Player.ClassLevel < 18 && (AstralFire || Core.Player.CurrentManaPercent > 80))
            {
                return await MySpells.Fire.Cast();
            }
            return false;
        }

        private async Task<bool> FireII()
        {
            if (AstralFire || Core.Player.ClassLevel < 34 && Core.Player.CurrentManaPercent > 80)
            {
                if (ActionManager.CanCast(MySpells.FireII.Name, Core.Player.CurrentTarget))
                {
                    if (Shinra.Settings.BlackMageTriplecast && ActionManager.LastSpell.Name == MySpells.FireIII.Name)
                    {
                        if (await MySpells.Triplecast.Cast(null, false))
                        {
                            await Coroutine.Wait(3000, () => Core.Player.HasAura(MySpells.Triplecast.Name));
                        }
                    }
                }
                return await MySpells.FireII.Cast();
            }
            return false;
        }

        private async Task<bool> FireIIIMulti()
        {
            if (!AstralFire && Core.Player.CurrentManaPercent > 25 &&
                (ActionManager.HasSpell(MySpells.Flare.Name) || Core.Player.CurrentManaPercent > 80))
            {
                Spell.RecentSpell.RemoveAll(t => DateTime.UtcNow > t);
                if (!RecentTranspose)
                {
                    return await MySpells.FireIII.Cast();
                }
            }
            return false;
        }

        private async Task<bool> Flare()
        {
            if (AstralFire && Core.Player.CurrentManaPercent < 25)
            {
                if (Shinra.Settings.BlackMageConvert && ActionManager.HasSpell(MySpells.Flare.Name) &&
                    !ActionManager.CanCast(MySpells.Flare.Name, Core.Player.CurrentTarget))
                {
                    if (await MySpells.Convert.Cast(null, false))
                    {
                        await Coroutine.Wait(3000, () => ActionManager.CanCast(MySpells.Flare.Name, Core.Player.CurrentTarget));
                    }
                }
                if (Shinra.Settings.BlackMageSwiftcast && ActionManager.CanCast(MySpells.Flare.Name, Core.Player.CurrentTarget) &&
                    !Core.Player.HasAura(MySpells.Triplecast.Name))
                {
                    if (await MySpells.Role.Swiftcast.Cast(null, false))
                    {
                        await Coroutine.Wait(3000, () => Core.Player.HasAura(MySpells.Role.Swiftcast.Name));
                    }
                }
                return await MySpells.Flare.Cast();
            }
            return false;
        }

        private async Task<bool> TransposeMulti()
        {
            if (AstralFire && Core.Player.CurrentManaPercent < 20 && !ActionManager.CanCast(MySpells.Flare.Name, Core.Player.CurrentTarget))
            {
                if (await MySpells.Transpose.Cast(null, false))
                {
                    Spell.RecentSpell.Add(MySpells.Transpose.Name, DateTime.UtcNow + TimeSpan.FromSeconds(4));
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Buff

        private async Task<bool> Transpose()
        {
            if (AstralFire && Core.Player.CurrentManaPercent < 20 &&
                (!ActionManager.HasSpell(MySpells.BlizzardIII.Name) || Core.Player.CurrentMana < BlizzardIIICost))
            {
                return await MySpells.Transpose.Cast(null, false);
            }
            return false;
        }

        private async Task<bool> Convert()
        {
            if (Shinra.Settings.BlackMageConvert && AstralFire && ActionManager.LastSpell.Name == MySpells.FireIII.Name)
            {
                return await MySpells.Convert.Cast();
            }
            return false;
        }

        private async Task<bool> LeyLines()
        {
            if (Shinra.Settings.BlackMageLeyLines && !MovementManager.IsMoving)
            {
                if (Core.Player.CurrentManaPercent > 80 || ActionManager.LastSpell.Name == MySpells.FireII.Name)
                {
                    return await MySpells.LeyLines.Cast(null, false);
                }
            }
            return false;
        }

        private async Task<bool> Sharpcast()
        {
            if (Shinra.Settings.BlackMageSharpcast && AstralFire && Core.Player.CurrentManaPercent > 60)
            {
                return await MySpells.Sharpcast.Cast(null, false);
            }
            return false;
        }

        private async Task<bool> Enochian()
        {
            if (Shinra.Settings.BlackMageEnochian && Core.Player.ClassLevel >= 60 && !Resource.Enochian &&
                Resource.StackTimer.TotalMilliseconds > 6000)
            {
                return await MySpells.Enochian.Cast(null, false);
            }
            return false;
        }

        #endregion

        #region Role

        private async Task<bool> Drain()
        {
            if (Shinra.Settings.BlackMageDrain && Core.Player.CurrentHealthPercent < Shinra.Settings.BlackMageDrainPct)
            {
                return await MySpells.Role.Drain.Cast();
            }
            return false;
        }

        private async Task<bool> LucidDreaming()
        {
            if (Shinra.Settings.BlackMageLucidDreaming && Core.Player.CurrentManaPercent < Shinra.Settings.BlackMageLucidDreamingPct)
            {
                return await MySpells.Role.LucidDreaming.Cast();
            }
            return false;
        }

        #endregion

        #region Custom

        private static double ManaReduction => Resource.AstralStacks > 1 ? 0.25 : Resource.AstralStacks > 0 ? 0.5 : 1;
        private static double BlizzardIIICost => DataManager.GetSpellData("Blizzard III").Cost * ManaReduction;

        private static bool RecentTranspose { get { return Spell.RecentSpell.Keys.Any(rs => rs.Contains("Transpose")); } }
        private static bool AstralFire => Resource.AstralStacks > 0 && Shinra.LastSpell.Name != "Transpose";
        private static bool UmbralIce => Resource.UmbralStacks > 0;

        #endregion
    }
}