using System.Collections.Generic;
using UnityEngine;
using UNO.scriptables;

namespace UNO.core
{
    public class PlayerState : MonoBehaviour
    {
        public string PlayerName { get; private set; }

        private List<CardData> _hand = new List<CardData>();

        public IReadOnlyList<CardData> Hand => _hand;

        public PlayerState(string name)
        {
            PlayerName = name;
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
    }
}
