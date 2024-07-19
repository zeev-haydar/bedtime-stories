using GameObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private Wave wave;
        [SerializeField] private GameObject blanketPrefab;
        private int currCluster = 0;
        private EnemySpawner spawner;

        private float currCD = 0;
        private bool rewarded = false;
        public bool test;
        private int currWave = 0;
        public int CurrWave {  get { return currWave; } }

        private void Awake()
        {
            spawner = GetComponent<EnemySpawner>();
            if (!test)
            {
                wave = LevelManager.Instance.LevelWave;
            }
            currCD = wave.enemyClusters[currCluster].countDown;
        }

        private void Update()
        {
            if(currCluster == wave.enemyClusters.Count)
            {
                if (spawner.ExistingEnemies.Count == 0)
                {
                    SceneManager.LoadScene("Win");
                }
                return;
            }
            switch (wave.Spawn(currCluster).enterCondition)
            {
                case Cluster.Condition.Countdown:
                    currCD -= Time.deltaTime;
                    if (currCD <= 0)
                    {
                        SpawnCluster(wave.Spawn(currCluster));
                        currCD = wave.enemyClusters[currCluster].countDown;
                        currCluster++;
                    }
                    break;
                case Cluster.Condition.ZeroEnemies:
                    if (spawner.ExistingEnemies.Count == 0)
                    {
                        currCD -= Time.deltaTime;
                        if (!rewarded)
                        {
                            SpawnBlankets(wave.enemyClusters[currCluster].rewardOnClear);
                            rewarded = true;
                        }
                        if (currCD <= 0)
                        {
                            SpawnCluster(wave.Spawn(currCluster));
                            currCD = wave.enemyClusters[currCluster].countDown;
                            currCluster++;
                            currWave++;
                            rewarded = false;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void SpawnCluster(Cluster cluster)
        {
            if (test || cluster.enemies[0] == EnemySpawner.EnemyType.BossEnemy)
            {
                foreach (var enemy in cluster.enemies)
                {
                    spawner.Spawn(enemy);
                }
                return;
            }

            for (int i = 0; i < PlayerManager.Instance.Players.Count; i++)
            {
                foreach (var enemy in cluster.enemies)
                {
                    spawner.Spawn(enemy);
                }
            }
        }

        public void SpawnBlankets(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Instantiate(blanketPrefab, BedObject.Instance.RandomBedPoint(), blanketPrefab.transform.rotation);
            }
        }
    }

}
