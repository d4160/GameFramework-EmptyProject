using d4160.Core;
using Game.Character_Inventory;
using Game.Menus;
using UnityEngine;
using UnityEngine.GameFoundation;

namespace Game.Character
{
    public class CharacterSystem : MonoBehaviourData<Character>
    {
        [SerializeField] private int _player;
        [SerializeField] private Transform _characterRoot;
        [Header("CURRENT")]
        [SerializeField] private GameObject _characterInstance;

        public void SetPlayer(int player)
        {
            _player = player;
        }

        void Awake()
        {
            if (_characterInstance)
            {
                Destroy(_characterInstance);
            }
        }

        public void InstantiateCharacter()
        {
            if (!GameFoundationSdk.IsInitialized)
            {
                Debug.LogError("You need to initialize GameFoundation first.");

                return;
            }

            //InventoryItem[] items = InventoryManager.FindItemsByTag("Personaje");

            //if (items.Length == 0)
            //{
            //    Debug.LogError("You don't have any character yet. Find one first!");
            //    return;
            //}

            //InventoryItem character = items.Random();

            //var assetsDetail = character.definition.GetDetail<AssetsDetail>();
            //var prefab = assetsDetail.GetAsset<GameObject>("Prefab");

            //_characterInstance = Instantiate(prefab);
            //_characterInstance.transform.SetParent(_characterRoot, false);

            //if (!GameFoundation.IsInitialized)
            //{
            //    Debug.LogError("You need to initialize GameFoundation first.");
            //    return;
            //}

            //var characters = InventoryManager.FindItemsByTag("Personaje");
            //if (characters.Length > 1)
            //{
            //    var player1 = characters[_player];

            //    string glove = player1.GetProperty("Guante");
            //    string boot = player1.GetProperty("Bota");
            //    int eyeColor = player1.GetProperty("EyeColor");
            //    int bodyColor = player1.GetProperty("BodyColor");
            //    int headColor = player1.GetProperty("HeadColor");
            //    string name = player1.GetProperty("Name");

            //    var characterInventory = GetComponentInChildren<CharacterInventorySystem>();
            //    characterInventory.SetBoots(boot);
            //    characterInventory.SetGloves(glove);
            //    //characterInventory.ChangeEyeColor(PlayerSelectionMenu.EyeColors[eyeColor]);
            //    //characterInventory.ChangeBodyColor(PlayerSelectionMenu.BodyColors[bodyColor]);
            //    //characterInventory.ChangeHeadColor(PlayerSelectionMenu.HeadColors[headColor]);
            //}
        }
    }
}
