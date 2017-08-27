using System.Threading.Tasks;

namespace ShinraCo.Rotations
{
    public sealed partial class Machinist : Rotation
    {
        #region Combat

        public override async Task<bool> Combat()
        {
            if (await HotShot()) return true;
            if (await Flamethrower()) return true;
            if (await SpreadShot()) return true;
            if (await CleanShot()) return true;
            if (await SlugShot()) return true;
            return await SplitShot();
        }

        #endregion

        #region CombatBuff

        public override async Task<bool> CombatBuff()
        {
            if (await Shinra.SummonChocobo()) return true;
            if (await Shinra.ChocoboStance()) return true;
            if (await FlamethrowerBuff()) return true;
            if (await BarrelStabilizer()) return true;
            if (await GaussBarrel()) return true;
            if (await BishopAutoturret()) return true;
            if (await RookAutoturret()) return true;
            if (await Hypercharge()) return true;
            if (await Heartbreak()) return true;
            if (await GaussRound()) return true;
            if (await Reload()) return true;
            if (await Wildfire()) return true;
            if (await Reassemble()) return true;
            if (await RapidFire()) return true;
            if (await Ricochet()) return true;
            if (await Cooldown()) return true;
            if (await Invigorate()) return true;
            return await Tactician();
        }

        #endregion

        #region Heal

        public override async Task<bool> Heal()
        {
            if (await Shinra.UsePotion()) return true;
            return await SecondWind();
        }

        #endregion

        #region PreCombatBuff

        public override async Task<bool> PreCombatBuff()
        {
            if (await Shinra.SummonChocobo()) return true;
            if (await QuickReload()) return true;
            if (await GaussBarrel()) return true;
            return await Peloton();
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