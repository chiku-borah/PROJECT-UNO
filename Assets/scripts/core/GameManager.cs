using System.Collections.Generic;
using UnityEngine;
using UNO.scriptables;
using UNO.enums;
using static UNO.enums.Enums;
using UNO.utiles;
using System;
namespace UNO.core
{
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DeckManager _deck;
        [SerializeField] private CardDatabase _allCardAssets;
        [SerializeField] private List<Player> _players;

        private RuleHandler _ruleHandler;

        private int _currentPlayerIndex = 0;
        private int _direction = 1;

        private GameState _currentState;
        private CardColor _activeColor;


        //Bools
        private bool _isWildColorSelected = true;
        // ======================================
        // UNITY
        // ======================================
        private void Awake()
        {
            _ruleHandler = new RuleHandler();
        }

        private void Start()
        {
            StartGame();

            GameEvents.Instance.OnWildColorSelected += OnWildSelected;
        }

        private void OnWildSelected(CardColor color)
        {
            _isWildColorSelected = true;
        }

        // ======================================
        // GAME START
        // ======================================
        private void StartGame()
        {
            _currentState = GameState.Setup;

            _deck.Initialize(_allCardAssets.allCards);
            DealCards(7);
            Cards firstCard = _deck.DrawCard();
            _deck.AddToDiscard(firstCard);

            _currentState = GameState.PlayerTurn;

            Debug.Log("Game Started");
        }

        private void DealCards(int amount)
        {
            foreach (var player in _players)
            {
                for (int i = 0; i < amount; i++)
                {
                    player.AddCard(_deck.DrawCard());
                }
            }
        }

        // ======================================
        // PLAY CARD
        // ======================================
        public void TryPlayCard(Player player, Cards card)
        {
            if (_currentState != GameState.PlayerTurn)
                return;

            if (!IsCurrentPlayer(player))
                return;

            if (!_isWildColorSelected) return;

            Cards topCard = _deck.GetTopDiscard();
            GameConstants.ActiveColor = topCard.CardData.cardColor;
            if (!_ruleHandler.IsValidMove(card.CardData, topCard.CardData, _activeColor))
            {
                Debug.Log("Invalid Move");
                return;
            }



            _currentState = GameState.ResolvingEffect;
            player.RemoveCard(card);
            _deck.AddToDiscard(card);
            CardEffectResult effect = _ruleHandler.EvaluateEffect(card.CardData);
            HandleEffect(effect);
            HandleDrawIfCannotStack(player);
            if (player.CardCount() == 0)
            {
                _currentState = GameState.GameOver;
                Debug.Log(player.PlayerName + " Wins!");
                return;
            }

            _currentState = GameState.PlayerTurn;
            MoveToNextPlayer();
        }

        // ======================================
        // HANDLE EFFECT
        // ======================================
        private void HandleEffect(CardEffectResult effect)
        {
            if (effect.ReverseDirection)
                _direction *= -1;

            if (effect.SkipNextPlayer)
                MoveToNextPlayer();

            if (effect.ColorWild)
            {
                _isWildColorSelected = false;
                GameEvents.Instance.TriggerPlayerPlayWild(GetCurrentPlayer());
                return;
            }


        }

        // ======================================
        // STACK DRAW PHASE
        // ======================================
        public void HandleDrawIfCannotStack(Player player)
        {
            if (_currentState != GameState.PlayerTurn)
                return;

            if (!_ruleHandler.IsStackingActive)
                return;

            _currentState = GameState.Drawing;

            int drawAmount = _ruleHandler.ResolveStackIfCannotContinue();

            for (int i = 0; i < drawAmount; i++)
            {
                player.AddCard(_deck.DrawCard());
            }

            _currentState = GameState.PlayerTurn;
            // MoveToNextPlayer();
        }

        // ======================================
        // TURN CONTROL
        // ======================================
        private void MoveToNextPlayer()
        {
            _currentPlayerIndex += _direction;

            if (_currentPlayerIndex >= _players.Count)
                _currentPlayerIndex = 0;

            if (_currentPlayerIndex < 0)
                _currentPlayerIndex = _players.Count - 1;

            Debug.Log("Current Player: " + _players[_currentPlayerIndex].PlayerName);
        }

        private bool IsCurrentPlayer(Player player)
        {
            return _players[_currentPlayerIndex] == player;
        }

        public Player GetCurrentPlayer()
        {
            return _players[_currentPlayerIndex];
        }

        public GameState GetGameState()
        {
            return _currentState;
        }


        private void OnDestroy()
        {
            GameEvents.Instance.OnWildColorSelected -= OnWildSelected;
        }
    }

}