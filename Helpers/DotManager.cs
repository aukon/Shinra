using System;
using System.Collections.Generic;
using System.Linq;
using ff14bot.Objects;
using static ShinraCo.Constants;

namespace ShinraCo
{
    public class DotManager
    {
        public static readonly Dictionary<string, Tuple<DateTime, int, int>> DotSnapshots =
            new Dictionary<string, Tuple<DateTime, int, int>>();

        private static readonly Dictionary<string, int> CritBuffs =
            new Dictionary<string, int>
            {
                { "Battle Litany", 15 },
                { "Chain Stratagem", 15 },
                { "The Spear", 5 }
            };

        private static readonly Dictionary<string, int> DamageBuffs =
            new Dictionary<string, int>
            {
                { "Brotherhood", 5 },
                { "Raging Strikes", 10 },
                { "The Balance", 5 }
            };

        public static bool BuffExpiring => CritExpiring || DamageExpiring;
        public static int CritBonus { get { return CritBuffs.Where(dic => Me.HasAura(dic.Key)).Sum(dic => dic.Value); } }
        public static bool CritExpiring { get { return CritBuffs.Any(dic => Me.AuraExpiring(dic.Key)); } }

        public static int DamageBonus
        {
            get
            {
                var count = 0;
                if (Me.HasAura("Embolden")) count += 2 * EmboldenStacks;
                if (Target.HasAura("Trick Attack")) count += 10;
                count += DamageBuffs.Where(dic => Me.HasAura(dic.Key)).Sum(dic => dic.Value);
                return count;
            }
        }

        public static bool DamageExpiring
        {
            get
            {
                return Me.HasAura("Embolden") && EmboldenStacks == 5 || Target.AuraExpiring("Trick Attack") ||
                       DamageBuffs.Any(dic => Me.AuraExpiring(dic.Key));
            }
        }

        private static int EmboldenStacks
        {
            get
            {
                var aura = Me.GetAuraById(1239);
                var value = aura?.Value ?? 0;
                return (int)value;
            }
        }

        public static void Add(GameObject tar)
        {
            DotSnapshots[ObjectId(tar)] = Tuple.Create(DateTime.Now + TimeSpan.FromSeconds(30), DamageBonus, CritBonus);
            Helpers.Debug($"DotManager - Damage: {DamageBonus}%, Crit: {CritBonus}%");
        }

        public static int Check(GameObject tar, bool crit = false)
        {
            DotSnapshots.RemoveAll(t => DateTime.Now > t.Item1);
            return crit ? DotSnapshots[ObjectId(tar)]?.Item3 ?? 0 : DotSnapshots[ObjectId(tar)]?.Item2 ?? 0;
        }

        public static int Difference(GameObject tar, bool crit = false)
        {
            var old = Check(tar, crit);
            return (crit ? CritBonus : DamageBonus) - old;
        }

        public static bool Recent(GameObject tar)
        {
            return DotSnapshots.ContainsKey(ObjectId(Target)) &&
                   DotSnapshots[ObjectId(tar)].Item1 - DateTime.Now > TimeSpan.FromSeconds(25);
        }

        private static string ObjectId(GameObject tar)
        {
            return tar != null && tar.IsValid ? string.Join("-", tar.ObjectId, tar.Name) : "";
        }
    }
}
