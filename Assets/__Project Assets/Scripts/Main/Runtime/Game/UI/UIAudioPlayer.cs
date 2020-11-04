using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class UIAudioPlayer : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _clickClip;
        [SerializeField] private AudioClip _hoverClip;

        public void OnPointerClick(PointerEventData eventData)
        {
            _audioSource?.PlayOneShot(_clickClip);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _audioSource?.PlayOneShot(_hoverClip);
        }
    }
}
