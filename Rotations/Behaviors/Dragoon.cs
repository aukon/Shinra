using System.Threading.Tasks;

namespace ShinraCo.Rotations
{
    public sealed partial class Dragoon : Rotation
    {
        #region Combat

        public override async Task<bool> Combat()
        {
            if (await DoomSpike()) return true;
            if (await ChaosThrust()) return true;
            if (await Disembowel()) return true;
            if (await FullThrust()) return true;
            if (await VorpalThrust()) return true;
            if (await HeavyThrust()) return true;
            if (await ImpulseDrive()) return true;
            return await TrueThrust();
        }

        #endregion

        #region CombatBuff

        public override async Task<bool> CombatBuff()
        {
            if (await Shinra.SummonChocobo()) return true;
            if (await Shinra.ChocoboStance()) return true;
            if (await TrueNorth()) return true;
            if (await BloodForBlood()) return true;
            if (await LifeSurge()) return true;
            if (await DragonfireDive()) return true;
            if (await SpineshatterDive()) return true;
            if (await Jump()) return true;
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