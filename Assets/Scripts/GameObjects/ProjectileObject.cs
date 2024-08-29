using UnityEngine;  
using System.Collections.Generic;
using Enemies;

namespace GameObjects
{
    public class ProjectileObject : MonoBehaviour
    {
        public float startTime;
        public float duration = 7.0f;

        public int damage = 2;
        public List<Sprite> possibleSprites;
        [SerializeField]private float zrotation;

        private void Awake()
        {
            //GetComponent<SpriteRenderer>().sprite = possibleSprites[Random.Range(0, possibleSprites.Count)];
        }
        void Start()
        {
            startTime = Time.time;
        }
        void Update()
        {
            float currentTime = Time.time;
            transform.localRotation = Quaternion.Euler(0, 0, zrotation - 90f);
            if (currentTime - startTime > duration)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if(collider.CompareTag("Enemy"))
            {
                EnemyObject enemy = collider.GetComponent<EnemyObject>();
                if (enemy == null) {
                    enemy = collider.GetComponentInParent<EnemyObject>();
                }

                enemy.TakeHit(damage);

                // Destroy the Projectile
                // Destroy(enemy.gameObject);
                Destroy(gameObject);
            }

        }

        public void Launch(float rotation)
        {
            zrotation = rotation;
        }
    }
}