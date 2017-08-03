using System.Threading.Tasks;

namespace ShinraCo.Rotations
{
    public sealed partial class Ninja : Rotation
    {
        #region Combat

        public override async Task<bool> Combat()
        {
            if (await AeolianEdge()) return true;
            if (await GustSlash()) return true;
            return await SpinningEdge();
        }

        #endregion

        #region CombatBuff

        public override async Task<bool> CombatBuff()
        {
            if (await Shinra.SummonChocobo()) return true;
            if (await Shinra.ChocoboStance()) return true;
            if (await Assassinate()) return true;
            if (await Mug()) return true;
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