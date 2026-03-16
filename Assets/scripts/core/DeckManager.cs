using System.Collections.Generic;
using UnityEngine;
using UNO.scriptables;
using static UNO.enums.Enums;

namespace UNO.core
{
    public class DeckManager : MonoBehaviour
    {
        [SerializeField] Cards _cardPrefab;
        public int DrawCount => _drawPile.Count;
        public int DiscardCount => _discardPile.Count;

        private Stack<Cards> _drawPile = new Stack<Cards>();
        private Stack<Cards> _discardPile = new Stack<Cards>();

        // ======================================
        // INITIALIZE DECK
        // ======================================
        public void Initialize(List<CardData> cardAssets)
        {
            _drawPile.Clear();
            _discardPile.Clear();

            List<Cards> fullDeck = BuildDeck(cardAssets);
            Shuffle(fullDeck);

            foreach (var card in fullDeck)
                _drawPile.Push(card);
        }

        // ======================================
        // BUILD DECK (You control duplicates here)
        // ======================================

        GameObject _obj;
        private List<Cards> BuildDeck(List<CardData> cardAssets)
        {
            // IMPORTANT:
            // If your ScriptableObjects already contain duplicates,
            // this is enough.
            // If not, duplicate them here manually.

             List<Cards> _cards = new List<Cards>();
            foreach (var data in cardAssets)
            {
                _obj = Instantiate(_cardPrefab.gameObject, this.transform);
                Cards c = _obj.GetComponent<Cards>();
                c.Initialize(data);
                _obj.SetActive(false);
                _obj.name = GetShortCardName(data);
                _cards.Add(c);
            }
                return _cards;
        }

        private string GetShortCardName(CardData data)
        {
            string color = data.cardColor.ToString()[0].ToString(); // R,G,B,Y
            string type = "";

            switch (data.cardType)
            {
                case CardType.Number:
                    type = data.numberValue.ToString();
                    break;

                case CardType.Skip:
                    type = "S";
                    break;

                case CardType.Reverse:
                    type = "R";
                    break;

                case CardType.DrawTwo:
                    type = "+2";
                    break;

                case CardType.Wild:
                    return "W";

                case CardType.WildDrawFour:
                    return "W+4";
            }

            return $"{color}_{type}";
        }

        // ======================================
        // SHUFFLE
        // ======================================
        private void Shuffle(List<Cards> cards)
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
        public Cards DrawCard()
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
        public void AddToDiscard(Cards card)
        {
            if (card == null) return;

            _discardPile.Push(card);
        }

        public Cards GetTopDiscard()
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

            Cards topCard = _discardPile.Pop();

            List<Cards> temp = new List<Cards>(_discardPile);
            _discardPile.Clear();

            Shuffle(temp);

            foreach (var card in temp)
                _drawPile.Push(card);

            _discardPile.Push(topCard);
        }
    }
}