using System;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        private static PlayerManager instance;
        public static PlayerManager Instance { get => instance; }
        public List<GameObject> Players { get => players; }

        [SerializeField] private List<GameObject> players = new List<GameObject>();
        [SerializeField] private List<Vector3> InitialMenuPosition;
        [SerializeField] private List<Vector3> InitialWorldPosition;
        [SerializeField] private List<RuntimeAnimatorController> controllers;
        [SerializeField] private TextMeshProUGUI hintText;

        private float joinTimer = 0;
        public float joinHoldTime = 10f;
        private bool canJoin = true;

        public int CurrentPlayerCount { get => players.Count; }
        [HideInInspector]
        public List<bool> PlayerReadyStatus = new List<bool>();

        private Action<InputAction.CallbackContext> OnReadyPressed;

        void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }
            instance = this;
            DontDestroyOnLoad(this);
        }

        private bool AllPlayersReady()
        {
            foreach (var item in PlayerReadyStatus)
            {
                if (!item)
                {
                    return false;
                }
            }
            return true;
        }

        private void Update()
        {
            if (canJoin && players.Count > 0 && AllPlayersReady())
            {
                joinTimer += Time.deltaTime;
                hintText.text = $"Game will start in {Mathf.CeilToInt(10 - joinTimer)} seconds";

                if (canJoin && joinTimer >= joinHoldTime)
                {
                    StartGame();
                    SceneManager.LoadScene("Testing");
                    joinTimer = float.MinValue;
                }
            } else {
                joinTimer = 0;
                hintText.text = "Press any key to join the game";
            }


            if (SceneManager.GetActiveScene().buildIndex != 1 && SceneManager.GetActiveScene().buildIndex != 2)
            {
                foreach (var item in players)
                {
                    Destroy(item);
                }
                Destroy(gameObject);
            }
        }

        public void StartGame()
        {
            canJoin = false;
            GetComponent<PlayerInputManager>().DisableJoining();
            for (int i = 0; i < players.Count; i++)
            {
                players[i].transform.position = InitialWorldPosition[i];
                players[i].GetComponent<PlayerObject>().UnlockMove();
                players[i].GetComponent<Renderer>().enabled = true;

                players[i].GetComponent<PlayerInput>().currentActionMap.FindAction("Attack").started -= OnStartPressed;
                players[i].GetComponent<PlayerInput>().currentActionMap.FindAction("Attack").performed -= OnStartPressed;
                players[i].GetComponent<PlayerInput>().currentActionMap.FindAction("Attack").canceled -= OnStartPressed;

                players[i].GetComponent<PlayerInput>().currentActionMap.FindAction("Pickup").canceled -= OnReadyPressed;

                players[i].GetComponent<Animator>().runtimeAnimatorController = controllers[i];
                players[i].GetComponent<PlayerObject>().inGameScene = true;
            }
        }

        public void OnPlayerJoined(PlayerInput device)
        {
            if (!canJoin) return;
            players.Add(device.gameObject);
            DontDestroyOnLoad (device);
            if (players.Count < InitialMenuPosition.Count)
            {
                //device.transform.position = InitialMenuPosition[players.Count - 1];
                //device.GetComponent<PlayerObject>().LockMove();
                device.GetComponent<Renderer>().enabled = false;
                device.currentActionMap.FindAction("Attack").started += OnStartPressed;
                device.currentActionMap.FindAction("Attack").performed += OnStartPressed;
                device.currentActionMap.FindAction("Attack").canceled += OnStartPressed;

                PlayerReadyStatus.Add(false);
                OnReadyPressed = (ctx) => PlayerReadyStatus[players.IndexOf(device.gameObject)] = !PlayerReadyStatus[players.IndexOf(device.gameObject)];
                device.currentActionMap.FindAction("Pickup").canceled += OnReadyPressed;
            }

        }

        public void OnPlayerLeft(PlayerInput device)
        {
            players.Remove(device.gameObject);
        }

        public void OnStartPressed(InputAction.CallbackContext context)
        {
            //if (context.started)
            //{
            //    joinTimer = 0;
            //}
            //if (context.canceled)
            //{
            //    joinTimer = float.MinValue;
            //}
        }

    }

}
