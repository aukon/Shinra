using System.Threading.Tasks;
using ShinraCo.Settings;

namespace ShinraCo.Rotations
{
    public sealed partial class BlackMage : Rotation
    {
        #region Combat

        public override async Task<bool> Combat()
        {
            if (Shinra.Settings.RotationMode == Modes.Smart)
            {
                if (await Drain()) return true;
                if (await ThunderII()) return true; 
                if (await ThunderIII()) return true;
                if (await Thunder()) return true;
                if (await FireIII()) return true;
                if (await Flare()) return true;
                if (await FireII()) return true;
                if (await Fire()) return true;
                if (await Scathe()) return true;
                if (await BlizzardIII()) return true;
                return await Blizzard();
            }
            if (Shinra.Settings.RotationMode == Modes.Single)
            {
                if (await Drain()) return true;
                if (await ThunderIII()) return true;
                if (await Thunder()) return true;
                if (await FireIII()) return true;
                if (await Fire()) return true;
                if (await Scathe()) return true;
                if (await BlizzardIII()) return true;
                return await Blizzard();
            }
            if (Shinra.Settings.RotationMode == Modes.Multi)
            {
                if (await Drain()) return true;
                if (await ThunderII()) return true;
                if (await FireIII()) return true;
                if (await Flare()) return true;
                if (await FireII()) return true;
                if (await Scathe()) return true;
                if (await BlizzardIII()) return true;
                return await Blizzard();
            }
            return false;
        }

        #endregion

        #region CombatBuff

        public override async Task<bool> CombatBuff()
        {
            if (await Shinra.SummonChocobo()) return true;
            if (await Shinra.ChocoboStance()) return true;
            if (await Transpose()) return true;
            return await LucidDreaming();
        }

        #endregion

        #region Heal

        public override async Task<bool> Heal()
        {
            return await Shinra.UsePotion();
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
    }
}