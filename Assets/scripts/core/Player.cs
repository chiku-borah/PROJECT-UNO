using System.Collections.Generic;
using UnityEngine;
using UNO.scriptables;

namespace UNO.core
{
    public class Player : MonoBehaviour
    {
        public int PlayerId { get; private set; }
        public string PlayerName { get; private set; }

        public PlayerState State { get; private set; }

        private List<CardData> _hand = new List<CardData>();
        public IReadOnlyList<CardData> Hand => _hand;

        // ===============================
        // INITIALIZE (instead of constructor)
        // ===============================
        public void Initialize(int id, string name)
        {
            PlayerId = id;
            PlayerName = name;
            State = new PlayerState(name);
        }


        public void AddCard(CardData card)
        {
            _hand.Add(card);
        }

        public void RemoveCard(CardData card)
        {
            _hand.Remove(card);
        }

        public int CardCount()
        {
            return _hand.Count;
        }

        // ===============================
        // DRAW CARDS (GameManager calls this)
        // ===============================
        public void DrawCards(int amount, DeckManager  deck)
        {
            for (int i = 0; i < amount; i++)
            {
                CardData drawnCard = deck.DrawCard();

                if (drawnCard != null)
                {
                    _hand.Add(drawnCard);
                }
            }
        }
    }
}