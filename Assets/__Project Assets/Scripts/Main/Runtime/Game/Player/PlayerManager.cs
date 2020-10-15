using d4160.Core;
using d4160.Core.MonoBehaviours;
using Game.Character_Inventory;
using Game.Health;
using NaughtyAttributes;
using PlayFab.ClientModels;
using Game.Menus;
using Game.UI;
using UnityEngine;
using UnityEngine.GameFoundation;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerManager : UnityObjectListSingleton<PlayerManager, GameObject>
    {
        [SerializeField] private PlayerInputManager _inputManager;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private string[] _inputSchemes;
        [SerializeField] private HealthUI[] _healthUIs;
        [SerializeField] private CombatHUD _combatHUD;

        protected override void Start()
        {
            base.Start();

            JoinPlayer();
            JoinPlayer();
        }

        [Button]
        public void JoinPlayer()
        {
            if (Count >= _inputManager.maxPlayerCount)
            {
                return;
            }

            Transform spawnPoint = _spawnPoints[Count];
            GameObject obj = AddObject(spawnPoint.position, spawnPoint.rotation);

            string inputSchema = _inputSchemes[Count - 1];
            var playerController = obj.GetComponent<PlayerController>();
            playerController?.SwitchCurrentControlScheme(inputSchema);

            HealthUI healthUI = _healthUIs[Count - 1];
            var healthSystem = obj.GetComponentInChildren<HealthSystem>();
            healthUI.RegisterEvent(healthSystem);

            if (Count == 2)
            {
                var newScale = playerController.Character.transform.localScale;
                newScale.x *= -1;

                playerController.Character.transform.localScale = newScale;
            }

            playerController.Character.SetPlayer(Count - 1);

            if (!GameFoundation.IsInitialized)
            {
                Debug.LogError("You need to initialize GameFoundation first.");
                return;
            }

            var characters = InventoryManager.FindItemsByTag("Personaje");
            if (characters.Length > 1)
            {
                var player1 = characters[Count - 1];

                //string glove = player1.GetProperty("Guante");
                //string boot = player1.GetProperty("Bota");
                //int eyeColor = player1.GetProperty("EyeColor");
                //int bodyColor = player1.GetProperty("BodyColor");
                //int headColor = player1.GetProperty("HeadColor");
                string name = player1.GetProperty("Name");

                //var characterInventory = obj.GetComponentInChildren<CharacterInventorySystem>();
                //characterInventory.SetBoots(boot);
                //characterInventory.SetGloves(glove);
                //characterInventory.ChangeEyeColor(PlayerSelectionMenu.EyeColors[eyeColor]);
                //characterInventory.ChangeBodyColor(PlayerSelectionMenu.BodyColors[bodyColor]);
                //characterInventory.ChangeHeadColor(PlayerSelectionMenu.HeadColors[headColor]);

                _combatHUD.SetPlayerName(name, Count - 1);
            }
        }

        public void OnPlayerJoined(PlayerInput player)
        {
            Debug.Log($"OnPlayerJoined {player.name}");
        }

        public void OnPlayerLeft(PlayerInput player)
        {
            Debug.Log($"OnPlayerLeft {player.name}");
        }
    }
}
