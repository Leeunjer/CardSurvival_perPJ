using System;
using UnityEngine;
namespace CardGame
{

    [Serializable]
    public class CardItem
    {
        public string cardName;
        public Sprite icon;

        public int cost;

        public CardEffectType effactType = CardEffectType.Damage;
        public int CardCount;
    }

    [CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
    public class CardData : ScriptableObject
    {
        public CardItem[] cards;
    }

}

