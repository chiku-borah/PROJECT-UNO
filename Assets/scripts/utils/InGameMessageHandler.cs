using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections;

namespace UNO.utiles
{
    public class InGameMessageHandler : MonoBehaviour
    {
        public static InGameMessageHandler Instance;

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private float _showDuration = 5f;
        [SerializeField] private float _animDuration = 0.4f;
        [SerializeField] private float _moveOffset = 80f;

        private CanvasGroup _canvasGroup;
        private RectTransform _rect;

        private Coroutine _messageRoutine;
        private Sequence _sequence;

        private Vector2 _defaultPos;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            _rect = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();

            if (_canvasGroup == null)
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();

            _defaultPos = _rect.anchoredPosition;
            _canvasGroup.alpha = 0;
        }

        [ContextMenu("SHOWMESSAGE")]
        public void ShowMessage(string message)
        {
            _text.text = message;

            if (_messageRoutine != null)
                StopCoroutine(_messageRoutine);

            if (_sequence != null && _sequence.IsActive())
                _sequence.Kill();

            _messageRoutine = StartCoroutine(MessageRoutine());
        }

        private IEnumerator MessageRoutine()
        {
            _rect.anchoredPosition = _defaultPos + Vector2.up * _moveOffset;

            _sequence = DOTween.Sequence();

            _sequence.Append(_canvasGroup.DOFade(1f, _animDuration));
            _sequence.Join(_rect.DOAnchorPos(_defaultPos, _animDuration).SetEase(Ease.OutCubic));

            yield return _sequence.WaitForCompletion();

            yield return new WaitForSeconds(_showDuration);

            PlayCloseAnimation();
        }

        private void PlayCloseAnimation()
        {
            if (_sequence != null && _sequence.IsActive())
                _sequence.Kill();

            _sequence = DOTween.Sequence();

            _sequence.Append(_canvasGroup.DOFade(0f, _animDuration));
            _sequence.Join(_rect.DOAnchorPos(_defaultPos + Vector2.up * _moveOffset, _animDuration).SetEase(Ease.InCubic));
        }
    }
}