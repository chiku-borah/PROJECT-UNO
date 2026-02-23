using UnityEngine;
using UNO.scriptables;
using UnityEngine.UI;
namespace UNO.core
{
    public class Crads : MonoBehaviour
    {
        public CardData CardData { get; private set; }
        [SerializeField] Image _image;

        public void Initialize(CardData data)
        {
            CardData = data;
            _image.sprite = data.cardFrontSprite;
            //SetHighlight(false);
        }

    }
}
