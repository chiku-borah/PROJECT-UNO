namespace UNO.enums
{
    public class Enums 
    {
        public enum CardType
        {
            Number = 0,
            Skip = 1,
            Reverse = 2,
            DrawTwo = 3,
            Wild = 4,
            WildDrawFour = 5
        }

        public enum CardColor
        {
            Red = 0,
            Blue = 1,
            Green = 2,
            Yellow = 3,
            None = 4   // For Wild cards
        }

        public enum GameState
        {
            WaitingForPlayers = 0,
            DealingCards = 1,
            PlayerTurn = 2,
            ResolvingCardEffect = 3,
            NextTurn = 4,
            GameOver = 5
        }

        public enum GameDirection
        {
            Clockwise = 1,
            CounterClockwise = -1
        }

        public enum TurnActionResult
        {
            None = 0,
            Skip = 1,
            Reverse = 2,
            DrawTwo = 3,
            DrawFour = 4
        }
    }
}