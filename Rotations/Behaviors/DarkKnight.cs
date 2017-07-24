using System.Threading.Tasks;
using ShinraCo.Settings;

namespace ShinraCo.Rotations
{
    public sealed partial class DarkKnight : Rotation
    {
        #region Combat

        public override async Task<bool> Combat()
        {
            if (Shinra.Settings.RotationMode == Modes.Smart)
            {
                if (await Unleash()) return true;
                if (await SyphonStrike()) return true;
                if (await PowerSlash()) return true;
                if (await SpinningSlash()) return true;
                return await HardSlash();
            }
            if (Shinra.Settings.RotationMode == Modes.Single)
            {
                if (await SyphonStrike()) return true;
                if (await PowerSlash()) return true;
                if (await SpinningSlash()) return true;
                return await HardSlash();
            }
            if (Shinra.Settings.RotationMode == Modes.Multi)
            {
                if (await Unleash()) return true;
                if (await SyphonStrike()) return true;
                return await HardSlash();
            }
            return false;
        }

        #endregion

        #region CombatBuff

        public override async Task<bool> CombatBuff()
        {
            if (await Shinra.SummonChocobo()) return true;
            if (await Shinra.ChocoboStance()) return true;
            if (await Grit()) return true;
            if (await Rampart()) return true;
            if (await Convalescence()) return true;
            if (await Anticipation()) return true;
            if (await Awareness()) return true;
            if (await Reprisal()) return true;
            if (await Darkside()) return true;
            return await BloodWeapon();
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
            if (await Shinra.SummonChocobo()) return true;
            return await Grit();
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