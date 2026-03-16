using System.Collections.Generic;
using UnityEngine;
using UNO.scriptables;

namespace UNO.core
{
    public class Player : MonoBehaviour
    {
        public string PlayerId { get; private set; }
        public string PlayerName { get; private set; }

        public PlayerState State { get; private set; }

        public List<Cards> HandCards => _hand;

        private List<Cards> _hand = new List<Cards>();
        public IReadOnlyList<Cards> Hand => _hand;

        // ===============================
        // INITIALIZE (instead of constructor)
        // ===============================
        public void Initialize(string id, string name)
        {
            PlayerId = id;
            PlayerName = name;
            State = new PlayerState(name);
        }


        public void AddCard(Cards card)
        {
            _hand.Add(card);
        }

        public void RemoveCard(Cards card)
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
                Cards drawnCard = deck.DrawCard();

                if (drawnCard != null)
                {
                    _hand.Add(drawnCard);
                }
            }
        }
    }
}