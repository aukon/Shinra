using System.Threading.Tasks;

namespace ShinraCo.Rotations
{
    public sealed partial class BasicClass : Rotation
    {
        #region Combat

        public override async Task<bool> Combat()
        {
            // Arcanist
            if (await Miasma()) return true;
            if (await BioII()) return true;
            if (await Bio()) return true;
            if (await Ruin()) return true;
            // Archer
            if (await VenomousBite()) return true;
            if (await StraightShot()) return true;
            if (await HeavyShot()) return true;
            // Conjurer
            if (await AeroII()) return true;
            if (await Aero()) return true;
            if (await StoneII()) return true;
            if (await Stone()) return true;
            // Gladiator
            if (await Flash()) return true;
            if (await RageOfHalone()) return true;
            if (await RiotBlade()) return true;
            if (await SavageBlade()) return true;
            if (await FastBlade()) return true;
            // Lancer
            if (await FullThrust()) return true;
            if (await VorpalThrust()) return true;
            if (await HeavyThrust()) return true;
            if (await TrueThrust()) return true;
            // Marauder
            if (await Overpower()) return true;
            if (await StormsPath()) return true;
            if (await Maim()) return true;
            if (await ButchersBlock()) return true;
            if (await SkullSunder()) return true;
            if (await HeavySwing()) return true;
            // Pugilist
            if (await SnapPunch()) return true;
            if (await TwinSnakes()) return true;
            if (await TrueStrike()) return true;
            return await Bootshine();
        }

        #endregion

        #region CombatBuff

        public override async Task<bool> CombatBuff()
        {
            if (await Shinra.SummonChocobo()) return true;
            if (await Shinra.ChocoboStance()) return true;
            // Arcanist
            if (await SummonII()) return true;
            if (await Summon()) return true;
            if (await EnergyDrain()) return true;
            if (await Aetherflow()) return true;
            // Archer
            if (await Bloodletter()) return true;
            if (await MiserysEnd()) return true;
            if (await RagingStrikes()) return true;
            // Gladiator
            if (await FightOrFlight()) return true;
            // Lancer
            if (await BloodForBlood()) return true;
            if (await LifeSurge()) return true;
            // Marauder
            if (await Berserk()) return true;
            // Pugilist
            if (await FistsOfEarth()) return true;
            return await InternalRelease();
        }

        #endregion

        #region Heal

        public override async Task<bool> Heal()
        {
            if (await Shinra.UsePotion()) return true;
            // Arcanist
            if (await Physick()) return true;
            // Conjurer
            if (await CureII()) return true;
            if (await Cure()) return true;
            // Role
            return await SecondWind();
        }

        #endregion

        #region PreCombatBuff

        public override async Task<bool> PreCombatBuff()
        {
            if (await Shinra.SummonChocobo()) return true;
            // Arcanist
            if (await SummonII()) return true;
            if (await Summon()) return true;
            // Pugilist
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