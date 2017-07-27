using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.AClasses;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ShinraCo.Rotations;
using ShinraCo.Settings;
using ShinraCo.Settings.Forms;
using ShinraCo.Spells;
using TreeSharp;

namespace ShinraCo
{
    public class Shinra : CombatRoutine
    {
        #region Overrides

        public sealed override string Name => "Shinra";
        public sealed override float PullRange => 15f;
        public sealed override bool WantButton => true;

        public sealed override void Initialize()
        {
            Logging.Write(Colors.GreenYellow, $@"[Shinra] Loaded Version: {Helpers.GetLocalVersion()}");
            HotkeyManager.Register("Shinra Rotation", Helpers.GetHotkey(Settings.RotationHotkey),
                                   Helpers.GetModkey(Settings.RotationHotkey), hk => CycleRotation());
            HotkeyManager.Register("Shinra Tank", Helpers.GetHotkey(Settings.TankHotkey),
                                   Helpers.GetModkey(Settings.TankHotkey), hk => CycleRotation(true));
        }

        public sealed override void Pulse()
        {
            var _class = CurrentClass;
        }

        public sealed override void ShutDown()
        {
            Logging.Write(Colors.GreenYellow, @"[Shinra] Shutting down...");
            HotkeyManager.Unregister("Shinra Rotation");
            HotkeyManager.Unregister("Shinra Tank");
        }

        #endregion

        #region Settings

        private Form _configForm;

        public static ShinraSettings Settings = ShinraSettings.Instance;

        public sealed override void OnButtonPress()
        {
            if (_configForm == null || _configForm.IsDisposed || _configForm.Disposing)
            {
                _configForm = new ShinraForm();
            }
            _configForm.ShowDialog();
        }

        public static void CycleRotation(bool isTank = false)
        {
            var textColor = Colors.GreenYellow;
            var shadowColor = Color.FromRgb(0, 0, 0);

            if (isTank)
            {
                switch (Settings.TankMode)
                {
                    case TankModes.DPS:
                        Settings.TankMode = TankModes.Enmity;
                        if (Settings.RotationOverlay)
                        {
                            Core.OverlayManager.AddToast(() => @"Shinra Tank >>> Enmity", TimeSpan.FromMilliseconds(1000), textColor,
                                                         shadowColor, new FontFamily("Agency FB"));
                        }
                        break;
                    case TankModes.Enmity:
                        Settings.TankMode = TankModes.DPS;
                        if (Settings.RotationOverlay)
                        {
                            Core.OverlayManager.AddToast(() => @"Shinra Tank >>> DPS", TimeSpan.FromMilliseconds(1000), textColor,
                                                         shadowColor, new FontFamily("Agency FB"));
                        }
                        break;
                }
                Logging.Write(Colors.Yellow, $@"[Shinra] Tank >>> {Settings.RotationMode}");
                return;
            }
            switch (Settings.RotationMode)
            {
                case Modes.Smart:
                    Settings.RotationMode = Modes.Single;
                    if (Settings.RotationOverlay)
                    {
                        Core.OverlayManager.AddToast(() => @"Shinra Rotation >>> Single", TimeSpan.FromMilliseconds(1000), textColor,
                                                     shadowColor, new FontFamily("Agency FB"));
                    }
                    break;
                case Modes.Single:
                    Settings.RotationMode = Modes.Multi;
                    if (Settings.RotationOverlay)
                    {
                        Core.OverlayManager.AddToast(() => @"Shinra Rotation >>> Multi", TimeSpan.FromMilliseconds(1000), textColor,
                                                     shadowColor, new FontFamily("Agency FB"));
                    }
                    break;
                case Modes.Multi:
                    Settings.RotationMode = Modes.Smart;
                    if (Settings.RotationOverlay)
                    {
                        Core.OverlayManager.AddToast(() => @"Shinra Rotation >>> Smart", TimeSpan.FromMilliseconds(1000), textColor,
                                                     shadowColor, new FontFamily("Agency FB"));
                    }
                    break;
            }
            Logging.Write(Colors.Yellow, $@"[Shinra] Rotation >>> {Settings.RotationMode}");
        }

        #endregion

        #region CurrentClass

        private IRotation _myRotation;
        private IRotation MyRotation => _myRotation ?? (_myRotation = GetRotation(CurrentClass));

        private ClassJobType _currentClass;
        private ClassJobType CurrentClass
        {
            get
            {
                if (_currentClass == Core.Player.CurrentJob)
                {
                    return _currentClass;
                }
                _currentClass = Core.Player.CurrentJob;
                _myRotation = GetRotation(_currentClass);
                Logging.Write(Colors.Yellow, $@"[Shinra] Loading {_currentClass}...");
                return _currentClass;
            }
        }

        public sealed override ClassJobType[] Class => new[] { Core.Player.CurrentJob };

        private static IRotation GetRotation(ClassJobType classJob)
        {
            switch (classJob)
            {
                case ClassJobType.Arcanist:
                case ClassJobType.Archer:
                case ClassJobType.Conjurer:
                case ClassJobType.Gladiator:
                case ClassJobType.Lancer:
                case ClassJobType.Marauder:
                    return new BasicClass();
                case ClassJobType.Astrologian:
                    return new Astrologian();
                case ClassJobType.Bard:
                    return new Bard();
                case ClassJobType.DarkKnight:
                    return new DarkKnight();
                case ClassJobType.Dragoon:
                    return new Dragoon();
                case ClassJobType.Paladin:
                    return new Paladin();
                case ClassJobType.RedMage:
                    return new RedMage();
                case ClassJobType.Samurai:
                    return new Samurai();
                case ClassJobType.Summoner:
                    return new Summoner();
                case ClassJobType.Warrior:
                    return new Warrior();
                case ClassJobType.WhiteMage:
                    return new WhiteMage();
                default:
                    Logging.Write(Colors.Red, $@"[Shinra] {classJob} is not supported.");
                    return new Default();
            }
        }

