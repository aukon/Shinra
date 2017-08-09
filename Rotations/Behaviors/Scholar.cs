using System.Threading.Tasks;

namespace ShinraCo.Rotations
{
    public sealed partial class Scholar : Rotation
    {
        #region Combat

        public override async Task<bool> Combat()
        {
            if (await MiasmaII()) return true;
            if (await BioII()) return true;
            if (await Miasma()) return true;
            if (await Bio()) return true;
            if (await BroilII()) return true;
            if (await Broil()) return true;
            return await Ruin();
        }

        #endregion

        #region CombatBuff

        public override async Task<bool> CombatBuff()
        {
            if (await Shinra.SummonChocobo()) return true;
            if (await Shinra.ChocoboStance()) return true;
            if (await SummonII()) return true;
            if (await Summon()) return true;
            if (await Aetherflow()) return true;
            if (await LucidDreaming()) return true;
            if (await Rouse()) return true;
            if (await Protect()) return true;
            if (await ChainStrategem()) return true;
            if (await ClericStance()) return true;
            if (await Bane()) return true;
            return await ShadowFlare();
        }

        #endregion

        #region Heal

        public override async Task<bool> Heal()
        {
            if (await Shinra.UsePotion()) return true;
            if (await Lustrate()) return true;
            if (await Aetherpact()) return true;
            if (await Indomitability()) return true;
            if (await Succor()) return true;
            if (await Adloquium()) return true;
            if (await Excogitation()) return true;
            if (await Physick()) return true;
            if (await Resurrection()) return true;
            return await Esuna();
        }

        #endregion

        #region PreCombatBuff

        public override async Task<bool> PreCombatBuff()
        {
            if (await Shinra.SummonChocobo()) return true;
            if (await Aetherflow()) return true;
            if (await SummonII()) return true;
            if (await Summon()) return true;
            return await Protect();
        }

        #endregion

        #region Pull

        public override async Task<bool> Pull()
        {
            if (await BioII()) return true;
            return await Bio();
        }

        #endregion
    }
}