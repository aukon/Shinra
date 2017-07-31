using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ff14bot.Managers;
using ShinraCo.Properties;

namespace ShinraCo.Settings.Forms
{
    public partial class ShinraForm : Form
    {
        private readonly Image _shinraBanner = Resources.ShinraBanner;

        public ShinraForm()
        {
            InitializeComponent();
        }

        #region Draggable

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        public const int WmNclbuttondown = 0xA1;
        public const int HtCaption = 0x2;

        private void ShinraForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            ReleaseCapture();
            SendMessage(Handle, WmNclbuttondown, HtCaption, 0);
        }

        #endregion

        private void ShinraForm_Load(object sender, EventArgs e)
        {
            ShinraBanner.Image = _shinraBanner;
            HotkeyManager.Unregister("Shinra Rotation");
            HotkeyManager.Unregister("Shinra Tank");
            Location = Shinra.Settings.WindowLocation;

            #region Main Settings

            #region Rotation

            RotationOverlay.Checked = Shinra.Settings.RotationOverlay;
            RotationMode.Text = Convert.ToString(Shinra.Settings.RotationMode);
            TankMode.Text = Convert.ToString(Shinra.Settings.TankMode);

            var kc = new KeysConverter();
            RotationHotkey.Text = kc.ConvertToString(Shinra.Settings.RotationHotkey);
            TankHotkey.Text = kc.ConvertToString(Shinra.Settings.TankHotkey);

            #endregion

            #region Chocobo

            ChocoboSummon.Checked = Shinra.Settings.SummonChocobo;
            ChocoboStanceDance.Checked = Shinra.Settings.ChocoboStanceDance;
            ChocoboStanceDancePct.Value = Shinra.Settings.ChocoboStanceDancePct;
            ChocoboStance.Text = Convert.ToString(Shinra.Settings.ChocoboStance);

            #endregion

            #region Item

            UsePotion.Checked = Shinra.Settings.UsePotion;
            UsePotionPct.Value = Shinra.Settings.UsePotionPct;

            #endregion

            #region Misc

            DefaultRoutine.Checked = Shinra.Settings.DefaultRoutine;

            #endregion

            #endregion

            #region Job Settings

            #region Astrologian

            #region Role

            AstrologianClericStance.Checked = Shinra.Settings.AstrologianClericStance;
            AstrologianProtect.Checked = Shinra.Settings.AstrologianProtect;
            AstrologianEsuna.Checked = Shinra.Settings.AstrologianEsuna;
            AstrologianLucidDreaming.Checked = Shinra.Settings.AstrologianLucidDreaming;
            AstrologianSwiftcast.Checked = Shinra.Settings.AstrologianSwiftcast;

            AstrologianLucidDreamingPct.Value = Shinra.Settings.AstrologianLucidDreamingPct;

            #endregion

            #region AoE

            AstrologianEarthlyStar.Checked = Shinra.Settings.AstrologianEarthlyStar;

            #endregion

            #region Heal

            AstrologianPartyHeal.Checked = Shinra.Settings.AstrologianPartyHeal;
            AstrologianBenefic.Checked = Shinra.Settings.AstrologianBenefic;
            AstrologianBeneficII.Checked = Shinra.Settings.AstrologianBeneficII;
            AstrologianEssDignity.Checked = Shinra.Settings.AstrologianEssDignity;
            AstrologianAspBenefic.Checked = Shinra.Settings.AstrologianAspBenefic;
            AstrologianHelios.Checked = Shinra.Settings.AstrologianHelios;
            AstrologianAspHelios.Checked = Shinra.Settings.AstrologianAspHelios;
            AstrologianAscend.Checked = Shinra.Settings.AstrologianAscend;

            AstrologianBeneficPct.Value = Shinra.Settings.AstrologianBeneficPct;
            AstrologianBeneficIIPct.Value = Shinra.Settings.AstrologianBeneficIIPct;
            AstrologianEssDignityPct.Value = Shinra.Settings.AstrologianEssDignityPct;
            AstrologianAspBeneficPct.Value = Shinra.Settings.AstrologianAspBeneficPct;
            AstrologianHeliosPct.Value = Shinra.Settings.AstrologianHeliosPct;
            AstrologianAspHeliosPct.Value = Shinra.Settings.AstrologianAspHeliosPct;

            #endregion

            #region Card

            AstrologianDraw.Checked = Shinra.Settings.AstrologianDraw;
            AstrologianSleeveDraw.Checked = Shinra.Settings.AstrologianSleeveDraw;

            #endregion

            #region Sect

            AstrologianSect.Text = Convert.ToString(Shinra.Settings.AstrologianSect);

            #endregion

            #endregion

            #region Bard

            #region Role

            BardSecondWind.Checked = Shinra.Settings.BardSecondWind;
            BardPeloton.Checked = Shinra.Settings.BardPeloton;
            BardInvigorate.Checked = Shinra.Settings.BardInvigorate;
            BardTactician.Checked = Shinra.Settings.BardTactician;

            BardSecondWindPct.Value = Shinra.Settings.BardSecondWindPct;
            BardInvigoratePct.Value = Shinra.Settings.BardInvigoratePct;
            BardTacticianPct.Value = Shinra.Settings.BardTacticianPct;

            #endregion

            #region Cooldown

            BardSongs.Checked = Shinra.Settings.BardSongs;
            BardSidewinder.Checked = Shinra.Settings.BardSidewinder;

            #endregion

            #region Buff

            BardRagingStrikes.Checked = Shinra.Settings.BardRagingStrikes;
            BardBarrage.Checked = Shinra.Settings.BardBarrage;
            BardBattleVoice.Checked = Shinra.Settings.BardBattleVoice;

            #endregion

            #endregion

            #region Black Mage

            #region Role

            BlackMageDrain.Checked = Shinra.Settings.BlackMageDrain;
            BlackMageLucidDreaming.Checked = Shinra.Settings.BlackMageLucidDreaming;
            BlackMageSwiftcast.Checked = Shinra.Settings.BlackMageSwiftcast;

            BlackMageDrainPct.Value = Shinra.Settings.BlackMageDrainPct;
            BlackMageLucidDreamingPct.Value = Shinra.Settings.BlackMageLucidDreamingPct;

            #endregion

            #endregion

            #region Dark Knight

            #region Role

            DarkKnightRampart.Checked = Shinra.Settings.DarkKnightRampart;
            DarkKnightConvalescence.Checked = Shinra.Settings.DarkKnightConvalescence;
            DarkKnightAnticipation.Checked = Shinra.Settings.DarkKnightAnticipation;
            DarkKnightReprisal.Checked = Shinra.Settings.DarkKnightReprisal;
            DarkKnightAwareness.Checked = Shinra.Settings.DarkKnightAwareness;

            DarkKnightRampartPct.Value = Shinra.Settings.DarkKnightRampartPct;
            DarkKnightConvalescencePct.Value = Shinra.Settings.DarkKnightConvalescencePct;
            DarkKnightAnticipationPct.Value = Shinra.Settings.DarkKnightAnticipationPct;
            DarkKnightAwarenessPct.Value = Shinra.Settings.DarkKnightAwarenessPct;

            #endregion

            #region Damage

            DarkKnightBloodspiller.Checked = Shinra.Settings.DarkKnightBloodspiller;

            #endregion

            #region AoE

            DarkKnightQuietus.Checked = Shinra.Settings.DarkKnightQuietus;

            #endregion

            #region Cooldown

            DarkKnightSaltedEarth.Checked = Shinra.Settings.DarkKnightSaltedEarth;
            DarkKnightPlunge.Checked = Shinra.Settings.DarkKnightPlunge;
            DarkKnightCarveAndSpit.Checked = Shinra.Settings.DarkKnightCarveAndSpit;

            #endregion

            #region Buff

            DarkKnightBloodWeapon.Checked = Shinra.Settings.DarkKnightBloodWeapon;
            DarkKnightBloodPrice.Checked = Shinra.Settings.DarkKnightBloodPrice;
            DarkKnightShadowWall.Checked = Shinra.Settings.DarkKnightShadowWall;
            DarkKnightLivingDead.Checked = Shinra.Settings.DarkKnightLivingDead;
            DarkKnightDelirium.Checked = Shinra.Settings.DarkKnightDelirium;
            DarkKnightBlackestNight.Checked = Shinra.Settings.DarkKnightBlackestNight;

            DarkKnightBloodPricePct.Value = Shinra.Settings.DarkKnightBloodPricePct;
            DarkKnightShadowWallPct.Value = Shinra.Settings.DarkKnightShadowWallPct;
            DarkKnightLivingDeadPct.Value = Shinra.Settings.DarkKnightLivingDeadPct;
            DarkKnightBlackestNightPct.Value = Shinra.Settings.DarkKnightBlackestNightPct;

            #endregion

            #region Dark Arts

            DarkKnightSouleaterArts.Checked = Shinra.Settings.DarkKnightSouleaterArts;
            DarkKnightAbyssalArts.Checked = Shinra.Settings.DarkKnightAbyssalArts;
            DarkKnightCarveArts.Checked = Shinra.Settings.DarkKnightCarveArts;
            DarkKnightQuietusArts.Checked = Shinra.Settings.DarkKnightQuietusArts;
            DarkKnightBloodspillerArts.Checked = Shinra.Settings.DarkKnightBloodspillerArts;

            #endregion

            #region Aura

            DarkKnightGrit.Checked = Shinra.Settings.DarkKnightGrit;
            DarkKnightDarkside.Checked = Shinra.Settings.DarkKnightDarkside;

            #endregion

            #endregion

            #region Dragoon

            #region Role

            DragoonSecondWind.Checked = Shinra.Settings.DragoonSecondWind;
            DragoonInvigorate.Checked = Shinra.Settings.DragoonInvigorate;
            DragoonBloodbath.Checked = Shinra.Settings.DragoonBloodbath;
            DragoonTrueNorth.Checked = Shinra.Settings.DragoonTrueNorth;

            DragoonSecondWindPct.Value = Shinra.Settings.DragoonSecondWindPct;
            DragoonInvigoratePct.Value = Shinra.Settings.DragoonInvigoratePct;
            DragoonBloodbathPct.Value = Shinra.Settings.DragoonBloodbathPct;

            #endregion

            #region Cooldown

            DragoonJump.Checked = Shinra.Settings.DragoonJump;
            DragoonSpineshatter.Checked = Shinra.Settings.DragoonSpineshatter;
            DragoonDragonfire.Checked = Shinra.Settings.DragoonDragonfire;
            DragoonGeirskogul.Checked = Shinra.Settings.DragoonGeirskogul;
            DragoonMirage.Checked = Shinra.Settings.DragoonMirage;

            #endregion

            #region Buff

            DragoonLifeSurge.Checked = Shinra.Settings.DragoonLifeSurge;
            DragoonBloodForBlood.Checked = Shinra.Settings.DragoonBloodForBlood;
            DragoonBattleLitany.Checked = Shinra.Settings.DragoonBattleLitany;
            DragoonBloodOfTheDragon.Checked = Shinra.Settings.DragoonBloodOfTheDragon;

            #endregion

            #endregion

            #region Monk

            #region Role

            MonkSecondWind.Checked = Shinra.Settings.MonkSecondWind;
            MonkInvigorate.Checked = Shinra.Settings.MonkInvigorate;
            MonkBloodbath.Checked = Shinra.Settings.MonkBloodbath;
            MonkTrueNorth.Checked = Shinra.Settings.MonkTrueNorth;

            MonkSecondWindPct.Value = Shinra.Settings.MonkSecondWindPct;
            MonkInvigoratePct.Value = Shinra.Settings.MonkInvigoratePct;
            MonkBloodbathPct.Value = Shinra.Settings.MonkBloodbathPct;

            #endregion

            #region DoT

            MonkDemolish.Checked = Shinra.Settings.MonkDemolish;
            MonkDemolishHP.Value = Shinra.Settings.MonkDemolishHP;

            #endregion

            #region Cooldown

            MonkShoulderTackle.Checked = Shinra.Settings.MonkShoulderTackle;
            MonkSteelPeak.Checked = Shinra.Settings.MonkSteelPeak;
            MonkHowlingFist.Checked = Shinra.Settings.MonkHowlingFist;
            MonkForbiddenChakra.Checked = Shinra.Settings.MonkForbiddenChakra;
            MonkElixirField.Checked = Shinra.Settings.MonkElixirField;
            MonkFireTackle.Checked = Shinra.Settings.MonkFireTackle;

            #endregion

            #region Buff

            MonkInternalRelease.Checked = Shinra.Settings.MonkInternalRelease;
            MonkPerfectBalance.Checked = Shinra.Settings.MonkPerfectBalance;
            MonkRiddleOfFire.Checked = Shinra.Settings.MonkRiddleOfFire;
            MonkBrotherhood.Checked = Shinra.Settings.MonkBrotherhood;

            #endregion

            #region Fists

            MonkFist.Text = Convert.ToString(Shinra.Settings.MonkFist);

            #endregion

            #endregion

            #region Paladin

            #region Role

            PaladinRampart.Checked = Shinra.Settings.PaladinRampart;
            PaladinConvalescence.Checked = Shinra.Settings.PaladinConvalescence;
            PaladinAnticipation.Checked = Shinra.Settings.PaladinAnticipation;
            PaladinReprisal.Checked = Shinra.Settings.PaladinReprisal;
            PaladinAwareness.Checked = Shinra.Settings.PaladinAwareness;

            PaladinRampartPct.Value = Shinra.Settings.PaladinRampartPct;
            PaladinConvalescencePct.Value = Shinra.Settings.PaladinConvalescencePct;
            PaladinAnticipationPct.Value = Shinra.Settings.PaladinAnticipationPct;
            PaladinAwarenessPct.Value = Shinra.Settings.PaladinAwarenessPct;

            #endregion

            #region Damage

            PaladinGoringBlade.Checked = Shinra.Settings.PaladinGoringBlade;

            #endregion

            #region AoE

            PaladinFlash.Checked = Shinra.Settings.PaladinFlash;
            PaladinTotalEclipse.Checked = Shinra.Settings.PaladinTotalEclipse;

            #endregion

            #region Cooldown

            PaladinShieldSwipe.Checked = Shinra.Settings.PaladinShieldSwipe;
            PaladinSpiritsWithin.Checked = Shinra.Settings.PaladinSpiritsWithin;
            PaladinCircleOfScorn.Checked = Shinra.Settings.PaladinCircleOfScorn;
            PaladinRequiescat.Checked = Shinra.Settings.PaladinRequiescat;

            #endregion

            #region Buff

            PaladinFightOrFlight.Checked = Shinra.Settings.PaladinFightOrFlight;
            PaladinBulwark.Checked = Shinra.Settings.PaladinBulwark;
            PaladinSentinel.Checked = Shinra.Settings.PaladinSentinel;
            PaladinHallowedGround.Checked = Shinra.Settings.PaladinHallowedGround;
            PaladinSheltron.Checked = Shinra.Settings.PaladinSheltron;

            PaladinBulwarkPct.Value = Shinra.Settings.PaladinBulwarkPct;
            PaladinSentinelPct.Value = Shinra.Settings.PaladinSentinelPct;
            PaladinHallowedGroundPct.Value = Shinra.Settings.PaladinHallowedGroundPct;

            #endregion

            #region Heal

            PaladinClemency.Checked = Shinra.Settings.PaladinClemency;
            PaladinClemencyPct.Value = Shinra.Settings.PaladinClemencyPct;

            #endregion

            #region Oath

            PaladinOath.Text = Convert.ToString(Shinra.Settings.PaladinOath);

            #endregion

            #endregion

            #region Red Mage

            #region Role

            RedMageDrain.Checked = Shinra.Settings.RedMageDrain;
            RedMageLucidDreaming.Checked = Shinra.Settings.RedMageLucidDreaming;
            RedMageSwiftcast.Checked = Shinra.Settings.RedMageSwiftcast;

            RedMageDrainPct.Value = Shinra.Settings.RedMageDrainPct;
            RedMageLucidDreamingPct.Value = Shinra.Settings.RedMageLucidDreamingPct;

            #endregion

            #region Cooldown

            RedMageCorpsACorps.Checked = Shinra.Settings.RedMageCorpsACorps;

            #endregion

            #region Buff

            RedMageEmbolden.Checked = Shinra.Settings.RedMageEmbolden;
            RedMageManafication.Checked = Shinra.Settings.RedMageManafication;

            #endregion

            #region Heal

            RedMageVercure.Checked = Shinra.Settings.RedMageVercure;
            RedMageVercurePct.Value = Shinra.Settings.RedMageVercurePct;

            #endregion

            #endregion

            #region Samurai

            #region Role

            SamuraiSecondWind.Checked = Shinra.Settings.SamuraiSecondWind;
            SamuraiInvigorate.Checked = Shinra.Settings.SamuraiInvigorate;
            SamuraiBloodbath.Checked = Shinra.Settings.SamuraiBloodbath;
            SamuraiTrueNorth.Checked = Shinra.Settings.SamuraiTrueNorth;

            SamuraiSecondWindPct.Value = Shinra.Settings.SamuraiSecondWindPct;
            SamuraiInvigoratePct.Value = Shinra.Settings.SamuraiInvigoratePct;
            SamuraiBloodbathPct.Value = Shinra.Settings.SamuraiBloodbathPct;

            #endregion

            #region DoT

            SamuraiHiganbana.Checked = Shinra.Settings.SamuraiHiganbana;
            SamuraiHiganbanaHP.Value = Shinra.Settings.SamuraiHiganbanaHP;

            #endregion

            #region Cooldown

            SamuraiGuren.Checked = Shinra.Settings.SamuraiGuren;

            #endregion

            #region Buff

            SamuraiMeikyo.Checked = Shinra.Settings.SamuraiMeikyo;
            SamuraiHagakure.Checked = Shinra.Settings.SamuraiHagakure;

            #endregion

            #endregion

            #region Summoner

            #region Role

            SummonerDrain.Checked = Shinra.Settings.SummonerDrain;
            SummonerLucidDreaming.Checked = Shinra.Settings.SummonerLucidDreaming;
            SummonerSwiftcast.Checked = Shinra.Settings.SummonerSwiftcast;

            SummonerDrainPct.Value = Shinra.Settings.SummonerDrainPct;
            SummonerLucidDreamingPct.Value = Shinra.Settings.SummonerLucidDreamingPct;

            #endregion

            #region AoE

            SummonerBane.Checked = Shinra.Settings.SummonerBane;

            #endregion

            #region Cooldown

            SummonerShadowFlare.Checked = Shinra.Settings.SummonerShadowFlare;
            SummonerEnkindle.Checked = Shinra.Settings.SummonerEnkindle;
            SummonerTriDisaster.Checked = Shinra.Settings.SummonerTriDisaster;
            SummonerEnkindleBahamut.Checked = Shinra.Settings.SummonerEnkindleBahamut;

            #endregion

            #region Buff

            SummonerRouse.Checked = Shinra.Settings.SummonerRouse;
            SummonerDreadwyrmTrance.Checked = Shinra.Settings.SummonerDreadwyrmTrance;
            SummonerAetherpact.Checked = Shinra.Settings.SummonerAetherpact;
            SummonerSummonBahamut.Checked = Shinra.Settings.SummonerSummonBahamut;

            #endregion

            #region Heal

            SummonerPhysick.Checked = Shinra.Settings.SummonerPhysick;
            SummonerPhysickPct.Value = Shinra.Settings.SummonerPhysickPct;

            #endregion

            #region Pet

            SummonerPet.Text = Convert.ToString(Shinra.Settings.SummonerPet);

            #endregion

            #endregion

            #region Warrior

            #region Role

            WarriorRampart.Checked = Shinra.Settings.WarriorRampart;
            WarriorConvalescence.Checked = Shinra.Settings.WarriorConvalescence;
            WarriorAnticipation.Checked = Shinra.Settings.WarriorAnticipation;
            WarriorReprisal.Checked = Shinra.Settings.WarriorReprisal;
            WarriorAwareness.Checked = Shinra.Settings.WarriorAwareness;

            WarriorRampartPct.Value = Shinra.Settings.WarriorRampartPct;
            WarriorConvalescencePct.Value = Shinra.Settings.WarriorConvalescencePct;
            WarriorAnticipationPct.Value = Shinra.Settings.WarriorAnticipationPct;
            WarriorAwarenessPct.Value = Shinra.Settings.WarriorAwarenessPct;

            #endregion

            #region Damage

            WarriorMaim.Checked = Shinra.Settings.WarriorMaim;
            WarriorStormsEye.Checked = Shinra.Settings.WarriorStormsEye;
            WarriorInnerBeast.Checked = Shinra.Settings.WarriorInnerBeast;
            WarriorFellCleave.Checked = Shinra.Settings.WarriorFellCleave;

            #endregion

            #region AoE

            WarriorOverpower.Checked = Shinra.Settings.WarriorOverpower;
            WarriorSteelCyclone.Checked = Shinra.Settings.WarriorSteelCyclone;
            WarriorDecimate.Checked = Shinra.Settings.WarriorDecimate;

            #endregion

            #region Cooldown

            WarriorOnslaught.Checked = Shinra.Settings.WarriorOnslaught;
            WarriorUpheaval.Checked = Shinra.Settings.WarriorUpheaval;

            #endregion

            #region Buff

            WarriorBerserk.Checked = Shinra.Settings.WarriorBerserk;
            WarriorThrillOfBattle.Checked = Shinra.Settings.WarriorThrillOfBattle;
            WarriorUnchained.Checked = Shinra.Settings.WarriorUnchained;
            WarriorVengeance.Checked = Shinra.Settings.WarriorVengeance;
            WarriorInfuriate.Checked = Shinra.Settings.WarriorInfuriate;
            WarriorEquilibriumTP.Checked = Shinra.Settings.WarriorEquilibriumTP;
            WarriorShakeItOff.Checked = Shinra.Settings.WarriorShakeItOff;
            WarriorInnerRelease.Checked = Shinra.Settings.WarriorInnerRelease;

            WarriorThrillOfBattlePct.Value = Shinra.Settings.WarriorThrillOfBattlePct;
            WarriorVengeancePct.Value = Shinra.Settings.WarriorVengeancePct;
            WarriorEquilibriumTPPct.Value = Shinra.Settings.WarriorEquilibriumTPPct;

            #endregion

            #region Heal

            WarriorEquilibrium.Checked = Shinra.Settings.WarriorEquilibrium;
            WarriorEquilibriumPct.Value = Shinra.Settings.WarriorEquilibriumPct;

            #endregion

            #region Stance

            WarriorStance.Text = Convert.ToString(Shinra.Settings.WarriorStance);

            #endregion

            #endregion

            #region White Mage

            #region Role

            WhiteMageClericStance.Checked = Shinra.Settings.WhiteMageClericStance;
            WhiteMageProtect.Checked = Shinra.Settings.WhiteMageProtect;
            WhiteMageEsuna.Checked = Shinra.Settings.WhiteMageEsuna;
            WhiteMageLucidDreaming.Checked = Shinra.Settings.WhiteMageLucidDreaming;
            WhiteMageSwiftcast.Checked = Shinra.Settings.WhiteMageSwiftcast;

            WhiteMageLucidDreamingPct.Value = Shinra.Settings.WhiteMageLucidDreamingPct;

            #endregion

            #region Buff

            WhiteMagePresenceOfMind.Checked = Shinra.Settings.WhiteMagePresenceOfMind;
            WhiteMageThinAir.Checked = Shinra.Settings.WhiteMageThinAir;

            #endregion

            #region Heal

            WhiteMagePartyHeal.Checked = Shinra.Settings.WhiteMagePartyHeal;
            WhiteMageCure.Checked = Shinra.Settings.WhiteMageCure;
            WhiteMageCureII.Checked = Shinra.Settings.WhiteMageCureII;
            WhiteMageTetragrammaton.Checked = Shinra.Settings.WhiteMageTetragrammaton;
            WhiteMageBenediction.Checked = Shinra.Settings.WhiteMageBenediction;
            WhiteMageRegen.Checked = Shinra.Settings.WhiteMageRegen;
            WhiteMageMedica.Checked = Shinra.Settings.WhiteMageMedica;
            WhiteMageMedicaII.Checked = Shinra.Settings.WhiteMageMedicaII;
            WhiteMageAssize.Checked = Shinra.Settings.WhiteMageAssize;
            WhiteMagePlenary.Checked = Shinra.Settings.WhiteMagePlenary;
            WhiteMageRaise.Checked = Shinra.Settings.WhiteMageRaise;

            WhiteMageCurePct.Value = Shinra.Settings.WhiteMageCurePct;
            WhiteMageCureIIPct.Value = Shinra.Settings.WhiteMageCureIIPct;
            WhiteMageTetragrammatonPct.Value = Shinra.Settings.WhiteMageTetragrammatonPct;
            WhiteMageBenedictionPct.Value = Shinra.Settings.WhiteMageBenedictionPct;
            WhiteMageRegenPct.Value = Shinra.Settings.WhiteMageRegenPct;
            WhiteMageMedicaPct.Value = Shinra.Settings.WhiteMageMedicaPct;
            WhiteMageMedicaIIPct.Value = Shinra.Settings.WhiteMageMedicaIIPct;
            WhiteMageAssizePct.Value = Shinra.Settings.WhiteMageAssizePct;
            WhiteMagePlenaryPct.Value = Shinra.Settings.WhiteMagePlenaryPct;

            #endregion

            #endregion

            #endregion
        }

