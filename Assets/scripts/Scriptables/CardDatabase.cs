using UnityEngine;
using System.Collections.Generic;
using UNO.scriptables;

[CreateAssetMenu(menuName = "UNO/Card Database")]
public class CardDatabase : ScriptableObject
{
    public List<CardData> allCards;
}