using UNO.scriptables;
using static UNO.enums.Enums;

namespace UNO.core
{
    public class RuleHandler
    {
        private int _stackedDrawAmount = 0;
        private CardType _stackingType;

        public bool IsStackingActive => _stackedDrawAmount > 0;

        // =========================
        // VALID MOVE CHECK
        // =========================
        public bool IsValidMove(CardData playedCard, CardData topCard)
        {
            // If stacking active → only same draw type allowed
            if (IsStackingActive)
            {
                return playedCard.cardType == _stackingType;
            }

            // Wild cards always valid
            if (playedCard.cardType == CardType.Wild ||
                playedCard.cardType == CardType.WildDrawFour)
                return true;

            // Match color
            if (playedCard.cardColor == topCard.cardColor)
                return true;

            // Match number (only if both are number cards)
            if (playedCard.cardType == CardType.Number &&
                topCard.cardType == CardType.Number &&
                playedCard.numberValue == topCard.numberValue)
                return true;

            // Match action type (Skip on Skip, Reverse on Reverse, DrawTwo on DrawTwo)
            if (playedCard.cardType == topCard.cardType)
                return true;

            return false;
        }

        // =========================
        // EFFECT EVALUATION
        // =========================
        public CardEffectResult EvaluateEffect(CardData card)
        {
            CardEffectResult result = new CardEffectResult();

            switch (card.cardType)
            {
                case CardType.Skip:
                    result.SkipNextPlayer = true;
                    break;

                case CardType.Reverse:
                    result.ReverseDirection = true;
                    break;

                case CardType.DrawTwo:
                    _stackedDrawAmount += 2;
                    _stackingType = CardType.DrawTwo;
                    result.IsStackingActive = true;
                    break;

                case CardType.WildDrawFour:
                    _stackedDrawAmount += 4;
                    _stackingType = CardType.WildDrawFour;
                    result.IsStackingActive = true;
                    break;
            }

            return result;
        }

        // =========================
        // STACK RESOLUTION
        // =========================
        public int ResolveStackIfCannotContinue()
        {
            int amount = _stackedDrawAmount;
            _stackedDrawAmount = 0;
            return amount;
        }
    }
}