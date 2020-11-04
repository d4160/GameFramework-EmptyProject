using d4160.GameFramework.SceneManagement;
using NaughtyAttributes;
using UnityEngine;

namespace Scene_Management
{
    public class SceneGroup : MonoBehaviour
    {
        [SerializeField] private SceneListDefinition _scenes;

        void Start()
        {
            LoadScenes();
        }

        [Button]
        public void LoadScenes()
        {
            _scenes.LoadScenesAfterAnother();
        }
    }
}
