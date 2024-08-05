using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameObjects;

namespace Managers
{
    public class EnemySpawner : MonoBehaviour
    {
        //test
        public Transform spawnPoint;

        
        private static EnemySpawner instance;
        [SerializeField] private GameObject meleeEnemyPrefab;
        [SerializeField] private GameObject rangedEnemyPrefab;
        [SerializeField] private GameObject bossEnemyPrefab;
        private List<GameObject> existingEnemies = new List<GameObject>();
        public List<GameObject> ExistingEnemies { get => existingEnemies; }
        public static EnemySpawner Instance { get => instance; }

        public enum EnemyType
        {
            MeleeEnemy,
            RangedEnemy,
            BossEnemy
        }

        private void Awake()
        {
            instance = this;
        }

        public void Spawn(EnemyType enemyType, RangedEnemyObject.RangedPosition rangedPosition = RangedEnemyObject.RangedPosition.Top, PlayerManager playerManager = null)
        {
            GameObject newEnemy;
            switch (enemyType)
            {
                case EnemyType.MeleeEnemy:
                    newEnemy = Instantiate(meleeEnemyPrefab, spawnPoint.position, spawnPoint.rotation);
                    ExistingEnemies.Add(newEnemy);
                    newEnemy.GetComponent<MeleeEnemyObject>().MeleeEnemySpawn();
                    break;
                case EnemyType.RangedEnemy:
                    newEnemy = Instantiate(rangedEnemyPrefab, spawnPoint.position, spawnPoint.rotation);
                    ExistingEnemies.Add(newEnemy);
                    int rand = Random.Range(1, 3);
                    newEnemy.GetComponent<RangedEnemyObject>().RangedEnemySpawn(rand == 1 ? RangedEnemyObject.RangedPosition.Top : RangedEnemyObject.RangedPosition.Bottom);
                    break;
                case EnemyType.BossEnemy:
                    newEnemy = Instantiate(bossEnemyPrefab, spawnPoint.position,spawnPoint.rotation);
                    Debug.Log("Ini boss");
                    if (playerManager) {
                        Debug.Log("Bossnya ngespawn");
                        KalaObject kalaEnemy = newEnemy.GetComponent<KalaObject>();
                        kalaEnemy.playerManager = playerManager;
                        kalaEnemy.OnCreate();
                        ExistingEnemies.Add(kalaEnemy.gameObject); 
                        kalaEnemy.Spawn();
                    }
                    break;
                default:
                    break;
            }
        }

        public void RemoveFallenEnemy(GameObject fallenEnemy)
        {
            existingEnemies.Remove(fallenEnemy);
            Destroy(fallenEnemy);
        }

        //test
        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.RightShift))
        //    {
        //        Spawn(EnemyType.MeleeEnemy);
        //    }
        //    if (Input.GetKeyDown(KeyCode.I))
        //    {
        //        Spawn(EnemyType.RangedEnemy);
        //    }
        //    if (Input.GetKeyDown(KeyCode.K))
        //    {
        //        Spawn(EnemyType.RangedEnemy, RangedEnemyObject.RangedPosition.Bottom);
        //    }
        //}
    }
}
