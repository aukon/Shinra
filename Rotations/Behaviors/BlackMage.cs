﻿using System.Threading.Tasks;
using ShinraCo.Settings;

namespace ShinraCo.Rotations
{
    public sealed partial class BlackMage : Rotation
    {
        #region Combat

        public override async Task<bool> Combat()
        {
            if (Shinra.Settings.RotationMode == Modes.Multi || Shinra.Settings.RotationMode == Modes.Smart &&
                Helpers.EnemiesNearTarget(5) > 2)
            {
                return await Multi();
            }
            return await Single();
        }

        private async Task<bool> Single()
        {
            if (Shinra.Settings.BlackMageOpener) { if (await Helpers.ExecuteOpener()) return true; }
            if (await Transpose()) return true;
            if (await Triplecast()) return true;
            if (await Swiftcast()) return true;
            if (await Sharpcast()) return true;
            if (await Thundercloud()) return true;
            if (await Drain()) return true;
            if (await Foul()) return true;
            if (await ThunderIII()) return true;
            if (await Thunder()) return true;
            if (await BlizzardIV()) return true;
            if (await FireIV()) return true;
            if (await FireIII()) return true;
            if (await Fire()) return true;
            if (await BlizzardIII()) return true;
            if (await Blizzard()) return true;
            return await Scathe();
        }

        private async Task<bool> Multi()
        {
            if (await Drain()) return true;
            if (await Foul()) return true;
            if (await ThunderIV()) return true;
            if (await ThunderII()) return true;
            if (await BlizzardIV()) return true;
            if (await FireIIIMulti()) return true;
            if (await Flare()) return true;
            if (await FireII()) return true;
            if (await BlizzardIII()) return true;
            if (await TransposeMulti()) return true;
            if (await FireMulti()) return true;
            return await BlizzardMulti();
        }

        #endregion

        #region CombatBuff

        public override async Task<bool> CombatBuff()
        {
            if (await Shinra.SummonChocobo()) return true;
            if (await Shinra.ChocoboStance()) return true;
            if (Shinra.Settings.BlackMageOpener) { if (await Helpers.ExecuteOpener()) return true; }
            if (await Convert()) return true;
            if (await Enochian()) return true;
            if (await LeyLines()) return true;
            if (await LucidDreaming()) return true;
		
            await Helpers.UpdateParty();
            return await ManaShift();

        }

        #endregion

        #region Heal

        public override async Task<bool> Heal()
        {
            return false;
        }

        #endregion

        #region PreCombatBuff

        public override async Task<bool> PreCombatBuff()
        {
            return await Shinra.SummonChocobo();
        }

        #endregion

        #region Pull

        public override async Task<bool> Pull()
        {
            return await Combat();
        }

        #endregion

        #region CombatPVP

        public override async Task<bool> CombatPVP()
        {
            if (await ThunderIIIPVP()) return true;
            if (await ThunderPVP()) return true;
            if (await FlarePVP()) return true;
            if (await FoulPVP()) return true;
            if (await FireIVPVP()) return true;
            if (await EnochianPVP()) return true;
            if (await FirePVP()) return true;
            if (await BlizzardIVPVP()) return true;
            return await BlizzardPVP();
        }

        #endregion
    }
}