        #endregion

        #region Behaviors

        public override Composite CombatBehavior { get { return new ActionRunCoroutine(ctx => MyRotation.Combat()); } }
        public override Composite CombatBuffBehavior { get { return new ActionRunCoroutine(ctx => MyRotation.CombatBuff()); } }
        public override Composite HealBehavior { get { return new ActionRunCoroutine(ctx => MyRotation.Heal()); } }
        public override Composite PreCombatBuffBehavior { get { return new ActionRunCoroutine(ctx => MyRotation.PreCombatBuff()); } }
        public override Composite PullBehavior { get { return new ActionRunCoroutine(ctx => MyRotation.Pull()); } }
        public override Composite RestBehavior { get { return new ActionRunCoroutine(ctx => Rest()); } }

        #endregion

        #region Rest

        public async Task<bool> Rest()
        {
            if (WorldManager.InSanctuary || Core.Player.HasAura("Sprint") ||
                Core.Player.CurrentHealthPercent > 70 && Helpers.CurrentEnergyPct > 50)
            {
                return false;
            }
            if (MovementManager.IsMoving)
            {
                Navigator.PlayerMover.MoveStop();
            }
            Logging.Write(Colors.Yellow, @"[Shinra] Resting...");
            return true;
        }

        #endregion

        #region Chocobo

        #region Summon

        public static async Task<bool> SummonChocobo()
        {
            if (!Settings.SummonChocobo || !BotManager.Current.IsAutonomous || !ChocoboManager.CanSummon || MovementManager.IsMoving ||
                ChocoboManager.Summoned)
            {
                return false;
            }
            ChocoboManager.Summon();
            await Coroutine.Wait(2000, () => ChocoboManager.Summoned);
            Logging.Write(Colors.Yellow, @"[Shinra] Summoning Chocobo...");

            if (!ChocoboManager.Summoned)
            {
                return await SummonChocobo();
            }
            return true;
        }

        #endregion

        #region Stance

        public static async Task<bool> ChocoboStance()
        {
            if (!Settings.SummonChocobo || !ChocoboManager.Summoned || Core.Player.IsMounted || ChocoboManager.Object == null)
            {
                return false;
            }

            if (Settings.ChocoboStanceDance)
            {
                if (ChocoboManager.Stance == CompanionStance.Healer &&
                    Core.Player.CurrentHealthPercent < Math.Min(100, Settings.ChocoboStanceDancePct + 10))
                {
                    return false;
                }

                if (Core.Player.CurrentHealthPercent < Settings.ChocoboStanceDancePct)
                {
                    ChocoboManager.HealerStance();
                    await Coroutine.Wait(1000, () => ChocoboManager.Stance == CompanionStance.Healer);
                    Logging.Write(Colors.Yellow, @"[Shinra] Chocobo Stance >>> Healer");
                    return true;
                }
            }

            switch (Settings.ChocoboStance)
            {
                case Stances.Free:
                    if (ChocoboManager.Stance == CompanionStance.Free)
                    {
                        break;
                    }
                    ChocoboManager.FreeStance();
                    await Coroutine.Wait(1000, () => ChocoboManager.Stance == CompanionStance.Free);
                    Logging.Write(Colors.Yellow, @"[Shinra] Chocobo Stance >>> Free");
                    return true;
                case Stances.Attacker:
                    if (ChocoboManager.Stance == CompanionStance.Attacker)
                    {
                        break;
                    }
                    ChocoboManager.AttackerStance();
                    await Coroutine.Wait(1000, () => ChocoboManager.Stance == CompanionStance.Attacker);
                    Logging.Write(Colors.Yellow, @"[Shinra] Chocobo Stance >>> Attacker");
                    return true;
                case Stances.Healer:
                    if (ChocoboManager.Stance == CompanionStance.Healer)
                    {
                        break;
                    }
                    ChocoboManager.HealerStance();
                    await Coroutine.Wait(1000, () => ChocoboManager.Stance == CompanionStance.Healer);
                    Logging.Write(Colors.Yellow, @"[Shinra] Chocobo Stance >>> Healer");
                    return true;
                case Stances.Defender:
                    if (ChocoboManager.Stance == CompanionStance.Defender)
                    {
                        break;
                    }
                    ChocoboManager.DefenderStance();
                    await Coroutine.Wait(1000, () => ChocoboManager.Stance == CompanionStance.Defender);
                    Logging.Write(Colors.Yellow, @"[Shinra] Chocobo Stance >>> Defender");
                    return true;
            }
            return false;
        }

        #endregion

        #endregion

        #region Potion

        public static async Task<bool> UsePotion()
        {
            if (!Settings.UsePotion || Core.Player.ClassLevel > 30 || Core.Player.CurrentHealthPercent > Settings.UsePotionPct)
            {
                return false;
            }

            var item = InventoryManager.FilledSlots.FirstOrDefault(s => s.Name == "Potion");

            if (item == null || !item.CanUse())
            {
                return false;
            }

            item.UseItem();
            await Coroutine.Wait(1000, () => !item.CanUse());
            Logging.Write(Colors.Yellow, @"[Shinra] Using >>> Potion");
            return true;
        }

        #endregion

        #region LastSpell

        private static Spell _lastSpell;
        public static Spell LastSpell { get { return _lastSpell ?? (_lastSpell = new Spell()); } set { _lastSpell = value; } }

        #endregion
    }
}