        private void ShinraForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            HotkeyManager.Register("Shinra Rotation", Helpers.GetHotkey(Shinra.Settings.RotationHotkey),
                                   Helpers.GetModkey(Shinra.Settings.RotationHotkey), hk => Shinra.CycleRotation());
            HotkeyManager.Register("Shinra Tank", Helpers.GetHotkey(Shinra.Settings.TankHotkey),
                                   Helpers.GetModkey(Shinra.Settings.TankHotkey), hk => Shinra.CycleRotation(true));
            Shinra.Settings.WindowLocation = Location;
            Shinra.Settings.Save();
        }

        private void ShinraClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region Main Settings

        #region Rotation

        private void RotationOverlay_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.RotationOverlay = RotationOverlay.Checked;
        }

        private void RotationMode_SelectedValueChanged(object sender, EventArgs e)
        {
            if (RotationMode.Text == @"Smart") Shinra.Settings.RotationMode = Modes.Smart;
            if (RotationMode.Text == @"Single") Shinra.Settings.RotationMode = Modes.Single;
            if (RotationMode.Text == @"Multi") Shinra.Settings.RotationMode = Modes.Multi;
        }

        private void RotationHotkey_KeyDown(object sender, KeyEventArgs e)
        {
            Shinra.Settings.RotationHotkey = RotationHotkey.Hotkey;
        }

        private void TankMode_SelectedValueChanged(object sender, EventArgs e)
        {
            if (TankMode.Text == @"DPS") Shinra.Settings.TankMode = TankModes.DPS;
            if (TankMode.Text == @"Enmity") Shinra.Settings.TankMode = TankModes.Enmity;
        }

        private void TankHotkey_KeyDown(object sender, KeyEventArgs e)
        {
            Shinra.Settings.TankHotkey = TankHotkey.Hotkey;
        }

        #endregion

        #region Chocobo

        private void ChocoboSummon_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SummonChocobo = ChocoboSummon.Checked;
        }

        private void ChocoboStanceDance_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.ChocoboStanceDance = ChocoboStanceDance.Checked;
        }

        private void ChocoboStanceDancePct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.ChocoboStanceDancePct = Convert.ToInt32(ChocoboStanceDancePct.Value);
        }

        private void ChocoboStance_SelectedValueChanged(object sender, EventArgs e)
        {
            if (ChocoboStance.Text == @"Free") Shinra.Settings.ChocoboStance = Stances.Free;
            if (ChocoboStance.Text == @"Attacker") Shinra.Settings.ChocoboStance = Stances.Attacker;
            if (ChocoboStance.Text == @"Healer") Shinra.Settings.ChocoboStance = Stances.Healer;
            if (ChocoboStance.Text == @"Defender") Shinra.Settings.ChocoboStance = Stances.Defender;
        }

        #endregion

        #region Item

        private void UsePotion_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.UsePotion = UsePotion.Checked;
        }

        private void UsePotionPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.UsePotionPct = Convert.ToInt32(UsePotionPct.Value);
        }

        #endregion

        #region Misc

        private void DefaultRoutine_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DefaultRoutine = DefaultRoutine.Checked;
        }

        #endregion

        #endregion

        #region Job Settings

        #region Astrologian

        #region Role

        private void AstrologianClericStance_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianClericStance = AstrologianClericStance.Checked;
        }

        private void AstrologianProtect_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianProtect = AstrologianProtect.Checked;
        }

        private void AstrologianEsuna_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianEsuna = AstrologianEsuna.Checked;
        }

        private void AstrologianLucidDreaming_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianLucidDreaming = AstrologianLucidDreaming.Checked;
        }

        private void AstrologianSwiftcast_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianSwiftcast = AstrologianSwiftcast.Checked;
        }

        private void AstrologianLucidDreamingPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianLucidDreamingPct = Convert.ToInt32(AstrologianLucidDreamingPct.Value);
        }

        #endregion

        #region AoE

        private void AstrologianEarthlyStar_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianEarthlyStar = AstrologianEarthlyStar.Checked;
        }

        #endregion

        #region Heal

        private void AstrologianPartyHeal_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianPartyHeal = AstrologianPartyHeal.Checked;
        }

        private void AstrologianBenefic_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianBenefic = AstrologianBenefic.Checked;
        }

        private void AstrologianBeneficII_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianBeneficII = AstrologianBeneficII.Checked;
        }

        private void AstrologianEssDignity_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianEssDignity = AstrologianEssDignity.Checked;
        }

        private void AstrologianAspBenefic_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianAspBenefic = AstrologianAspBenefic.Checked;
        }

        private void AstrologianHelios_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianHelios = AstrologianHelios.Checked;
        }

        private void AstrologianAspHelios_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianAspHelios = AstrologianAspHelios.Checked;
        }

        private void AstrologianAscend_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianAscend = AstrologianAscend.Checked;
        }

        private void AstrologianBeneficPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianBeneficPct = Convert.ToInt32(AstrologianBeneficPct.Value);
        }

        private void AstrologianBeneficIIPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianBeneficIIPct = Convert.ToInt32(AstrologianBeneficIIPct.Value);
        }

        private void AstrologianEssDignityPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianEssDignityPct = Convert.ToInt32(AstrologianEssDignityPct.Value);
        }

        private void AstrologianAspBeneficPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianAspBeneficPct = Convert.ToInt32(AstrologianAspBeneficPct.Value);
        }

        private void AstrologianHeliosPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianHeliosPct = Convert.ToInt32(AstrologianHeliosPct.Value);
        }

        private void AstrologianAspHeliosPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianAspHeliosPct = Convert.ToInt32(AstrologianAspHeliosPct.Value);
        }

        #endregion

        #region Card

        private void AstrologianDraw_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianDraw = AstrologianDraw.Checked;
        }

        private void AstrologianSleeveDraw_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.AstrologianSleeveDraw = AstrologianSleeveDraw.Checked;
        }

        #endregion

        #region Sect

        private void AstrologianSect_SelectedValueChanged(object sender, EventArgs e)
        {
            if (AstrologianSect.Text == @"None") Shinra.Settings.AstrologianSect = AstrologianSects.None;
            if (AstrologianSect.Text == @"Diurnal") Shinra.Settings.AstrologianSect = AstrologianSects.Diurnal;
            if (AstrologianSect.Text == @"Nocturnal") Shinra.Settings.AstrologianSect = AstrologianSects.Nocturnal;
        }

        #endregion

        #endregion

        #region Bard

        #region Role

        private void BardSecondWind_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.BardSecondWind = BardSecondWind.Checked;
        }

        private void BardPeloton_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.BardPeloton = BardPeloton.Checked;
        }

        private void BardInvigorate_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.BardInvigorate = BardInvigorate.Checked;
        }

        private void BardTactician_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.BardTactician = BardTactician.Checked;
        }

        private void BardSecondWindPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.BardSecondWindPct = Convert.ToInt32(BardSecondWindPct.Value);
        }

        private void BardInvigoratePct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.BardInvigoratePct = Convert.ToInt32(BardInvigoratePct.Value);
        }

        private void BardTacticianPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.BardTacticianPct = Convert.ToInt32(BardTacticianPct.Value);
        }

        #endregion

        #region Cooldown

        private void BardSongs_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.BardSongs = BardSongs.Checked;
        }

        private void BardSidewinder_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.BardSidewinder = BardSidewinder.Checked;
        }

        #endregion

        #region Buff

        private void BardRagingStrikes_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.BardRagingStrikes = BardRagingStrikes.Checked;
        }

        private void BardBarrage_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.BardBarrage = BardBarrage.Checked;
        }

        private void BardBattleVoice_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.BardBattleVoice = BardBattleVoice.Checked;
        }

        #endregion

        #endregion

        #region Black Mage

        #region Role

        private void BlackMageDrain_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.BlackMageDrain = BlackMageDrain.Checked;
        }

        private void BlackMageLucidDreaming_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.BlackMageLucidDreaming = BlackMageLucidDreaming.Checked;
        }

        private void BlackMageSwiftcast_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.BlackMageSwiftcast = BlackMageSwiftcast.Checked;
        }

        private void BlackMageDrainPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.BlackMageDrainPct = Convert.ToInt32(BlackMageDrainPct.Value);
        }

        private void BlackMageLucidDreamingPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.BlackMageLucidDreamingPct = Convert.ToInt32(BlackMageLucidDreamingPct.Value);
        }

        #endregion

        #endregion

        #region Dark Knight

        #region Role

        private void DarkKnightRampart_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightRampart = DarkKnightRampart.Checked;
        }

        private void DarkKnightConvalescence_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightConvalescence = DarkKnightConvalescence.Checked;
        }

        private void DarkKnightAnticipation_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightAnticipation = DarkKnightAnticipation.Checked;
        }

        private void DarkKnightReprisal_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightReprisal = DarkKnightReprisal.Checked;
        }

        private void DarkKnightAwareness_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightAwareness = DarkKnightAwareness.Checked;
        }

        private void DarkKnightRampartPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightRampartPct = Convert.ToInt32(DarkKnightRampartPct.Value);
        }

        private void DarkKnightConvalescencePct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightConvalescencePct = Convert.ToInt32(DarkKnightConvalescencePct.Value);
        }

        private void DarkKnightAnticipationPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightAnticipationPct = Convert.ToInt32(DarkKnightAnticipationPct.Value);
        }

        private void DarkKnightAwarenessPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightAwarenessPct = Convert.ToInt32(DarkKnightAwarenessPct.Value);
        }

        #endregion

        #region Damage

        private void DarkKnightBloodspiller_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightBloodspiller = DarkKnightBloodspiller.Checked;
        }

        #endregion

        #region AoE

        private void DarkKnightQuietus_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightQuietus = DarkKnightQuietus.Checked;
        }

        #endregion

        #region Cooldown

        private void DarkKnightSaltedEarth_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightSaltedEarth = DarkKnightSaltedEarth.Checked;
        }

        private void DarkKnightPlunge_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightPlunge = DarkKnightPlunge.Checked;
        }

        private void DarkKnightCarveAndSpit_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightCarveAndSpit = DarkKnightCarveAndSpit.Checked;
        }

        #endregion

        #region Buff

        private void DarkKnightBloodWeapon_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightBloodWeapon = DarkKnightBloodWeapon.Checked;
        }

        private void DarkKnightBloodPrice_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightBloodPrice = DarkKnightBloodPrice.Checked;
        }

        private void DarkKnightShadowWall_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightShadowWall = DarkKnightShadowWall.Checked;
        }

        private void DarkKnightLivingDead_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightLivingDead = DarkKnightLivingDead.Checked;
        }

        private void DarkKnightDelirium_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightDelirium = DarkKnightDelirium.Checked;
        }

        private void DarkKnightBlackestNight_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightBlackestNight = DarkKnightBlackestNight.Checked;
        }

        private void DarkKnightBloodPricePct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightBloodPricePct = Convert.ToInt32(DarkKnightBloodPricePct.Value);
        }

        private void DarkKnightShadowWallPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightShadowWallPct = Convert.ToInt32(DarkKnightShadowWallPct.Value);
        }

        private void DarkKnightLivingDeadPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightLivingDeadPct = Convert.ToInt32(DarkKnightLivingDeadPct.Value);
        }

        private void DarkKnightBlackestNightPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightBlackestNightPct = Convert.ToInt32(DarkKnightBlackestNightPct.Value);
        }

        #endregion

        #region Dark Arts

        private void DarkKnightSouleaterArts_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightSouleaterArts = DarkKnightSouleaterArts.Checked;
        }

        private void DarkKnightAbyssalArts_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightAbyssalArts = DarkKnightAbyssalArts.Checked;
        }

        private void DarkKnightCarveArts_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightCarveArts = DarkKnightCarveArts.Checked;
        }

        private void DarkKnightQuietusArts_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightQuietusArts = DarkKnightQuietusArts.Checked;
        }

        private void DarkKnightBloodspillerArts_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightBloodspillerArts = DarkKnightBloodspillerArts.Checked;
        }

        #endregion

        #region Aura

        private void DarkKnightGrit_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightGrit = DarkKnightGrit.Checked;
        }

        private void DarkKnightDarkside_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DarkKnightDarkside = DarkKnightDarkside.Checked;
        }

        #endregion

        #endregion

        #region Dragoon

        #region Role

        private void DragoonSecondWind_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DragoonSecondWind = DragoonSecondWind.Checked;
        }

        private void DragoonInvigorate_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DragoonInvigorate = DragoonInvigorate.Checked;
        }

        private void DragoonBloodbath_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DragoonBloodbath = DragoonBloodbath.Checked;
        }

        private void DragoonTrueNorth_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DragoonTrueNorth = DragoonTrueNorth.Checked;
        }

        private void DragoonSecondWindPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DragoonSecondWindPct = Convert.ToInt32(DragoonSecondWindPct.Value);
        }

        private void DragoonInvigoratePct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DragoonInvigoratePct = Convert.ToInt32(DragoonInvigoratePct.Value);
        }

        private void DragoonBloodbathPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DragoonBloodbathPct = Convert.ToInt32(DragoonBloodbathPct.Value);
        }

        #endregion

        #region Cooldown

        private void DragoonJump_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DragoonJump = DragoonJump.Checked;
        }

        private void DragoonSpineshatter_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DragoonSpineshatter = DragoonSpineshatter.Checked;
        }

        private void DragoonDragonfire_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DragoonDragonfire = DragoonDragonfire.Checked;
        }

        private void DragoonGeirskogul_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DragoonGeirskogul = DragoonGeirskogul.Checked;
        }

        private void DragoonMirage_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DragoonMirage = DragoonMirage.Checked;
        }

        #endregion

        #region Buff

        private void DragoonLifeSurge_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DragoonLifeSurge = DragoonLifeSurge.Checked;
        }

        private void DragoonBloodForBlood_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DragoonBloodForBlood = DragoonBloodForBlood.Checked;
        }

        private void DragoonBattleLitany_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DragoonBattleLitany = DragoonBattleLitany.Checked;
        }

        private void DragoonBloodOfTheDragon_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.DragoonBloodOfTheDragon = DragoonBloodOfTheDragon.Checked;
        }

        #endregion

        #endregion

        #region Monk

        #region Role

        private void MonkSecondWind_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkSecondWind = MonkSecondWind.Checked;
        }

        private void MonkInvigorate_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkInvigorate = MonkInvigorate.Checked;
        }

        private void MonkBloodbath_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkBloodbath = MonkBloodbath.Checked;
        }

        private void MonkTrueNorth_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkTrueNorth = MonkTrueNorth.Checked;
        }

        private void MonkSecondWindPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkSecondWindPct = Convert.ToInt32(MonkSecondWindPct.Value);
        }

        private void MonkInvigoratePct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkInvigoratePct = Convert.ToInt32(MonkInvigoratePct.Value);
        }

        private void MonkBloodbathPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkBloodbathPct = Convert.ToInt32(MonkBloodbathPct.Value);
        }

        #endregion

        #region DoT

        private void MonkDemolish_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkDemolish = MonkDemolish.Checked;
        }

        private void MonkDemolishHP_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkDemolishHP = Convert.ToInt32(MonkDemolishHP.Value);
        }

        #endregion

        #region Cooldown

        private void MonkShoulderTackle_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkShoulderTackle = MonkShoulderTackle.Checked;
        }

        private void MonkSteelPeak_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkSteelPeak = MonkSteelPeak.Checked;
        }

        private void MonkHowlingFist_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkHowlingFist = MonkHowlingFist.Checked;
        }

        private void MonkForbiddenChakra_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkForbiddenChakra = MonkForbiddenChakra.Checked;
        }

        private void MonkElixirField_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkElixirField = MonkElixirField.Checked;
        }

        private void MonkFireTackle_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkFireTackle = MonkFireTackle.Checked;
        }

        #endregion

        #region Buff

        private void MonkInternalRelease_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkInternalRelease = MonkInternalRelease.Checked;
        }

        private void MonkPerfectBalance_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkPerfectBalance = MonkPerfectBalance.Checked;
        }

        private void MonkRiddleOfFire_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkRiddleOfFire = MonkRiddleOfFire.Checked;
        }

        private void MonkBrotherhood_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.MonkBrotherhood = MonkBrotherhood.Checked;
        }

        #endregion

        #region Fists

        private void MonkFist_SelectedValueChanged(object sender, EventArgs e)
        {
            if (MonkFist.Text == @"None") Shinra.Settings.MonkFist = MonkFists.None;
            if (MonkFist.Text == @"Earth") Shinra.Settings.MonkFist = MonkFists.Earth;
            if (MonkFist.Text == @"Wind") Shinra.Settings.MonkFist = MonkFists.Wind;
            if (MonkFist.Text == @"Fire") Shinra.Settings.MonkFist = MonkFists.Fire;
        }

        #endregion

        #endregion

        #region Paladin

        #region Role

        private void PaladinRampart_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinRampart = PaladinRampart.Checked;
        }

        private void PaladinConvalescence_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinConvalescence = PaladinConvalescence.Checked;
        }

        private void PaladinAnticipation_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinAnticipation = PaladinAnticipation.Checked;
        }

        private void PaladinReprisal_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinReprisal = PaladinReprisal.Checked;
        }

        private void PaladinAwareness_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinAwareness = PaladinAwareness.Checked;
        }

        private void PaladinRampartPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinRampartPct = Convert.ToInt32(PaladinRampartPct.Value);
        }

        private void PaladinConvalescencePct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinConvalescencePct = Convert.ToInt32(PaladinConvalescencePct.Value);
        }

        private void PaladinAnticipationPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinAnticipationPct = Convert.ToInt32(PaladinAnticipationPct.Value);
        }

        private void PaladinAwarenessPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinAwarenessPct = Convert.ToInt32(PaladinAwarenessPct.Value);
        }

        #endregion

        #region Damage

        private void PaladinGoringBlade_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinGoringBlade = PaladinGoringBlade.Checked;
        }

        #endregion

        #region AoE

        private void PaladinFlash_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinFlash = PaladinFlash.Checked;
        }

        private void PaladinTotalEclipse_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinTotalEclipse = PaladinTotalEclipse.Checked;
        }

        #endregion

        #region Cooldown

        private void PaladinShieldSwipe_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinShieldSwipe = PaladinShieldSwipe.Checked;
        }

        private void PaladinSpiritsWithin_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinSpiritsWithin = PaladinSpiritsWithin.Checked;
        }

        private void PaladinCircleOfScorn_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinCircleOfScorn = PaladinCircleOfScorn.Checked;
        }

        private void PaladinRequiescat_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinRequiescat = PaladinRequiescat.Checked;
        }

        #endregion

        #region Buff

        private void PaladinFightOrFlight_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinFightOrFlight = PaladinFightOrFlight.Checked;
        }

        private void PaladinBulwark_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinBulwark = PaladinBulwark.Checked;
        }

        private void PaladinSentinel_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinSentinel = PaladinSentinel.Checked;
        }

        private void PaladinHallowedGround_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinHallowedGround = PaladinHallowedGround.Checked;
        }

        private void PaladinSheltron_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinSheltron = PaladinSheltron.Checked;
        }

        private void PaladinBulwarkPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinBulwarkPct = Convert.ToInt32(PaladinBulwarkPct.Value);
        }

        private void PaladinSentinelPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinSentinelPct = Convert.ToInt32(PaladinSentinelPct.Value);
        }

        private void PaladinHallowedGroundPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinHallowedGroundPct = Convert.ToInt32(PaladinHallowedGroundPct.Value);
        }

        #endregion

        #region Heal

        private void PaladinClemency_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinClemency = PaladinClemency.Checked;
        }

        private void PaladinClemencyPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinClemencyPct = Convert.ToInt32(PaladinClemencyPct.Value);
        }

        #endregion

        #region Oath

        private void PaladinOath_SelectedValueChanged(object sender, EventArgs e)
        {
            if (PaladinOath.Text == @"None") Shinra.Settings.PaladinOath = PaladinOaths.None;
            if (PaladinOath.Text == @"Shield") Shinra.Settings.PaladinOath = PaladinOaths.Shield;
            if (PaladinOath.Text == @"Sword") Shinra.Settings.PaladinOath = PaladinOaths.Sword;
        }

        #endregion

        #endregion

        #region Red Mage

        #region Role

        private void RedMageDrain_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.RedMageDrain = RedMageDrain.Checked;
        }

        private void RedMageLucidDreaming_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.RedMageLucidDreaming = RedMageLucidDreaming.Checked;
        }

        private void RedMageSwiftcast_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.RedMageSwiftcast = RedMageSwiftcast.Checked;
        }

        private void RedMageDrainPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.RedMageDrainPct = Convert.ToInt32(RedMageDrainPct.Value);
        }

        private void RedMageLucidDreamingPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.RedMageLucidDreamingPct = Convert.ToInt32(RedMageLucidDreamingPct.Value);
        }

        #endregion

        #region Cooldown

        private void RedMageCorpsACorps_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.RedMageCorpsACorps = RedMageCorpsACorps.Checked;
        }

        #endregion

        #region Buff

        private void RedMageEmbolden_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.RedMageEmbolden = RedMageEmbolden.Checked;
        }

        private void RedMageManafication_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.RedMageManafication = RedMageManafication.Checked;
        }

        #endregion

        #region Heal

        private void RedMageVercure_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.RedMageVercure = RedMageVercure.Checked;
        }

        private void RedMageVercurePct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.RedMageVercurePct = Convert.ToInt32(RedMageVercurePct.Value);
        }

        #endregion

        #endregion

        #region Samurai

        #region Role

        private void SamuraiSecondWind_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SamuraiSecondWind = SamuraiSecondWind.Checked;
        }

        private void SamuraiInvigorate_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SamuraiInvigorate = SamuraiInvigorate.Checked;
        }

        private void SamuraiBloodbath_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SamuraiBloodbath = SamuraiBloodbath.Checked;
        }

        private void SamuraiTrueNorth_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SamuraiTrueNorth = SamuraiTrueNorth.Checked;
        }

        private void SamuraiSecondWindPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SamuraiSecondWindPct = Convert.ToInt32(SamuraiSecondWindPct.Value);
        }

        private void SamuraiInvigoratePct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SamuraiInvigoratePct = Convert.ToInt32(SamuraiInvigoratePct.Value);
        }

        private void SamuraiBloodbathPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SamuraiBloodbathPct = Convert.ToInt32(SamuraiBloodbathPct.Value);
        }

        #endregion

        #region DoT

        private void SamuraiHiganbana_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SamuraiHiganbana = SamuraiHiganbana.Checked;
        }

        private void SamuraiHiganbanaHP_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SamuraiHiganbanaHP = Convert.ToInt32(SamuraiHiganbanaHP.Value);
        }

        #endregion

        #region Cooldown

        private void SamuraiGuren_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SamuraiGuren = SamuraiGuren.Checked;
        }

        #endregion

        #region Buff

        private void SamuraiMeikyo_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SamuraiMeikyo = SamuraiMeikyo.Checked;
        }

        private void SamuraiHagakure_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SamuraiHagakure = SamuraiHagakure.Checked;
        }

        #endregion

        #endregion

        #region Summoner

        #region Role

        private void SummonerDrain_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SummonerDrain = SummonerDrain.Checked;
        }

        private void SummonerLucidDreaming_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SummonerLucidDreaming = SummonerLucidDreaming.Checked;
        }

        private void SummonerSwiftcast_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SummonerSwiftcast = SummonerSwiftcast.Checked;
        }

        private void SummonerDrainPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SummonerDrainPct = Convert.ToInt32(SummonerDrainPct.Value);
        }

        private void SummonerLucidDreamingPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SummonerLucidDreamingPct = Convert.ToInt32(SummonerLucidDreamingPct.Value);
        }

        #endregion

        #region AoE

        private void SummonerBane_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SummonerBane = SummonerBane.Checked;
        }

        #endregion

        #region Cooldown

        private void SummonerShadowFlare_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SummonerShadowFlare = SummonerShadowFlare.Checked;
        }

        private void SummonerEnkindle_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SummonerEnkindle = SummonerEnkindle.Checked;
        }

        private void SummonerTriDisaster_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SummonerTriDisaster = SummonerTriDisaster.Checked;
        }

        private void SummonerEnkindleBahamut_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SummonerEnkindleBahamut = SummonerEnkindleBahamut.Checked;
        }

        #endregion

        #region Buff

        private void SummonerRouse_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SummonerRouse = SummonerRouse.Checked;
        }

        private void SummonerDreadwyrmTrance_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SummonerDreadwyrmTrance = SummonerDreadwyrmTrance.Checked;
        }

        private void SummonerAetherpact_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SummonerAetherpact = SummonerAetherpact.Checked;
        }

        private void SummonerSummonBahamut_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SummonerSummonBahamut = SummonerSummonBahamut.Checked;
        }

        #endregion

        #region Heal

        private void SummonerPhysick_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SummonerPhysick = SummonerPhysick.Checked;
        }

        private void SummonerPhysickPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.SummonerPhysickPct = Convert.ToInt32(SummonerPhysickPct.Value);
        }

        #endregion

        #region Pet

        private void SummonerPet_SelectedValueChanged(object sender, EventArgs e)
        {
            if (SummonerPet.Text == @"None") Shinra.Settings.SummonerPet = SummonerPets.None;
            if (SummonerPet.Text == @"Garuda") Shinra.Settings.SummonerPet = SummonerPets.Garuda;
            if (SummonerPet.Text == @"Titan") Shinra.Settings.SummonerPet = SummonerPets.Titan;
            if (SummonerPet.Text == @"Ifrit") Shinra.Settings.SummonerPet = SummonerPets.Ifrit;
        }

        #endregion

        #endregion

        #region Warrior

        #region Role

        private void WarriorRampart_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorRampart = WarriorRampart.Checked;
        }

        private void WarriorConvalescence_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorConvalescence = WarriorConvalescence.Checked;
        }

        private void WarriorAnticipation_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorAnticipation = WarriorAnticipation.Checked;
        }

        private void WarriorReprisal_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorReprisal = WarriorReprisal.Checked;
        }

        private void WarriorAwareness_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorAwareness = WarriorAwareness.Checked;
        }

        private void WarriorRampartPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorRampartPct = Convert.ToInt32(WarriorRampartPct.Value);
        }

        private void WarriorConvalescencePct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorConvalescencePct = Convert.ToInt32(WarriorConvalescencePct.Value);
        }

        private void WarriorAnticipationPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorAnticipationPct = Convert.ToInt32(WarriorAnticipationPct.Value);
        }

        private void WarriorAwarenessPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorAwarenessPct = Convert.ToInt32(WarriorAwarenessPct.Value);
        }

        #endregion

        #region Damage

        private void WarriorMaim_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorMaim = WarriorMaim.Checked;
        }

        private void WarriorStormsEye_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorStormsEye = WarriorStormsEye.Checked;
        }

        private void WarriorInnerBeast_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorInnerBeast = WarriorInnerBeast.Checked;
        }

        private void WarriorFellCleave_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorFellCleave = WarriorFellCleave.Checked;
        }

        #endregion

        #region AoE

        private void WarriorOverpower_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorOverpower = WarriorOverpower.Checked;
        }

        private void WarriorSteelCyclone_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorSteelCyclone = WarriorSteelCyclone.Checked;
        }

        private void WarriorDecimate_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorDecimate = WarriorDecimate.Checked;
        }

        #endregion

        #region Cooldown

        private void WarriorOnslaught_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorOnslaught = WarriorOnslaught.Checked;
        }

        private void WarriorUpheaval_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorUpheaval = WarriorUpheaval.Checked;
        }

        #endregion

        #region Buff

        private void WarriorBerserk_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorBerserk = WarriorBerserk.Checked;
        }

        private void WarriorThrillOfBattle_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorThrillOfBattle = WarriorThrillOfBattle.Checked;
        }

        private void WarriorUnchained_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorUnchained = WarriorUnchained.Checked;
        }

        private void WarriorVengeance_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorVengeance = WarriorVengeance.Checked;
        }

        private void WarriorInfuriate_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorInfuriate = WarriorInfuriate.Checked;
        }

        private void WarriorEquilibriumTP_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorEquilibriumTP = WarriorEquilibriumTP.Checked;
        }

        private void WarriorShakeItOff_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorShakeItOff = WarriorShakeItOff.Checked;
        }

        private void WarriorInnerRelease_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorInnerRelease = WarriorInnerRelease.Checked;
        }

        private void WarriorThrillOfBattlePct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorThrillOfBattlePct = Convert.ToInt32(WarriorThrillOfBattlePct.Value);
        }

        private void WarriorVengeancePct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorVengeancePct = Convert.ToInt32(WarriorVengeancePct.Value);
        }

        private void WarriorEquilibriumTPPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorEquilibriumTPPct = Convert.ToInt32(WarriorEquilibriumTPPct.Value);
        }

        #endregion

        #region Heal

        private void WarriorEquilibrium_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorEquilibrium = WarriorEquilibrium.Checked;
        }

        private void WarriorEquilibriumPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WarriorEquilibriumPct = Convert.ToInt32(WarriorEquilibriumPct.Value);
        }

        #endregion

        #region Stance

        private void WarriorStance_SelectedValueChanged(object sender, EventArgs e)
        {
            if (WarriorStance.Text == @"None") Shinra.Settings.WarriorStance = WarriorStances.None;
            if (WarriorStance.Text == @"Defiance") Shinra.Settings.WarriorStance = WarriorStances.Defiance;
            if (WarriorStance.Text == @"Deliverance") Shinra.Settings.WarriorStance = WarriorStances.Deliverance;
        }

        #endregion

        #endregion

        #region White Mage

        #region Role

        private void WhiteMageClericStance_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageClericStance = WhiteMageClericStance.Checked;
        }

        private void WhiteMageProtect_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageProtect = WhiteMageProtect.Checked;
        }

        private void WhiteMageEsuna_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageEsuna = WhiteMageEsuna.Checked;
        }

        private void WhiteMageLucidDreaming_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageLucidDreaming = WhiteMageLucidDreaming.Checked;
        }

        private void WhiteMageSwiftcast_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageSwiftcast = WhiteMageSwiftcast.Checked;
        }

        private void WhiteMageLucidDreamingPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageLucidDreamingPct = Convert.ToInt32(WhiteMageLucidDreamingPct.Value);
        }

        #endregion

        #region Buff

        private void WhiteMagePresenceOfMind_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMagePresenceOfMind = WhiteMagePresenceOfMind.Checked;
        }

        private void WhiteMageThinAir_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageThinAir = WhiteMageThinAir.Checked;
        }

        #endregion

        #region Heal

        private void WhiteMagePartyHeal_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMagePartyHeal = WhiteMagePartyHeal.Checked;
        }

        private void WhiteMageCure_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageCure = WhiteMageCure.Checked;
        }

        private void WhiteMageCureII_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageCureII = WhiteMageCureII.Checked;
        }

        private void WhiteMageTetragrammaton_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageTetragrammaton = WhiteMageTetragrammaton.Checked;
        }

        private void WhiteMageBenediction_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageBenediction = WhiteMageBenediction.Checked;
        }

        private void WhiteMageRegen_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageRegen = WhiteMageRegen.Checked;
        }

        private void WhiteMageMedica_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageMedica = WhiteMageMedica.Checked;
        }

        private void WhiteMageMedicaII_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageMedicaII = WhiteMageMedicaII.Checked;
        }

        private void WhiteMageAssize_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageAssize = WhiteMageAssize.Checked;
        }

        private void WhiteMagePlenary_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMagePlenary = WhiteMagePlenary.Checked;
        }

        private void WhiteMageRaise_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageRaise = WhiteMageRaise.Checked;
        }

        private void WhiteMageCurePct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageCurePct = Convert.ToInt32(WhiteMageCurePct.Value);
        }

        private void WhiteMageCureIIPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageCureIIPct = Convert.ToInt32(WhiteMageCureIIPct.Value);
        }

        private void WhiteMageTetragrammatonPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageTetragrammatonPct = Convert.ToInt32(WhiteMageTetragrammatonPct.Value);
        }

        private void WhiteMageBenedictionPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageBenedictionPct = Convert.ToInt32(WhiteMageBenedictionPct.Value);
        }

        private void WhiteMageRegenPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageRegenPct = Convert.ToInt32(WhiteMageRegenPct.Value);
        }

        private void WhiteMageMedicaPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageMedicaPct = Convert.ToInt32(WhiteMageMedicaPct.Value);
        }

        private void WhiteMageMedicaIIPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageMedicaIIPct = Convert.ToInt32(WhiteMageMedicaIIPct.Value);
        }

        private void WhiteMageAssizePct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMageAssizePct = Convert.ToInt32(WhiteMageAssizePct.Value);
        }

        private void WhiteMagePlenaryPct_ValueChanged(object sender, EventArgs e)
        {
            Shinra.Settings.WhiteMagePlenaryPct = Convert.ToInt32(WhiteMagePlenaryPct.Value);
        }

        #endregion

        #endregion

        #endregion
    }
}