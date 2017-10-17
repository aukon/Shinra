using System.Threading.Tasks;
using ShinraCo.Settings;

namespace ShinraCo.Rotations
{
    public sealed partial class Astrologian : Rotation
    {
        #region Combat

        public override async Task<bool> Combat()
        {
            if (Shinra.Settings.RotationMode == Modes.Smart)
            {
                if (await StellarDetonation()) return true;
                if (await EarthlyStar()) return true;
                if (await Gravity()) return true;
                if (await CombustII()) return true;
                if (await Combust()) return true;
                if (await MaleficIII()) return true;
                if (await MaleficII()) return true;
                return await Malefic();
            }
            if (Shinra.Settings.RotationMode == Modes.Single)
            {
                if (await CombustII()) return true;
                if (await Combust()) return true;
                if (await MaleficIII()) return true;
                if (await MaleficII()) return true;
                return await Malefic();
            }
            if (Shinra.Settings.RotationMode == Modes.Multi)
            {
                if (await StellarDetonation()) return true;
                if (await EarthlyStar()) return true;
                if (await Gravity()) return true;
                if (await CombustII()) return true;
                return await Combust();
            }
            return false;
        }

        #endregion

        #region CombatBuff

        public override async Task<bool> CombatBuff()
        {
            if (await Shinra.SummonChocobo()) return true;
            if (await Shinra.ChocoboStance()) return true;
            if (await LucidDreaming()) return true;
            if (await SleeveDraw()) return true;
            if (await RoyalRoad()) return true;
            if (await Redraw()) return true;
            if (await Spread()) return true;
            if (await LordOfCrowns()) return true;
            if (await LadyOfCrowns()) return true;
            if (await MinorArcana()) return true;
            if (await Undraw()) return true;
            if (await Draw()) return true;
            return await ClericStance();
        }

        #endregion

        #region Heal

        public override async Task<bool> Heal()
        {
            if (await UpdateHealing()) return true;
            if (await StopCasting()) return true;
            if (await EssentialDignity()) return true;
            if (await AspectedHelios()) return true;
            if (await Helios()) return true;
            if (await BeneficII()) return true;
            if (await Benefic()) return true;
            if (await AspectedBenefic()) return true;
            if (await Ascend()) return true;
            if (await Esuna()) return true;
            return await Protect();
        }

        #endregion

        #region PreCombatBuff

        public override async Task<bool> PreCombatBuff()
        {
            if (await Shinra.SummonChocobo()) return true;
            if (await NocturnalSect()) return true;
            return await DiurnalSect();
        }

        #endregion

        #region Pull

        public override async Task<bool> Pull()
        {
            if (await CombustII()) return true;
            return await Combust();
        }

        #endregion
    }
}