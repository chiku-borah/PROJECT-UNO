using UnityEngine;
using UnityEngine.UI;
using UNO.enums;
using UNO.utiles;
using System;
using DG.Tweening;

namespace UNO.core

{
    public class WildCardSelectionHandler : MonoBehaviour
    {
        [SerializeField] Button[] _buttons;
        private RectTransform _rect;

        private Vector2 _startPos;

        private Enums.CardColor _chosenColor;

        private void Start()
        {
            _rect = GetComponent<RectTransform>();
            _startPos = _rect.anchoredPosition;
            GameEvents.Instance.OnPlayerPlayWild += OnPlayerPlayWildCard;
            SetupButtons();
        }


        private void OnPlayerPlayWildCard(Player player)
        {
            if (player.PlayerId == GameConstants.MainPlayerId)
            {
                MoveUp();
            }
        }

        [ContextMenu("Move Up")]
        private void MoveUp()
        {
            Debug.Log("MOVE UP");
            _rect.DOAnchorPos(_startPos + new Vector2(0, 250f), 0.7f)
                 .SetEase(Ease.InOutQuad);
        }

        [ContextMenu("Move Down")]
        private void MoveDown()
        {
            _rect.DOAnchorPos(_startPos, 0.7f)
                 .SetEase(Ease.InQuad);
        }



        private void SetupButtons()
        {
            _buttons[0].onClick.AddListener(() => SelectColor(Enums.CardColor.Red));
            _buttons[1].onClick.AddListener(() => SelectColor(Enums.CardColor.Blue));
            _buttons[2].onClick.AddListener(() => SelectColor(Enums.CardColor.Green));
            _buttons[3].onClick.AddListener(() => SelectColor(Enums.CardColor.Yellow));
        }

        private void SelectColor(Enums.CardColor color)
        {
            _chosenColor = color;

            Debug.Log("Player selected color: " + color);

            // Update active color
            GameConstants.ActiveColor = color;

            // Notify game
            GameEvents.Instance.TriggerWildColorSelected(color);
            InGameMessageHandler.Instance.ShowMessage($"THE ACTIVE COLOR IS NOW {color.ToString()}!!!");

            MoveDown();
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnPlayerPlayWild += OnPlayerPlayWildCard;
        }

    } 
}
