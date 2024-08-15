using System.Collections;
using System.Collections.Generic;
using Enemies;
using GameObjects;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu]
    public class Wave : ScriptableObject
    {
        public List<Cluster> enemyClusters;

        //public Wave(Queue<Cluster> enemyClusters)
        //{
        //    this.enemyClusters = enemyClusters;
        //}

        public Cluster Spawn(int idx)
        {
            if (PlayerManager.Instance.CurrentPlayerCount > 1)
            {
                enemyClusters.ForEach(cluster =>
                    {
                        List<EnemySpawner.EnemyType> currClusterEnemies = cluster.enemies;
                        for (int i=0; i<Mathf.FloorToInt(PlayerManager.Instance.CurrentPlayerCount/2); i++)
                        {
                            currClusterEnemies.ForEach(enemyEnem =>
                                {
                                    cluster.enemies.Add(enemyEnem);
                                }
                            );
                        }
                    }
                );
            }
            if(idx >= enemyClusters.Count) { return null; }
            Cluster cluster = enemyClusters[idx];
            return cluster;
        }
    }
}

