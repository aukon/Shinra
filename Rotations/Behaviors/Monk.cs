using System.Threading.Tasks;
using ShinraCo.Settings;

namespace ShinraCo.Rotations
{
    public sealed partial class Monk : Rotation
    {
        #region Combat

        public override async Task<bool> Combat()
        {
            if (Shinra.Settings.RotationMode == Modes.Smart)
            {
                if (await Rockbreaker()) return true;
                if (await Demolish()) return true;
                if (await SnapPunch()) return true;
                if (await DragonKick()) return true;
                if (await TwinSnakes()) return true;
                if (await TrueStrike()) return true;
                return await Bootshine();
            }
            if (Shinra.Settings.RotationMode == Modes.Single)
            {
                if (await Demolish()) return true;
                if (await SnapPunch()) return true;
                if (await DragonKick()) return true;
                if (await TwinSnakes()) return true;
                if (await TrueStrike()) return true;
                return await Bootshine();
            }
            if (Shinra.Settings.RotationMode == Modes.Multi)
            {
                if (await Rockbreaker()) return true;
                if (await TwinSnakes()) return true;
                if (await TrueStrike()) return true;
                if (await DragonKick()) return true;
                return await Bootshine();
            }
            return false;
        }

        #endregion

        #region CombatBuff

        public override async Task<bool> CombatBuff()
        {
            if (await Shinra.SummonChocobo()) return true;
            if (await Shinra.ChocoboStance()) return true;
            if (await FistsOfFire()) return true;
            if (await FistsOfWind()) return true;
            if (await FistsOfEarth()) return true;
            if (await ShoulderTackle()) return true;
            if (await PerfectBalance()) return true;
            if (await InternalRelease()) return true;
            if (await TrueNorth()) return true;
            if (await HowlingFist()) return true;
            if (await SteelPeak()) return true;
            return await Invigorate();
        }

        #endregion

        #region Heal

        public override async Task<bool> Heal()
        {
            if (await Shinra.UsePotion()) return true;
            if (await SecondWind()) return true;
            return await Bloodbath();
        }

        #endregion

        #region PreCombatBuff

        public override async Task<bool> PreCombatBuff()
        {
            if (await Shinra.SummonChocobo()) return true;
            if (await FistsOfFire()) return true;
            if (await FistsOfWind()) return true;
            return await FistsOfEarth();
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