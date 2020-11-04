using UnityEngine;

namespace d4160.GameFramework.DataPersistence
{
    [CreateAssetMenu(menuName = "Game Framework/TitleData Settings", fileName = "TitleDataSettings")]
    public class TitleDataSettings : ScriptableObject
    {
        [SerializeField] private BaseTitleDataScriptable _titleDatabaseScriptable;

        public BaseTitleDataScriptable TitleDatabaseScriptable => _titleDatabaseScriptable;

        private static TitleDataSettings _titleDataSettings;

        public static TitleDataSettings Instance
        {
            get
            {
                if (!_titleDataSettings)
                {
                    _titleDataSettings = Resources.Load<TitleDataSettings>("TitleDataSettings");
                }

                if (!_titleDataSettings)
                {
                    Debug.LogError("Need a TitleDataSettings asset inside Resources folder.");
                }

                return _titleDataSettings;
            }
        }
    }
}
