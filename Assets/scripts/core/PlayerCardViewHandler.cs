using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UNO.scriptables;

namespace UNO.core
{
    public class PlayerCardViewHandler : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private RectTransform _cardContainer;

        private readonly List<Cards> _cards = new();

        [SerializeField] private int _maxCardsPerRow = 10;
        [SerializeField] private float _rowHeight = 200f;

        [SerializeField] private float _horizontalPadding = 80f;
        [SerializeField] private float _verticalPadding = 60f;

        [SerializeField] private float _spacingMultiplier = 1.2f;
        [SerializeField] private float _rowSpacing = 40f;

        [SerializeField] private float _cardSize = 1f;
        [SerializeField] private float _fanAngle = 8f;

        // ==========================
        // Cached Layout Variables
        // ==========================

        private int _count;
        private float _containerWidth;
        private float _containerHeight;

        private float _usableWidth;
        private float _usableHeight;

        private int _rows;
        private float _totalRowsHeight;
        private float _startY;

        private int _cardIndex;
        private int _cardsInRow;

        private float _spacing;
        private float _startX;
        private float _centerOffset;

        private float _x;
        private float _y;
        private float _angle;

        private RectTransform _rect;
        private Vector2 _targetPos;

        // ==========================
        // INITIALIZE
        // ==========================

        public void Initialize(Player player)
        {
            _player = player;
            RefreshHand();
        }

        // ==========================
        // REFRESH HAND
        // ==========================

        [ContextMenu("RefreshHand")]
        public void RefreshHand()
        {
            ClearHand();

            List<Cards> _handCards = _player.HandCards;

            for (int i = 0; i < _handCards.Count; i++)
                AddCard(_handCards[i]);

            LayoutCards();
        }

        // ==========================
        // ADD CARD
        // ==========================

        private void AddCard(Cards card)
        {
            _cards.Add(card);

            RectTransform _rect = card.RectTransform;

            _rect.SetParent(_cardContainer, false);
            _rect.gameObject.SetActive(true);
        }

        // ==========================
        // CARD LAYOUT
        // ==========================

        [ContextMenu("LayoutCards")]
        private void LayoutCards()
        {
            _count = _cards.Count;
            if (_count == 0) return;

            _containerWidth = _cardContainer.rect.width;
            _containerHeight = _cardContainer.rect.height;

            _usableWidth = _containerWidth - _horizontalPadding * 2;
            _usableHeight = _containerHeight - _verticalPadding * 2;

            _rows = Mathf.CeilToInt((float)_count / _maxCardsPerRow);

            _totalRowsHeight = _rows * _rowHeight + (_rows - 1) * _rowSpacing;
            _totalRowsHeight = Mathf.Min(_totalRowsHeight, _usableHeight);

            _startY = (_totalRowsHeight * 0.5f) - (_rowHeight * 0.5f);

            _cardIndex = 0;

            for (int row = 0; row < _rows; row++)
            {
                _cardsInRow = Mathf.Min(_maxCardsPerRow, _count - _cardIndex);

                _spacing = (_usableWidth / (_cardsInRow + 1)) * _spacingMultiplier;
                _startX = -((_cardsInRow - 1) * _spacing) * 0.5f;

                _centerOffset = (_cardsInRow - 1) * 0.5f;

                _y = _startY - row * (_rowHeight + _rowSpacing);

                for (int i = 0; i < _cardsInRow; i++)
                {
                    _rect = _cards[_cardIndex].RectTransform;

                    _rect.localScale = Vector3.one * _cardSize;

                    _x = _startX + i * _spacing;
                    _targetPos = new Vector2(_x, _y);

                    _angle = (i - _centerOffset) * _fanAngle;

                    _rect.DOAnchorPos(_targetPos, 0.25f).SetEase(Ease.OutQuad);
                    // _rect.DORotateQuaternion(Quaternion.Euler(0,0,_angle),0.25f);

                    _cardIndex++;
                }
            }
        }

        // ==========================
        // CLEAR
        // ==========================

        private void ClearHand()
        {
            _cards.Clear();
        }
    }
}