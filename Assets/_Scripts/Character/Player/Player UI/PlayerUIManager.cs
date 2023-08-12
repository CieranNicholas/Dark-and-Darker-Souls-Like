using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace LS
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;
        [Header("NETWORK JOIN")]
        [SerializeField]
        bool StartGameAsClient;

        public PlayerUIHudManager playerUIHudManager;
        public PlayerUIPopUPManger playerUIPopUpManager;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            playerUIHudManager = GetComponent<PlayerUIHudManager>();
            playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUPManger>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if(StartGameAsClient)
            {
                StartGameAsClient = false;
                // WE MUST FIRST SHUTDOWN NETWORK, BECAUSE WE STARTED AS HOST DURING TITLE SCREEN
                NetworkManager.Singleton.Shutdown();
                // WE THEN RESTART AS A CLIENT
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}
