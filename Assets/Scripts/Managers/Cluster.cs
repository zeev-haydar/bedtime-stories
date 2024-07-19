using System;
using System.Collections.Generic;
using Enemies;

namespace Managers
{
    [Serializable]
    public class Cluster
    {
        /// <summary>
        /// enemies spawned in this cluster
        /// </summary>
        public List<EnemySpawner.EnemyType> enemies;

        /// <summary>
        /// condition for spawning this cluster's enemies
        /// </summary>
        public Condition enterCondition;

        /// <summary>
        /// seconds of countdown before spawning enemies
        /// </summary>
        public float countDown;

        /// <summary>
        /// amount of blankets given after clearing cluster, only works if condition is ZeroEnemies
        /// </summary>
        public int rewardOnClear;
        public enum Condition {
            Countdown,
            ZeroEnemies
        }
    }
}