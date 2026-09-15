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
        public int CardMaxCounct;

        public DetailCardEffect cardEffect;

        public void UseEffect()
        {
            cardEffect.Excute();
        }
    }

    public abstract class DetailCardEffect
    {
        public abstract void Excute();
    }

    [CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
    public class CardData : ScriptableObject
    {
        public CardItem[] cards;
    }

}

