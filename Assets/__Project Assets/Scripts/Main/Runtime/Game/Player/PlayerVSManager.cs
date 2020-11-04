//using d4160.Core.MonoBehaviours;
//using Game.Character_Inventory;
//using Game.Menus;
//using NaughtyAttributes;
//using UnityEngine;
//using UnityEngine.GameFoundation;

//namespace Game.Player
//{
//    public class PlayerVSManager : UnityObjectListSingleton<PlayerVSManager, GameObject>
//    {
//        [SerializeField] private float _scaleMultiplier = 2;
//        [SerializeField] private int _max = 2;
//        [SerializeField] private Transform[] _spawnPoints;
//        [SerializeField] private VSMenu _vsMenu;

//        protected override void Start()
//        {
//            base.Start();

//            JoinPlayer();
//            JoinPlayer();
//        }

//        [Button]
//        public void JoinPlayer()
//        {
//            if (Count >= _max)
//            {
//                return;
//            }

//            Transform spawnPoint = _spawnPoints[Count];
//            GameObject obj = AddObject(spawnPoint.position, spawnPoint.rotation);

//            obj.transform.localScale = Vector3.one * _scaleMultiplier;

//            if (Count == 2)
//            {
//                var newScale = obj.transform.localScale;
//                newScale.x *= -1;

//                obj.transform.localScale = newScale;
//            }

//            if (!GameFoundation.IsInitialized)
//            {
//                Debug.LogError("You need to initialize GameFoundation first.");
//                return;
//            }

//            var characters = InventoryManager.FindItemsByTag("Personaje");
//            if (characters.Length > 1)
//            {
//                var player1 = characters[Count - 1];

//                string glove = player1.GetProperty("Guante");
//                string boot = player1.GetProperty("Bota");
//                int eyeColor = player1.GetProperty("EyeColor");
//                int bodyColor = player1.GetProperty("BodyColor");
//                int headColor = player1.GetProperty("HeadColor");
//                string name = player1.GetProperty("Name");

//                var characterInventory = obj.GetComponent<CharacterInventorySystem>();
//                characterInventory.SetBoots(boot);
//                characterInventory.SetGloves(glove);
//                characterInventory.ChangeEyeColor(PlayerSelectionMenu.EyeColors[eyeColor]);
//                characterInventory.ChangeBodyColor(PlayerSelectionMenu.BodyColors[bodyColor]);
//                characterInventory.ChangeHeadColor(PlayerSelectionMenu.HeadColors[headColor]);

//                _vsMenu.SetPlayerName(name, Count - 1);
//            }
//        }
//    }
//}
