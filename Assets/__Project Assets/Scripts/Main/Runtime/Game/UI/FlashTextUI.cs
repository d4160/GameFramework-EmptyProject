using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class FlashTextUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textComp;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _distanceFromOrigin;
        [SerializeField] private Ease _easeType = Ease.InCubic;

        private FlashTextUIManager _manager;

        public FlashTextUIManager Manager
        {
            get => _manager;
            set => _manager = value;
        }

        [Button]
        public void Show()
        {
            Show(Vector3.zero, "14", Color.blue);
        }

        public void Show(Vector3 worldSpace, string value, Color toColor)
        {
            var screenSpace = Camera.main.WorldToScreenPoint(worldSpace);

            _textComp.text = value;
            _textComp.alpha = 0f;
            _textComp.color = Color.white;
            _textComp.transform.position = screenSpace;
            _textComp.transform.localScale = Vector3.zero;
            _textComp.gameObject.SetActive(true);

            Sequence seq = DOTween.Sequence().Pause();

            var scaleIn = _textComp.transform.DOScale(Vector3.one, _duration * .25f);
            var moveUp = _textComp.transform.DOMoveY(screenSpace.y + _distanceFromOrigin, _duration * .75f);
            var fadeIn = DOTween.To(() => _textComp.alpha, (v) => _textComp.alpha = v, 255, _duration * .5f);
            var colorIn = DOTween.To(() => _textComp.color, (v) => _textComp.color = v, toColor, _duration * .5f);

            
            var fadeOut = DOTween.To(() => _textComp.alpha, (v) => _textComp.alpha = v, 0, _duration * .9f).Pause();
            var colorOut = DOTween.To(() => _textComp.color, (v) => _textComp.color = v, Color.white, _duration * .95f).Pause();
            var scaleOut = _textComp.transform.DOScale(Vector3.zero, _duration).OnStart(
                () =>
                {
                    fadeOut.Play();
                    colorOut.Play();
                }
            );

            seq.Append(moveUp);

            seq.AppendInterval(_duration * .5f);

            seq.Append(scaleOut);

            seq.SetEase(_easeType).OnComplete(() => Manager.Remove(this));

            seq.Play();
        }
    }
}
