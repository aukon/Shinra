﻿using System;
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
        public sealed override float PullRange => 25;
        public sealed override bool WantButton => true;

        public sealed override void Initialize()
        {
            Logging.Write(Colors.GreenYellow, $@"[Shinra] Loaded Version: {Helpers.GetLocalVersion()}");
            Overlay.Visible = Settings.RotationOverlay;
            Overlay.UpdateText();
            HotkeyManager.Register("Shinra Rotation", Helpers.GetHotkey(Settings.RotationHotkey),
                                   Helpers.GetModkey(Settings.RotationHotkey), hk => CycleRotation());
            HotkeyManager.Register("Shinra Cooldown", Helpers.GetHotkey(Settings.CooldownHotkey),
                                   Helpers.GetModkey(Settings.CooldownHotkey), hk => CycleRotation(true));
            HotkeyManager.Register("Shinra Tank", Helpers.GetHotkey(Settings.TankHotkey), Helpers.GetModkey(Settings.TankHotkey),
                                   hk => CycleRotation(false, true));
            HotkeyManager.Register("Shinra Turret Toggle", Helpers.GetHotkey(Settings.MachinistToggle), Helpers.GetModkey(Settings.MachinistToggle),
                                   hk => TurretLocationChange());
        
        }

        public sealed override void Pulse()
        {
            var _class = CurrentClass;
            ResetOpener();
        }

        public sealed override void ShutDown()
        {
            Logging.Write(Colors.GreenYellow, @"[Shinra] Shutting down...");
            HotkeyManager.Unregister("Shinra Rotation");
            HotkeyManager.Unregister("Shinra Tank");
            HotkeyManager.Unregister("Shinra Turret Toggle");
        }

        #endregion

        #region Settings

        private Form _configForm;

        public static ShinraSettings Settings = ShinraSettings.Instance;
        public static readonly ShinraOverlay Overlay = new ShinraOverlay();

        public sealed override void OnButtonPress()
        {
            if (_configForm == null || _configForm.IsDisposed || _configForm.Disposing)
            {
                _configForm = new ShinraForm();
            }
            _configForm.ShowDialog();
        }

        public static void TurretLocationChange()
        {
        switch(Settings.MachinistTurretLocation)
        {
            case CastLocations.Self:
             Settings.MachinistTurretLocation = CastLocations.Target;
             break;
            case CastLocations.Target:
             Settings.MachinistTurretLocation = CastLocations.Self;
             break;
        }
        Helpers.DisplayToast($@"Shinra Turret Location >>> {Settings.MachinistTurretLocation}");
        Logging.Write(Colors.Yellow, $@"[Shinra] Turret Location >>> {Settings.MachinistTurretLocation}");
        }

        public static void CycleRotation(bool cooldown = false, bool tank = false)
        {
            if (cooldown)
            {
                switch (Settings.CooldownMode)
                {
                    case CooldownModes.Enabled:
                        Settings.CooldownMode = CooldownModes.Disabled;
                        break;
                    case CooldownModes.Disabled:
                        Settings.CooldownMode = CooldownModes.Enabled;
                        break;
                }
                Helpers.DisplayToast($@"Shinra Cooldown >>> {Settings.CooldownMode}");
                Logging.Write(Colors.Yellow, $@"[Shinra] Cooldown >>> {Settings.CooldownMode}");
            }
            else if (tank)
            {
                switch (Settings.TankMode)
                {
                    case TankModes.DPS:
                        Settings.TankMode = TankModes.Enmity;
                        break;
                    case TankModes.Enmity:
                        Settings.TankMode = TankModes.DPS;
                        break;
                }
                Helpers.DisplayToast($@"Shinra Tank >>> {Settings.TankMode}");
                Logging.Write(Colors.Yellow, $@"[Shinra] Tank >>> {Settings.TankMode}");
            }
            else
            {
                switch (Settings.RotationMode)
                {
                    case Modes.Smart:
                        Settings.RotationMode = Modes.Single;
                        break;
                    case Modes.Single:
                        Settings.RotationMode = Modes.Multi;
                        break;
                    case Modes.Multi:
                        Settings.RotationMode = Modes.Smart;
                        break;
                }
                Helpers.DisplayToast($@"Shinra Rotation >>> {Settings.RotationMode}");
                Logging.Write(Colors.Yellow, $@"[Shinra] Rotation >>> {Settings.RotationMode}");
            }
            Overlay.UpdateText();
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
                case ClassJobType.Pugilist:
                case ClassJobType.Rogue:
                case ClassJobType.Thaumaturge:
                    return new BasicClass();
                case ClassJobType.Astrologian:
                    return new Astrologian();
                case ClassJobType.Bard:
                    return new Bard();
                case ClassJobType.BlackMage:
                    return new BlackMage();
                case ClassJobType.DarkKnight:
                    return new DarkKnight();
                case ClassJobType.Dragoon:
                    return new Dragoon();
                case ClassJobType.Machinist:
                    return new Machinist();
                case ClassJobType.Monk:
                    return new Monk();
                case ClassJobType.Ninja:
                    return new Ninja();
                case ClassJobType.Paladin:
                    return new Paladin();
                case ClassJobType.RedMage:
                    return new RedMage();
                case ClassJobType.Samurai:
                    return new Samurai();
                case ClassJobType.Scholar:
                    return new Scholar();
                case ClassJobType.Summoner:
                    return new Summoner();
                case ClassJobType.Warrior:
                    return new Warrior();
                case ClassJobType.WhiteMage:
                    return new WhiteMage();
                default:
                    //Logging.Write(Colors.OrangeRed, $@"[Shinra] {classJob} is not supported.");
                    return new Default();
            }
        }

        #endregion

        #region Behaviors

        public override Composite CombatBehavior
        {
            get { return new PrioritySelector(new Decorator(r => Core.Player.HasTarget, new ActionRunCoroutine(ctx => MyRotation.Combat()))); }
        }

        public override Composite CombatBuffBehavior
        {
            get { return new PrioritySelector(new Decorator(r => Core.Player.HasTarget, new ActionRunCoroutine(ctx => MyRotation.CombatBuff()))); }
        }

        public override Composite PullBehavior
        {
            get { return new PrioritySelector(new Decorator(r => Core.Player.HasTarget, new ActionRunCoroutine(ctx => MyRotation.Pull()))); }
        }

        public override Composite HealBehavior { get { return new ActionRunCoroutine(ctx => MyRotation.Heal()); } }
        public override Composite PreCombatBuffBehavior { get { return new ActionRunCoroutine(ctx => MyRotation.PreCombatBuff()); } }
        public override Composite RestBehavior { get { return new ActionRunCoroutine(ctx => Rest()); } }

        #endregion

        #region Rest

        public async Task<bool> Rest()
        {
            if (!BotManager.Current.IsAutonomous || WorldManager.InSanctuary || Core.Player.HasAura("Sprint") ||
                (!Settings.RestHealth || Core.Player.CurrentHealthPercent > Settings.RestHealthPct) &&
                (!Settings.RestEnergy || Helpers.CurrentEnergyPct > Settings.RestEnergyPct))
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

        #region Opener

        public static int OpenerStep;
        public static bool OpenerFinished;

        private static void ResetOpener()
        {
            if (!Core.Player.InCombat && !Spell.RecentSpell.ContainsKey("Opener"))
            {
                OpenerStep = 0;
                OpenerFinished = false;
            }
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

        #region LastSpell

        private static Spell _lastSpell;
        public static Spell LastSpell { get { return _lastSpell ?? (_lastSpell = new Spell()); } set { _lastSpell = value; } }

        #endregion
    }
}