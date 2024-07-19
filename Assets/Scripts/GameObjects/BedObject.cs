using UnityEngine;
using UnityEngine.UI;
using BedMechanics;
using Exceptions;
using Player;
using UnityEngine.SceneManagement;
using Managers;

namespace GameObjects
{
    public class BedObject : MonoBehaviour, IAppliableObject
    {
        private static BedObject instance;
        private Bounds bedBounds;
        public Bed bed;
        public bool testing = true;
        public Slider hpbar;

        public GameObject holePrefab;
        public static BedObject Instance { get => instance; }

        public int maxHealth = 10;
        private void Awake()
        {
            instance = this;
            bedBounds = transform.Find("Bounds").GetComponent<SpriteRenderer>().bounds;
        }
        void Start()
        {
            bed = new Bed(maxHealth);
        }
        void Update()
        {
            // kalau testing, berarti bisa ngaktifin key buat ngurangin darah kasur
            if (testing)
            {
                // if (Input.GetKeyDown(KeyCode.K)) GetHit();
            }
            hpbar.value = bed.mainHealth / bed.maxMainHealth;

            // check condition when the bed is destroyed
            if (GameOverCondition()) Lose();
        }

        public void GetHit(Vector2 hitPoint)
        {
            bed.ReduceHealth();
            Instantiate(holePrefab,hitPoint,transform.rotation, transform);
            
        }

        private bool GameOverCondition()
        {
            return bed.mainHealth <= 0;
        }

        private void Lose()
        {
            //Destroy(gameObject);
            SceneManager.LoadScene("Lose");
        }

        public void Apply(ItemObject item, PlayerObject player)
        {
            try
            {
                bed.Apply(item.item);
                Destroy(item.gameObject);
            }
            catch (CannotApplyException e)
            {
                Debug.Log(e.Message);

            }
        }

        public bool CanApply(ItemObject item)
        {
            if (item == null) return false;
            return bed.CanApply(item.item);
        }

        /// <summary>
        /// Get health from 0-5
        /// </summary>
        /// <returns>Integer from [0,5]; representasi nilai health</returns>
        public int GetHealthNormalized()
        {
            return Mathf.CeilToInt(5 * bed.mainHealth / bed.maxMainHealth);
        }
        public Vector2 RandomBedPoint()
        {
            return new Vector2(Random.Range(bedBounds.min.x, bedBounds.max.x), Random.Range(bedBounds.min.y, bedBounds.max.y));
        }
    }
}