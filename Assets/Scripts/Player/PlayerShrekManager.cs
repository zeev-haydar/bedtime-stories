
using System.Collections.Generic;
using UnityEngine;

namespace Player{
    public class PlayerShrekManager : MonoBehaviour 
    {
        private static PlayerShrekManager instance;
        public static PlayerShrekManager Instance {get => instance;}
        public List<PlayerBox> playerBoxes;

        void Start()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }
            instance = this;
            DontDestroyOnLoad(this);
        }
        public void StartGame()
        {

        }

    }
}