using System;
using UnityEngine;
using UNO.enums;
namespace UNO.utiles
{
    public class GameConstants : MonoBehaviour
    {
        public static GameConstants Instance;
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            DontDestroyOnLoad(this.gameObject);
        }


        public static Enums.CardColor ActiveColor = Enums.CardColor.None;
        public static int NumberofPlayerInGame;
        public static int MaxNumberOfPlayer;

        public static int CurrntTurnPlayerIndex;
        public static int MainPlayerIndex;
        public static string MainPlayerId;
    }
}
