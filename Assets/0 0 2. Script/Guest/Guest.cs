
using UnityEngine;

namespace CardGame 
{

    public abstract class Guest : MonoBehaviour, IICardCommand
    {
        public void CardEffect(CardEffectType cardEffect)
        {
            switch (cardEffect)
            {
                case CardEffectType.Damage :
                Attack();
                break;
                    
                case CardEffectType.Heal:
                Heal();
                break;


                case CardEffectType.Draw:
                Draw();
                break;


                case CardEffectType.Block:
                Block();
                break;

            }
        }
        protected abstract void Attack();
        protected abstract void Heal();
        protected abstract void Draw();
        protected abstract void Block();
        

        
    }

}