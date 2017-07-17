using System;
using System.Collections.Generic;
using System.Linq;
using Clio.Common;
using Clio.Utilities;
using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;

namespace ShinraCo
{
    public static partial class Helpers
    {
        public static bool InView(Vector3 playerLocation, float playerHeading, Vector3 targetLocation)
        {
            var d = Math.Abs(MathEx.NormalizeRadian(playerHeading -
                                                    MathEx.NormalizeRadian(MathHelper.CalculateHeading(playerLocation, targetLocation) +
                                                                           (float)Math.PI)));

            if (d > Math.PI)
            {
                d = Math.Abs(d - 2 * (float)Math.PI);
            }
            return d < 0.78539f;
        }

        public static bool TargetDistance(this LocalPlayer cp, float range, bool useMinRange = true)
        {
            return useMinRange
                ? cp.HasTarget && cp.Distance2D(cp.CurrentTarget) - cp.CombatReach - cp.CurrentTarget.CombatReach >= range
                : cp.HasTarget && cp.Distance2D(cp.CurrentTarget) - cp.CombatReach - cp.CurrentTarget.CombatReach <= range;
        }

        private static bool IsEnemy(this BattleCharacter ie)
        {
            return GameObjectManager.Attackers.Contains(ie) && ie.IsAlive && ie.CanAttack && ie.IsTargetable;
        }

        public static IEnumerable<BattleCharacter> EnemyUnit
        {
            get { return GameObjectManager.GetObjectsOfType<BattleCharacter>(true).Where(eu => eu.IsEnemy()); }
        }

        public static int EnemiesNearTarget(float radius)
        {
            return Core.Player.CurrentTarget == null
                ? 0
                : EnemyUnit.Count(eu => eu.Distance2D(Core.Player.CurrentTarget) - eu.CombatReach - Core.Player.CurrentTarget.CombatReach <=
                                        radius);
        }

        public static int EnemiesNearPlayer(float radius)
        {
            return EnemyUnit.Count(eu => eu.Distance2D(Core.Player) - eu.CombatReach - Core.Player.CombatReach <= radius);
        }
    }
}