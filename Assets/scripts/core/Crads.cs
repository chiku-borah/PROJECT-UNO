using UnityEngine;
using UNO.scriptables;
using UnityEngine.UI;
namespace UNO.core
{
    public class Crads : MonoBehaviour
    {
        
        [SerializeField] Image _image;
        [SerializeField] Button _btn;

        private Player _ownPlayer;
        public CardData CardData { get; private set; }
        public void Initialize(CardData data)
        {
            CardData = data;
            _image.sprite = data.cardFrontSprite;
            _btn.onClick.AddListener(OnClickCard);
            //SetHighlight(false);
        }

        private void OnClickCard()
        {
           GameEvents.Instance.OnTriggerClickedCard(_ownPlayer, CardData);
        }

    }
}
