using UnityEngine;
using static UNO.enums.Enums;


namespace UNO.scriptables
{
    [CreateAssetMenu(fileName = "NewCard", menuName = "UNO/Card Data")]
    public class CardData : ScriptableObject
    {
        public int id;                  // Unique ID (important for multiplayer sync)
        public CardType cardType;       // Number, Skip, Reverse, etc.
        public CardColor cardColor;     // Red, Blue, Green, Yellow, None
        public int numberValue;         // 0–9 (Only used if CardType == Number)

        public Sprite cardFrontSprite;  // Card artwork
    }
}