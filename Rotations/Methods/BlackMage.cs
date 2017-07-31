using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using ShinraCo.Settings;
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
            if (!ActionManager.HasSpell(MySpells.BlizzardIII.Name))
            {
                return await MySpells.Blizzard.Cast();
            }
            return false;
        }

        private async Task<bool> BlizzardIII()
        {
            return await MySpells.BlizzardIII.Cast();
        }

        private async Task<bool> Fire()
        {
            if (AstralFire && Core.Player.CurrentManaPercent > 20 || Core.Player.CurrentManaPercent > 80)
            {
                return await MySpells.Fire.Cast();
            }
            return false;
        }

        private async Task<bool> FireIII()
        {
            if (AstralFire && Core.Player.HasAura(165) || !AstralFire &&
                (Core.Player.CurrentManaPercent > 80 || Shinra.LastSpell.Name == ThunderIIDebuff))
            {
                if (Shinra.Settings.BlackMageSwiftcast && !UmbralIce && !Core.Player.HasAura(165) &&
                    ActionManager.CanCast(MySpells.FireIII.Name, Core.Player.CurrentTarget))
                {
                    if (await MySpells.Role.Swiftcast.Cast(null, false))
                    {
                        await Coroutine.Wait(3000, () => Core.Player.HasAura(MySpells.Role.Swiftcast.Name));
                    }
                }
                return await MySpells.FireIII.Cast();
            }
            return false;
        }

        private async Task<bool> Scathe()
        {
            if (UmbralIce || MovementManager.IsMoving && Core.Player.CurrentManaPercent > 20)
            {
                return await MySpells.Scathe.Cast();
            }
            return false;
        }

        #endregion

        #region DoT

        private async Task<bool> Thunder()
        {
            if (!ActionManager.HasSpell(MySpells.ThunderIII.Name))
            {
                if (!RecentThunder && UmbralIce && Core.Player.CurrentManaPercent < 80 &&
                    !Core.Player.CurrentTarget.HasAura(MySpells.Thunder.Name, true, 6000) &&
                    !Core.Player.CurrentTarget.HasAura(ThunderIIDebuff, true) || Thundercloud)
                {
                    return await MySpells.Thunder.Cast();
                }
            }
            return false;
        }

        private async Task<bool> ThunderIII()
        {
            if (!RecentThunder && UmbralIce && Core.Player.CurrentManaPercent < 80 &&
                !Core.Player.CurrentTarget.HasAura(MySpells.ThunderIII.Name, true, 8000) &&
                !Core.Player.CurrentTarget.HasAura(ThunderIIDebuff, true) || Thundercloud)
            {
                return await MySpells.ThunderIII.Cast();
            }
            return false;
        }

        #endregion

        #region AoE

        private async Task<bool> FireII()
        {
            if (AstralFire || Core.Player.CurrentManaPercent > 80)
            {
                return await MySpells.FireII.Cast();
            }
            return false;
        }

        private async Task<bool> ThunderII()
        {
            if (UmbralIce && Core.Player.CurrentManaPercent < 80 &&
                !Core.Player.CurrentTarget.HasAura(MySpells.ThunderII.Name, true, 4000) || Thundercloud)
            {
                if (Helpers.EnemiesNearTarget(5) > 2)
                {
                    return await MySpells.ThunderII.Cast();
                }
            }
            return false;
        }

        private async Task<bool> Flare()
        {
            if (AstralFire && Core.Player.CurrentManaPercent < 20)
            {
                return await MySpells.Flare.Cast();
            }
            return false;
        }

        #endregion

        #region Buff

        private async Task<bool> Transpose()
        {
            if (AstralFire && Core.Player.CurrentManaPercent <= 20 &&
                (!ActionManager.HasSpell(MySpells.BlizzardIII.Name) || Core.Player.CurrentMana < BlizzardIIICost))
            {
                return await MySpells.Transpose.Cast(null, false);
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

        private static string ThunderDebuff => Core.Player.ClassLevel > 44 ? "Thunder III" : "Thunder";
        private static string ThunderIIDebuff => Core.Player.ClassLevel > 63 ? "Thunder IV" : "Thunder II";
        private static double ManaReduction => Resource.AstralStacks > 1 ? 0.25 : Resource.AstralStacks > 0 ? 0.5 : 1;
        private static double BlizzardIIICost => DataManager.GetSpellData("Blizzard III").Cost * ManaReduction;
        private static bool RecentThunder { get { return Spell.RecentSpell.Keys.Any(rs => rs.Contains("Thunder")); } }
        private static bool AstralFire => Resource.AstralStacks > 0 && Shinra.LastSpell.Name != "Transpose";
        private static bool UmbralIce => Resource.UmbralStacks > 0;
        private static bool Thundercloud => Core.Player.HasAura(164);

        #endregion
    }
}