using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using Enemies;
using Managers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace GameObjects
{
    public class KalaObject : EnemyObject
    {
        // Start is called before the first frame update
        public Kala kala;

        public int maxHealth;

        public Slider hpBar;

        public int numberOfBalls;

        public bool right = false;

        bool createComplete = false;

        public PlayerManager playerManager;


        [SerializeField] private float projectileAirtime;
        public GameObject projectilePrefab;
        public GameObject mouthPoint;
        public List<GameObject> handObjects;
        public Bounds bedBounds;

        public AudioManager audioManager;
        private List<SkillKey> keysToUpdate = new List<SkillKey>();
        void Awake()
        {
            animator = GetComponent<Animator>();
            audioManager = GetComponent<AudioManager>();
            handObjects[right ? 0 : 1].SetActive(false);
            
        }

        public void OnCreate() {
            maxHealth = playerManager.Players.Count * 100;
            kala = new Kala(maxHealth);
            bedBounds = BedObject.transform.Find("Bounds").GetComponent<SpriteRenderer>().bounds;
            foreach (KeyValuePair<SkillKey, float> cooldown in kala.currCooldowns)
            {
                keysToUpdate.Add(cooldown.Key);
            }
            StartCoroutine(CooldownPrinterCoroutine());
            createComplete = true;
        }

        public static KalaObject Instantiate(object[] parameterValues, PlayerManager pm) {
            MethodInfo methodInfo = typeof(GameObject).GetMethod("Instantiate");
            KalaObject gameObject = new() ;
            methodInfo.Invoke(gameObject, parameterValues);
            gameObject.playerManager = pm;
            return gameObject;
        }
        public void Spawn()
        {
            animator.Play("kala_idle");
        }

        // Update is called once per frame
        void Update()
        {
            if (!createComplete) {
                return;
            }
            Debug.Log("Current health = "+kala.Health);
            float healthPercent = (float)kala.Health / kala.MaxHealth;
            hpBar.value = healthPercent;
            if (isAttacking || isHit) { return; }

            foreach (SkillKey key in keysToUpdate)
            {

                kala.currCooldowns[key] = kala.currCooldowns[key] - Time.deltaTime;
                if (kala.currCooldowns[key] <= 0 && !isAttacking)
                {
                    StartCoroutine(ExecuteSkillCoroutine(key));
                    kala.currCooldowns[key] = kala.cooldowns[key];
                }
            }

        }

        public void Shoot()
        {
            audioManager.PlaySFX(0);
            for (int i = 0; i < numberOfBalls; i++)
            {
                GameObject projectile = Instantiate(projectilePrefab, mouthPoint.transform.position, mouthPoint.transform.rotation);
                projectile.GetComponent<EnemyProjectileObject>().Launch(mouthPoint.transform.position, RandomTargetPoint(bedBounds), projectileAirtime);
            }
        }

        public void Geprek()
        {
            StartCoroutine(GeprekCoroutine());
        }

        private void ExecuteSkill(SkillKey key)
        {
            switch (key)
            {
                case SkillKey.Balls:
                    animator.Play("kala_ranged_attack");
                    break;
                case SkillKey.Geprek:
                    animator.Play("kala_prep");
                    animator.SetBool("right", right);
                    break;
                default:
                    Debug.Log("Skill not found");
                    break;

            }
        }

        private IEnumerator ExecuteSkillCoroutine(SkillKey key)
        {
            ExecuteSkill(key);
            yield return null;
        }

        private IEnumerator CooldownPrinterCoroutine()
        {
            while (true)
            {
                foreach (SkillKey key in keysToUpdate)
                {
                    // Debug.Log("Skill " + key + " cooldown: " + kala.currCooldowns[key] + " seconds");
                }
                yield return new WaitForSeconds(1);
            }
        }

        private new Vector2 RandomTargetPoint(Bounds bounds)
        {
            return new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
        }

        private IEnumerator GeprekCoroutine()
        {
            handObjects[right ? 0 : 1].SetActive(true);
            // GetComponent<BoxCollider>.bounds
            Debug.Log("Gua geprek lu");
            GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Bed Top");
            audioManager.PlaySFX(1);
            BedObject.GetHit(handObjects[right ? 0 : 1].transform.position);

            yield return new WaitForSeconds(7.5f);
            Debug.Log("udah aing geprek");
            GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Bed Back");
            handObjects[right ? 0 : 1].SetActive(false);
            right = !right;
        }

        public override void TakeHit(int damage)
        {
            kala.ReduceHealth(damage);
            if (kala.Health <= 0)
            {
                EnemyDeath();
            }
            Debug.LogWarning("Kena hit. health sekarang: " + kala.Health);
        }

        public override void EnemyDeath()
        {
            animator.Play("kala_defeat");
            RemoveFromManager();

        }

        public void RemoveFromManager()
        {
            EnemySpawner.Instance.RemoveFallenEnemy(gameObject);
        }

        public void ForwardLayer()
        {
            Sprite.sortingLayerID = 5;
        }

        public void BackLayer()
        {
            Sprite.sortingLayerID = 2;
        }

        public static explicit operator KalaObject(GameObject v)
        {
            throw new System.NotImplementedException();
        }
    }

}
