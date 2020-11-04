using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class MenuOptionUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textView;
        [SerializeField] private Button _button;

        public void SetText(string text)
        {
            _textView.text = text;
        }
    }
}
