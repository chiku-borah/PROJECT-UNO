using System.Collections.Generic;
using UnityEngine;
using UNO.scriptables;

namespace UNO.core
{
    public class DeckManager : MonoBehaviour
    {
        public int DrawCount => _drawPile.Count;
        public int DiscardCount => _discardPile.Count;

        private Stack<CardData> _drawPile = new Stack<CardData>();
        private Stack<CardData> _discardPile = new Stack<CardData>();

        // ======================================
        // INITIALIZE DECK
        // ======================================
        public void Initialize(List<CardData> cardAssets)
        {
            _drawPile.Clear();
            _discardPile.Clear();

            List<CardData> fullDeck = BuildDeck(cardAssets);
            Shuffle(fullDeck);

            foreach (var card in fullDeck)
                _drawPile.Push(card);
        }

        // ======================================
        // BUILD DECK (You control duplicates here)
        // ======================================
        private List<CardData> BuildDeck(List<CardData> cardAssets)
        {
            // IMPORTANT:
            // If your ScriptableObjects already contain duplicates,
            // this is enough.
            // If not, duplicate them here manually.

            return new List<CardData>(cardAssets);
        }

        // ======================================
        // SHUFFLE
        // ======================================
        private void Shuffle(List<CardData> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                int randomIndex = Random.Range(i, cards.Count);
                (cards[i], cards[randomIndex]) =
                    (cards[randomIndex], cards[i]);
            }
        }

        // ======================================
        // DRAW CARD
        // ======================================
        public CardData DrawCard()
        {
            if (_drawPile.Count == 0)
                ReshuffleFromDiscard();

            if (_drawPile.Count == 0)
                return null;

            return _drawPile.Pop();
        }

        // ======================================
        // DISCARD
        // ======================================
        public void AddToDiscard(CardData card)
        {
            if (card == null) return;

            _discardPile.Push(card);
        }

        public CardData GetTopDiscard()
        {
            if (_discardPile.Count == 0)
                return null;

            return _discardPile.Peek();
        }

        // ======================================
        // RESHUFFLE LOGIC
        // ======================================
        private void ReshuffleFromDiscard()
        {
            if (_discardPile.Count <= 1)
                return;

            CardData topCard = _discardPile.Pop();

            List<CardData> temp = new List<CardData>(_discardPile);
            _discardPile.Clear();

            Shuffle(temp);

            foreach (var card in temp)
                _drawPile.Push(card);

            _discardPile.Push(topCard);
        }
    }
}