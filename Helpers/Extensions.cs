using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ff14bot;
using ff14bot.Enums;

namespace ShinraCo
{
    public static partial class Helpers
    {
        public static void RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dic, Func<TValue, bool> predicate)
        {
            var keys = dic.Keys.Where(k => predicate(dic[k])).ToList();
            foreach (var key in keys)
                dic.Remove(key);
        }

        private static readonly HashSet<ClassJobType> ManaJobs = new HashSet<ClassJobType>
        {
            ClassJobType.Arcanist,
            ClassJobType.Astrologian,
            ClassJobType.BlackMage,
            ClassJobType.Conjurer,
            ClassJobType.RedMage,
            ClassJobType.Scholar,
            ClassJobType.Summoner,
            ClassJobType.Thaumaturge,
            ClassJobType.WhiteMage
        };

        public static float CurrentEnergyPct => ManaJobs.Contains(Core.Player.CurrentJob)
            ? Core.Player.CurrentManaPercent
            : Core.Player.CurrentTPPercent;

        private static readonly string VersionPath = Path.Combine(Environment.CurrentDirectory, @"Routines\Shinra\Properties\Version.txt");

        public static string GetLocalVersion()
        {
            if (!File.Exists(VersionPath)) { return null; }
            try
            {
                return File.ReadAllText(VersionPath);
            }
            catch { return null; }
        }
    }
}