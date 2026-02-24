using System.Collections.Generic;
using UnityEngine;
using UNO.enums;
using UNO.scriptables;

namespace UNO.core
{
    public class DeckManager : MonoBehaviour
    {
        public class DeckSystem
        {
            private Stack<CardData> _drawPile = new Stack<CardData>();
            private Stack<CardData> _discardPile = new Stack<CardData>();

            public void Initialize(List<CardData> cardTypes)
            {
                List<CardData> fullDeck = BuildDeck(cardTypes);
                Shuffle(fullDeck);

                foreach (var card in fullDeck)
                    _drawPile.Push(card);
            }

            private List<CardData> BuildDeck(List<CardData> cardAssets)
            {
                return new List<CardData>(cardAssets);
            }

            private void Shuffle(List<CardData> list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    int randomIndex = Random.Range(i, list.Count);
                    (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
                }
            }

            public CardData DrawCard()
            {
                if (_drawPile.Count == 0)
                    ReshuffleFromDiscard();

                return _drawPile.Count > 0 ? _drawPile.Pop() : null;
            }

            public void AddToDiscard(CardData card)
            {
                _discardPile.Push(card);
            }

            public CardData GetTopDiscard()
            {
                return _discardPile.Count > 0 ? _discardPile.Peek() : null;
            }

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

            public int DrawCount => _drawPile.Count;
            public int DiscardCount => _discardPile.Count;
        }
    }
}
