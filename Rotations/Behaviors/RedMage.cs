using System.Threading.Tasks;

namespace ShinraCo.Rotations
{
    public sealed partial class RedMage : Rotation
    {
        #region Combat

        public override async Task<bool> Combat()
        {
            if (await Drain()) return true;
            if (await EnchantedMoulinet()) return true;
            if (await Scatter()) return true;
            if (await Verholy()) return true;
            if (await Verflare()) return true;
            if (await EnchantedRiposte()) return true;
            if (await EnchantedZwerchhau()) return true;
            if (await EnchantedRedoublement()) return true;
            if (await Veraero()) return true;
            if (await Verthunder()) return true;
            if (await Verstone()) return true;
            if (await Verfire()) return true;
            if (await Impact()) return true;
            if (await JoltII()) return true;
            if (await Jolt()) return true;
            return await Riposte();
        }

        #endregion
        
        #region CombatBuff

        public override async Task<bool> CombatBuff()
        {
            if (await Shinra.SummonChocobo()) return true;
            if (await Shinra.ChocoboStance()) return true;
            if (await Embolden()) return true;
            if (await CorpsACorps()) return true;
            if (await Manafication()) return true;
            if (await Fleche()) return true;
            if (await ContreSixte()) return true;
            if (await Acceleration()) return true;
            if (await Swiftcast()) return true;
            return await LucidDreaming();
        }

        #endregion

        #region Heal

        public override async Task<bool> Heal()
        {
            if (await Shinra.UsePotion()) return true;
            return await Vercure();
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