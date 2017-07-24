using System.Threading.Tasks;
using ff14bot;
using ff14bot.Managers;
using ShinraCo.Spells.Main;
using Resource = ff14bot.Managers.ActionResourceManager.DarkKnight;

namespace ShinraCo.Rotations
{
    public sealed partial class DarkKnight
    {
        private DarkKnightSpells MySpells { get; } = new DarkKnightSpells();

        #region Damage

        private async Task<bool> HardSlash()
        {
            return await MySpells.HardSlash.Cast();
        }

        private async Task<bool> SpinningSlash()
        {
            if (ActionManager.LastSpell.Name == MySpells.HardSlash.Name)
            {
                return await MySpells.SpinningSlash.Cast();
            }
            return false;
        }

        private async Task<bool> PowerSlash()
        {
            if (ActionManager.LastSpell.Name == MySpells.SpinningSlash.Name)
            {
                return await MySpells.PowerSlash.Cast();
            }
            return false;
        }

        private async Task<bool> SyphonStrike()
        {
            if (ActionManager.LastSpell.Name == MySpells.HardSlash.Name && Core.Player.CurrentManaPercent < 40)
            {
                return await MySpells.SyphonStrike.Cast();
            }
            return false;
        }

        #endregion

        #region AoE

        private async Task<bool> Unleash()
        {
            if (Shinra.Settings.DarkKnightUnleash && Core.Player.CurrentManaPercent > 40)
            {
                return await MySpells.Unleash.Cast();
            }
            return false;
        }

        #endregion

        #region Buff

        private async Task<bool> BloodWeapon()
        {
            if (Shinra.Settings.DarkKnightBloodWeapon)
            {
                return await MySpells.BloodWeapon.Cast();
            }
            return false;
        }

        #endregion

        #region Aura

        private async Task<bool> Grit()
        {
            if (Shinra.Settings.DarkKnightGrit && !Core.Player.HasAura(MySpells.Grit.Name))
            {
                return await MySpells.Grit.Cast();
            }
            return false;
        }

        private async Task<bool> Darkside()
        {
            if (Shinra.Settings.DarkKnightDarkside && !Core.Player.HasAura(MySpells.Darkside.Name) && Core.Player.CurrentManaPercent > 50)
            {
                return await MySpells.Darkside.Cast();
            }
            return false;
        }

        #endregion

        #region Role

        private async Task<bool> Rampart()
        {
            if (Shinra.Settings.DarkKnightRampart && Core.Player.CurrentHealthPercent < Shinra.Settings.DarkKnightRampartPct)
            {
                return await MySpells.Role.Rampart.Cast();
            }
            return false;
        }

        private async Task<bool> Convalescence()
        {
            if (Shinra.Settings.DarkKnightConvalescence && Core.Player.CurrentHealthPercent < Shinra.Settings.DarkKnightConvalescencePct)
            {
                return await MySpells.Role.Convalescence.Cast();
            }
            return false;
        }

        private async Task<bool> Anticipation()
        {
            if (Shinra.Settings.DarkKnightAnticipation && Core.Player.CurrentHealthPercent < Shinra.Settings.DarkKnightAnticipationPct)
            {
                return await MySpells.Role.Anticipation.Cast();
            }
            return false;
        }

        private async Task<bool> Reprisal()
        {
            if (Shinra.Settings.DarkKnightReprisal)
            {
                return await MySpells.Role.Reprisal.Cast();
            }
            return false;
        }

        private async Task<bool> Awareness()
        {
            if (Shinra.Settings.DarkKnightAwareness && Core.Player.CurrentHealthPercent < Shinra.Settings.DarkKnightAwarenessPct)
            {
                return await MySpells.Role.Awareness.Cast();
            }
            return false;
        }

        #endregion

        #region Custom

        private static int BloodValue => Resource.BlackBlood;

        #endregion
    }
}