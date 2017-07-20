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
            Location = Shinra.Settings.WindowLocation;

            #region Main Settings

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

            #region Rotation

            RotationOverlay.Checked = Shinra.Settings.RotationOverlay;
            RotationMode.Text = Convert.ToString(Shinra.Settings.RotationMode);

            var kc = new KeysConverter();
            RotationKey.Text = kc.ConvertToString(Shinra.Settings.RotationKey);
            RotationModKey.Text = kc.ConvertToString(Shinra.Settings.RotationModKey);

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

            #endregion

            #region Buff

            DragoonLifeSurge.Checked = Shinra.Settings.DragoonLifeSurge;
            DragoonBloodForBlood.Checked = Shinra.Settings.DragoonBloodForBlood;
            DragoonBattleLitany.Checked = Shinra.Settings.DragoonBattleLitany;
            DragoonBloodOfTheDragon.Checked = Shinra.Settings.DragoonBloodOfTheDragon;

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
            PaladinRoyalAuthority.Checked = Shinra.Settings.PaladinRoyalAuthority;
            PaladinHolySpirit.Checked = Shinra.Settings.PaladinHolySpirit;

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

            #region Heal

            SummonerPhysick.Checked = Shinra.Settings.SummonerPhysick;
            SummonerPhysickPct.Value = Shinra.Settings.SummonerPhysickPct;

            #endregion

            #region Pet

            SummonerPet.Text = Convert.ToString(Shinra.Settings.SummonerPet);

            #endregion

            #endregion

            #endregion
        }

        private void ShinraForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            HotkeyManager.Register("Shinra Rotation", Shinra.Settings.RotationKey, Shinra.Settings.RotationModKey,
                                   hk => Shinra.CycleRotation());
            Shinra.Settings.WindowLocation = Location;
            Shinra.Settings.Save();
        }

        private void ShinraClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region Main Settings

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

        private void CycleRotationKey_SelectedValueChanged(object sender, EventArgs e)
        {
            if (RotationKey.Text == @"None") Shinra.Settings.RotationKey = Keys.None;
            if (RotationKey.Text == @"Oemtilde") Shinra.Settings.RotationKey = Keys.Oemtilde;
            if (RotationKey.Text == @"XButton1") Shinra.Settings.RotationKey = Keys.XButton1;
            if (RotationKey.Text == @"XButton2") Shinra.Settings.RotationKey = Keys.XButton2;
            if (RotationKey.Text == @"Space") Shinra.Settings.RotationKey = Keys.Space;
            if (RotationKey.Text == @"A") Shinra.Settings.RotationKey = Keys.A;
            if (RotationKey.Text == @"B") Shinra.Settings.RotationKey = Keys.B;
            if (RotationKey.Text == @"C") Shinra.Settings.RotationKey = Keys.C;
            if (RotationKey.Text == @"D") Shinra.Settings.RotationKey = Keys.D;
            if (RotationKey.Text == @"E") Shinra.Settings.RotationKey = Keys.E;
            if (RotationKey.Text == @"F") Shinra.Settings.RotationKey = Keys.F;
            if (RotationKey.Text == @"G") Shinra.Settings.RotationKey = Keys.G;
            if (RotationKey.Text == @"H") Shinra.Settings.RotationKey = Keys.H;
            if (RotationKey.Text == @"I") Shinra.Settings.RotationKey = Keys.I;
            if (RotationKey.Text == @"J") Shinra.Settings.RotationKey = Keys.J;
            if (RotationKey.Text == @"K") Shinra.Settings.RotationKey = Keys.K;
            if (RotationKey.Text == @"L") Shinra.Settings.RotationKey = Keys.L;
            if (RotationKey.Text == @"M") Shinra.Settings.RotationKey = Keys.M;
            if (RotationKey.Text == @"N") Shinra.Settings.RotationKey = Keys.N;
            if (RotationKey.Text == @"O") Shinra.Settings.RotationKey = Keys.O;
            if (RotationKey.Text == @"P") Shinra.Settings.RotationKey = Keys.P;
            if (RotationKey.Text == @"Q") Shinra.Settings.RotationKey = Keys.Q;
            if (RotationKey.Text == @"R") Shinra.Settings.RotationKey = Keys.R;
            if (RotationKey.Text == @"S") Shinra.Settings.RotationKey = Keys.S;
            if (RotationKey.Text == @"T") Shinra.Settings.RotationKey = Keys.T;
            if (RotationKey.Text == @"U") Shinra.Settings.RotationKey = Keys.U;
            if (RotationKey.Text == @"V") Shinra.Settings.RotationKey = Keys.V;
            if (RotationKey.Text == @"W") Shinra.Settings.RotationKey = Keys.W;
            if (RotationKey.Text == @"X") Shinra.Settings.RotationKey = Keys.X;
            if (RotationKey.Text == @"Y") Shinra.Settings.RotationKey = Keys.Y;
            if (RotationKey.Text == @"Z") Shinra.Settings.RotationKey = Keys.Z;
        }

        private void CycleRotationModKey_SelectedValueChanged(object sender, EventArgs e)
        {
            if (RotationModKey.Text == @"None") Shinra.Settings.RotationModKey = System.Windows.Input.ModifierKeys.None;
            if (RotationModKey.Text == @"Alt") Shinra.Settings.RotationModKey = System.Windows.Input.ModifierKeys.Alt;
            if (RotationModKey.Text == @"Control") Shinra.Settings.RotationModKey = System.Windows.Input.ModifierKeys.Control;
            if (RotationModKey.Text == @"Shift") Shinra.Settings.RotationModKey = System.Windows.Input.ModifierKeys.Shift;
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

        private void PaladinRoyalAuthority_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinRoyalAuthority = PaladinRoyalAuthority.Checked;
        }

        private void PaladinHolySpirit_CheckedChanged(object sender, EventArgs e)
        {
            Shinra.Settings.PaladinHolySpirit = PaladinHolySpirit.Checked;
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

        #endregion
    }
}