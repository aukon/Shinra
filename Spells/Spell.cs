using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using ff14bot.Pathing;
using ShinraCo.Settings;

namespace ShinraCo.Spells
{
    #region Enums

    public enum CastType
    {
        Target,
        Self,
        TargetLocation,
        SelfLocation
    }

    public enum GCDType
    {
        On,
        Off
    }

    public enum SpellType
    {
        Damage,
        DoT,
        AoE,
        Cooldown,
        Buff,
        Heal,
        Pet,
        Ninjutsu,
        Mudra,
        Card
    }

    #endregion

    public class Spell
    {
        public static readonly Dictionary<string, DateTime> RecentSpell = new Dictionary<string, DateTime>();
        public string Name { get; set; }
        public uint ID { get; set; }
        public byte Level { get; set; }
        public GCDType GCDType { private get; set; }
        public SpellType SpellType { private get; set; }
        public CastType CastType { private get; set; }

        public async Task<bool> Cast(GameObject target = null, bool checkGCDType = true)
        {
            #region Target

            if (target == null)
            {
                switch (CastType)
                {
                    case CastType.Target:
                    case CastType.TargetLocation:
                        if (!Core.Player.HasTarget)
                        {
                            return false;
                        }
                        target = Core.Player.CurrentTarget;
                        break;
                    default:
                        target = Core.Player;
                        break;
                }
            }

            #endregion

            #region RecentSpell

            RecentSpell.RemoveAll(t => DateTime.UtcNow > t);
            if (RecentSpell.ContainsKey(target.ObjectId.ToString("X") + "-" + Name))
            {
                return false;
            }

            #endregion

            #region AoE

            if (SpellType == SpellType.AoE && Shinra.Settings.RotationMode != Modes.Multi)
            {
                var enemyCount = Helpers.EnemyUnit.Count(eu => eu.Distance2D(target) - eu.CombatReach - target.CombatReach <=
                                                               DataManager.GetSpellData(ID).Radius);

                switch (Core.Player.CurrentJob)
                {
                    case ClassJobType.Arcanist:
                    case ClassJobType.Summoner:
                        if (enemyCount < 2)
                        {
                            return false;
                        }
                        break;
                    default:
                        if (enemyCount < 3)
                        {
                            return false;
                        }
                        break;
                }
            }

            #endregion

            #region Directional

            // Cone
            if (ID == 41 || ID == 70 || ID == 106 || ID == 7483 || ID == 7488)
            {
                if (!Helpers.InView(Core.Player.Location, Core.Player.Heading, target.Location))
                {
                    return false;
                }
            }

            // Line
            if (ID == 86 || ID == 7496)
            {
                if (!Core.Player.IsFacing(target))
                {
                    return false;
                }
            }

            #endregion

            #region Card

            if (SpellType == SpellType.Card)
            {
                #region IsMounted

                if (Core.Player.IsMounted)
                {
                    return false;
                }

                #endregion

                #region CanCast

                if (!ActionManager.CanCast(ID, target) || RecentSpell.ContainsKey("Card"))
                {
                    return false;
                }

                #endregion

                #region DoAction

                if (!await Coroutine.Wait(1000, () => ActionManager.DoAction(ID, target)))
                {
                    return false;
                }

                #endregion

                Shinra.LastSpell = this;

                #region AddRecent

                var val = DateTime.UtcNow + TimeSpan.FromSeconds(.5);

                if (ID == 3593)
                {
                    val += TimeSpan.FromSeconds(2);
                }
                RecentSpell.Add("Card", val);

                #endregion

                Logging.Write(Colors.GreenYellow, $@"[Shinra] Casting >>> {Name}");
                return true;
            }

            #endregion

            #region Ninjutsu

            if (SpellType == SpellType.Ninjutsu || SpellType == SpellType.Mudra)
            {
                #region Movement

                if (BotManager.Current.IsAutonomous)
                {
                    switch (ActionManager.InSpellInRangeLOS(2247, target))
                    {
                        case SpellRangeCheck.ErrorNotInLineOfSight:
                            await CommonTasks.MoveAndStop(new MoveToParameters(target.Location), 0f);
                            return false;
                        case SpellRangeCheck.ErrorNotInRange:
                            await CommonTasks.MoveAndStop(new MoveToParameters(target.Location), 0f);
                            return false;
                        case SpellRangeCheck.ErrorNotInFront:
                            if (!target.InLineOfSight())
                            {
                                await CommonTasks.MoveAndStop(new MoveToParameters(target.Location), 0f);
                                return false;
                            }
                            target.Face();
                            return false;
                        case SpellRangeCheck.Success:
                            if (MovementManager.IsMoving)
                            {
                                Navigator.PlayerMover.MoveStop();
                            }
                            break;
                    }
                }

                #endregion

                #region IsMounted

                if (Core.Player.IsMounted)
                {
                    return false;
                }

                #endregion

                #region CanCast

                if (!ActionManager.CanCast(ID, target))
                {
                    return false;
                }

                #endregion

                #region DoAction

                if (!await Coroutine.Wait(1000, () => ActionManager.DoAction(ID, target)))
                {
                    return false;
                }

                #endregion

                #region Wait

                await Coroutine.Wait(2000, () => !ActionManager.CanCast(ID, target));

                #endregion

                Shinra.LastSpell = this;

                #region AddRecent

                if (SpellType == SpellType.Mudra)
                {
                    var key = target.ObjectId.ToString("X") + "-" + Name;
                    var val = DateTime.UtcNow + TimeSpan.FromSeconds(1);
                    RecentSpell.Add(key, val);
                }

                #endregion

                Logging.Write(Colors.GreenYellow, $@"[Shinra] Casting >>> {Name}");
                return true;
            }

            #endregion

            #region CanAttack

            if (!target.CanAttack && CastType != CastType.Self)
            {
                switch (SpellType)
                {
                    case SpellType.Damage:
                    case SpellType.DoT:
                    case SpellType.Cooldown:
                        return false;
                }
            }

            #endregion

            #region HasSpell

            if (!ActionManager.HasSpell(ID))
            {
                return false;
            }

            #endregion

            #region Movement

            if (BotManager.Current.IsAutonomous)
            {
                switch (ActionManager.InSpellInRangeLOS(ID, target))
                {
                    case SpellRangeCheck.ErrorNotInLineOfSight:
                        await CommonTasks.MoveAndStop(new MoveToParameters(target.Location), 0f);
                        //Logging.Write(Colors.OrangeRed, $@"[Shinra] DEBUG - LineOfSight >>> {Name}");
                        return false;
                    case SpellRangeCheck.ErrorNotInRange:
                        await CommonTasks.MoveAndStop(new MoveToParameters(target.Location), 0f);
                        //Logging.Write(Colors.OrangeRed, $@"[Shinra] DEBUG - Range >>> {Name}");
                        return false;
                    case SpellRangeCheck.ErrorNotInFront:
                        if (!target.InLineOfSight())
                        {
                            await CommonTasks.MoveAndStop(new MoveToParameters(target.Location), 0f);
                            //Logging.Write(Colors.OrangeRed, $@"[Shinra] DEBUG - Facing >>> {Name}");
                            return false;
                        }
                        target.Face();
                        return false;
                    case SpellRangeCheck.Success:
                        if (CastType == CastType.TargetLocation && Core.Player.Distance2D(target) + Core.Player.CombatReach +
                            target.CombatReach > 25)
                        {
                            await CommonTasks.MoveAndStop(new MoveToParameters(target.Location), 0f);
                            await Coroutine.Wait(1000,
                                                 () => Core.Player.Distance2D(target) + Core.Player.CombatReach + target.CombatReach <= 25);
                            return false;
                        }
                        Navigator.PlayerMover.MoveStop();
                        break;
                }

                if (Core.Player.HasTarget && !MovementManager.IsMoving && Core.Player.IsMounted)
                {
                    Logging.Write(Colors.Yellow, @"[Shinra] Dismounting...");
                    ActionManager.Dismount();
                    await Coroutine.Sleep(1000);
                }
            }

            #endregion

            #region IsMounted

            if (Core.Player.IsMounted)
            {
                return false;
            }

            #endregion

            #region StopCasting

            if (SpellType == SpellType.Heal)
            {
                if (Core.Player.IsCasting && !Helpers.HealingSpells.Contains(Core.Player.SpellCastInfo.Name))
                {
                    var stopCasting = false;
                    switch (Core.Player.CurrentJob)
                    {
                        case ClassJobType.Astrologian:
                            stopCasting = Shinra.Settings.AstrologianInterruptDamage;
                            break;
                        case ClassJobType.Scholar:
                            stopCasting = Shinra.Settings.ScholarInterruptDamage;
                            break;
                        case ClassJobType.WhiteMage:
                            stopCasting = Shinra.Settings.WhiteMageInterruptDamage;
                            break;
                    }
                    if (stopCasting)
                    {
                        Logging.Write(Colors.Yellow, $@"[Shinra] Interrupting >>> {Core.Player.SpellCastInfo.Name}");
                        ActionManager.StopCasting();
                    }
                }
            }

            #endregion

            #region CanCast

            switch (CastType)
            {
                case CastType.TargetLocation:
                case CastType.SelfLocation:
                    if (!ActionManager.CanCastLocation(ID, target.Location) || Core.Player.IsCasting)
                    {
                        return false;
                    }
                    break;
                default:
                    if (!ActionManager.CanCast(ID, target))
                    {
                        return false;
                    }
                    break;
            }

            if (MovementManager.IsMoving && DataManager.GetSpellData(ID).AdjustedCastTime.TotalMilliseconds > 0)
            {
                if (!BotManager.Current.IsAutonomous)
                {
                    return false;
                }
                Navigator.PlayerMover.MoveStop();
            }

            #endregion

            #region InView

            if (GameSettingsManager.FaceTargetOnAction == false && CastType == CastType.Target && SpellType != SpellType.Heal &&
                SpellType != SpellType.Buff && !Helpers.InView(Core.Player.Location, Core.Player.Heading, target.Location))
            {
                return false;
            }

            #endregion

            #region GCD

            if (GCDType == GCDType.Off && checkGCDType)
            {
                switch (Core.Player.CurrentJob)
                {
                    case ClassJobType.Arcanist:
                    case ClassJobType.Scholar:
                    case ClassJobType.Summoner:
                        if (DataManager.GetSpellData(163).Cooldown.TotalMilliseconds < 1000)
                        {
                            return false;
                        }
                        break;
                    case ClassJobType.Archer:
                    case ClassJobType.Bard:
                        if (DataManager.GetSpellData(97).Cooldown.TotalMilliseconds < 1000)
                        {
                            return false;
                        }
                        break;
                    case ClassJobType.Astrologian:
                        if (DataManager.GetSpellData(3594).Cooldown.TotalMilliseconds < 1000)
                        {
                            return false;
                        }
                        break;
                    case ClassJobType.Conjurer:
                    case ClassJobType.WhiteMage:
                        if (DataManager.GetSpellData(119).Cooldown.TotalMilliseconds < 1000)
                        {
                            return false;
                        }
                        break;
                    case ClassJobType.DarkKnight:
                        if (DataManager.GetSpellData(3617).Cooldown.TotalMilliseconds < 1000)
                        {
                            return false;
                        }
                        break;
                    case ClassJobType.Gladiator:
                    case ClassJobType.Paladin:
                        if (DataManager.GetSpellData(9).Cooldown.TotalMilliseconds < 1000)
                        {
                            return false;
                        }
                        break;
                    case ClassJobType.Lancer:
                    case ClassJobType.Dragoon:
                        if (DataManager.GetSpellData(75).Cooldown.TotalMilliseconds < 1000)
                        {
                            return false;
                        }
                        break;
                    case ClassJobType.Machinist:
                        if (DataManager.GetSpellData(2866).Cooldown.TotalMilliseconds < 1000)
                        {
                            return false;
                        }
                        break;
                    case ClassJobType.Marauder:
                    case ClassJobType.Warrior:
                        if (DataManager.GetSpellData(31).Cooldown.TotalMilliseconds < 1000)
                        {
                            return false;
                        }
                        break;
                    case ClassJobType.Pugilist:
                    case ClassJobType.Monk:
                        if (DataManager.GetSpellData(53).Cooldown.TotalMilliseconds < 1000)
                        {
                            return false;
                        }
                        break;
                    case ClassJobType.RedMage:
                        if (DataManager.GetSpellData(7504).Cooldown.TotalMilliseconds < 1000)
                        {
                            return false;
                        }
                        break;
                    case ClassJobType.Rogue:
                    case ClassJobType.Ninja:
                        if (DataManager.GetSpellData(2240).Cooldown.TotalMilliseconds < 1000)
                        {
                            return false;
                        }
                        break;
                    case ClassJobType.Samurai:
                        if (DataManager.GetSpellData(7477).Cooldown.TotalMilliseconds < 1000)
                        {
                            return false;
                        }
                        break;
                    case ClassJobType.Thaumaturge:
                    case ClassJobType.BlackMage:
                        if (DataManager.GetSpellData(142).Cooldown.TotalMilliseconds < 1000)
                        {
                            return false;
                        }
                        break;
                }
            }

            #endregion

            #region DoAction

            switch (CastType)
            {
                case CastType.SelfLocation:
                case CastType.TargetLocation:
                    if (!await Coroutine.Wait(1000, () => ActionManager.DoActionLocation(ID, target.Location)))
                    {
                        return false;
                    }
                    break;
                default:
                    if (!await Coroutine.Wait(1000, () => ActionManager.DoAction(ID, target)))
                    {
                        return false;
                    }
                    break;
            }

            #endregion

            #region Wait

            switch (CastType)
            {
                case CastType.SelfLocation:
                case CastType.TargetLocation:
                    await Coroutine.Wait(3000, () => !ActionManager.CanCastLocation(ID, target.Location));
                    break;
                default:
                    await Coroutine.Wait(3000, () => !ActionManager.CanCast(ID, target));
                    break;
            }

            #endregion

            Shinra.LastSpell = this;

            #region AddRecent

            if (SpellType != SpellType.Damage && SpellType != SpellType.AoE && SpellType != SpellType.Heal && await CastComplete(this))
            {
                var key = target.ObjectId.ToString("X") + "-" + Name;
                var val = DateTime.UtcNow + DataManager.GetSpellData(ID).AdjustedCastTime + TimeSpan.FromSeconds(3);
                //Logging.Write(Colors.OrangeRed, $@"[Shinra] Blacklisting >>> {Name}");
                RecentSpell.Add(key, val);
            }

            #endregion

            Logging.Write(Colors.GreenYellow, $@"[Shinra] Casting >>> {Name}");
            return true;
        }

        private static async Task<bool> CastComplete(Spell spell)
        {
            if (spell.SpellType == SpellType.DoT)
            {
                var castTime = DataManager.GetSpellData(spell.ID).AdjustedCastTime;
                if (castTime.TotalMilliseconds > 0)
                {
                    var timer = new Stopwatch();
                    timer.Start();
                    await Coroutine.Wait(castTime, () => Core.Player.IsCasting);
                    while (timer.ElapsedMilliseconds < castTime.TotalMilliseconds - 100)
                    {
                        if (!Core.Player.IsCasting)
                        {
                            return false;
                        }
                        await Coroutine.Yield();
                    }
                }
            }
            return true;
        }
    }
}