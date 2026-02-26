using UnityEngine;
using UNO.utiles;
using System;
using UNO.enums;
using UNO.scriptables;

namespace UNO.core
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents Instance;
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


        public Action<string ,CardData> OnPlayerPlayCard = delegate{};

        public void OnTriggerPlayerPlayCard(string playerID , CardData _card)
        {
            OnPlayerPlayCard?.Invoke(playerID, _card);
        }

    }
}
