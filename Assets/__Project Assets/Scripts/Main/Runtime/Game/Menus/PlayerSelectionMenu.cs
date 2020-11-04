//using Game.Character_Inventory;
//using NaughtyAttributes;
//using Persistence.PlayFab;
//using TMPro;
//using UnityEngine;
//using UnityEngine.GameFoundation;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using Button = UnityEngine.UI.Button;

//namespace Game.Menus
//{
//    public class PlayerSelectionMenu : MonoBehaviour
//    {
//        [SerializeField] private Button _headButton;
//        [SerializeField] private Button _bodyButton;
//        [SerializeField] private Button _bootsButton;
//        [SerializeField] private Button _eyeButton;
//        [SerializeField] private Button _glovesButton;
//        [SerializeField] private Button _goButton;
//        [SerializeField] private InputField _nameInputText;

//        [Header("COLORS")]
//        [SerializeField] private Color[] _headColors;
//        [SerializeField] private Color[] _bodyColors;
//        [SerializeField] private Color[] _eyeColors;

//        [SerializeField] private string[] _gloveLabels;
//        [SerializeField] private string[] _bootLabels;

//        [Header("REFERENCES")]
//        [SerializeField] private CharacterInventorySystem _inventory;

//        private int _currentHead, _currentBody, _currentEye, _currentGlove, _currentBoot;

//        public static Color[] EyeColors;
//        public static Color[] BodyColors;
//        public static Color[] HeadColors;
//        public static int Player = 0;

//        void Start()
//        {
//            if (GameFoundation.IsInitialized)
//            {
//                LoadData();
//            }

//            var persistenceController = PlayFabPersistenceController.Instance;
//            persistenceController.As<PlayFabPersistenceController>().OnLoaded.DynamicCalls += LoadData;

//            EyeColors = _eyeColors;
//            BodyColors = _bodyColors;
//            HeadColors = _headColors;

//            LoadPlayerInventory();
//        }

//        void LoadPlayerInventory()
//        {
//            if (!GameFoundation.IsInitialized)
//            {
//                Debug.LogError("You need to initialize GameFoundation first.");
//                return;
//            }

//            var characters = InventoryManager.FindItemsByTag("Personaje");
//            if (characters.Length > 1)
//            {
//                var player1 = characters[Player];

//                string glove = player1.GetProperty("Guante");
//                string boot = player1.GetProperty("Bota");
//                int eyeColor = player1.GetProperty("EyeColor");
//                int bodyColor = player1.GetProperty("BodyColor");
//                int headColor = player1.GetProperty("HeadColor");
//                string name = player1.GetProperty("Name");

//                _nameInputText.text = name;

//                var characterInventory = _inventory;
//                characterInventory.SetBoots(boot);
//                characterInventory.SetGloves(glove);
//                characterInventory.ChangeEyeColor(PlayerSelectionMenu.EyeColors[eyeColor]);
//                characterInventory.ChangeBodyColor(PlayerSelectionMenu.BodyColors[bodyColor]);
//                characterInventory.ChangeHeadColor(PlayerSelectionMenu.HeadColors[headColor]);
//            }
//        }

//        void OnDestroy()
//        {
//            if(!PlayFabPersistenceController.Instanced)
//                return;

//            var persistenceController = PlayFabPersistenceController.Instance;
//            persistenceController.As<PlayFabPersistenceController>().OnLoaded.DynamicCalls -= LoadData;
//        }

//        [Button]
//        public void LoadData()
//        {
//            if (!GameFoundation.IsInitialized)
//            {
//                Debug.LogError("You need to initialize GameFoundation first.");
//                return;
//            }

//            var gloves = InventoryManager.catalog.FindItemsByTag("Guante");
//            var boots = InventoryManager.catalog.FindItemsByTag("Bota");

//            _gloveLabels = new string[gloves.Length];
//            _bootLabels = new string[boots.Length];

//            for (int i = 0; i < _gloveLabels.Length; i++)
//            {
//                _gloveLabels[i] = gloves[i].GetStaticProperty("SpriteLibraryLabel");
//            }

//            for (int i = 0; i < _bootLabels.Length; i++)
//            {
//                _bootLabels[i] = boots[i].GetStaticProperty("SpriteLibraryLabel");
//            }
//        }

//        [Button]
//        public void GoClick()
//        {
//            if (string.IsNullOrEmpty(_nameInputText.text))
//            {
//                Debug.Log("Please write a name");
//                return;
//            }

//            if (!GameFoundation.IsInitialized)
//            {
//                Debug.LogError("You need to initialize GameFoundation first.");
//                return;
//            }

//            var characters = InventoryManager.FindItemsByTag("Personaje");
//            if (characters.Length > 1)
//            {
//                var character = characters[Player];

//                character.SetProperty("Guante", _gloveLabels[_currentGlove]);
//                character.SetProperty("Bota", _bootLabels[_currentBoot]);
//                character.SetProperty("EyeColor", _currentEye);
//                character.SetProperty("BodyColor", _currentBody);
//                character.SetProperty("HeadColor", _currentHead);
//                character.SetProperty("Name", _nameInputText.text);
//            }

//            SceneManager.LoadScene("Fighting Screen - Test", LoadSceneMode.Single);
//        }

//        public void NextHeadColor()
//        {
//            _currentHead = ((_currentHead + 1) % _headColors.Length);

//            _inventory.ChangeHeadColor(_headColors[_currentHead]);
//        }

//        public void NextBodyColor()
//        {
//            _currentBody = ((_currentBody + 1) % _bodyColors.Length);

//            _inventory.ChangeBodyColor(_bodyColors[_currentBody]);
//        }

//        public void NextEyeColor()
//        {
//            _currentEye = ((_currentEye + 1) % _eyeColors.Length);

//            _inventory.ChangeEyeColor(_eyeColors[_currentEye]);
//        }

//        public void NextBoots()
//        {
//            _currentBoot = ((_currentBoot + 1) % _bootLabels.Length);

//            _inventory.SetBoots(_bootLabels[_currentBoot]);
//        }

//        public void NextGloves()
//        {
//            _currentGlove = ((_currentGlove + 1) % _gloveLabels.Length);

//            _inventory.SetGloves(_gloveLabels[_currentGlove]);
//        }
//    }
//}
