using UnityEngine;
namespace CardGame
{
    public class Cube : Guest
    {
        protected override void Attack()
        {
            Debug.Log($"{gameObject.name} + Attack");
        }

        protected override void Block()
        {
            Debug.Log($"{gameObject.name} + Block");
            
        }

        protected override void Draw()
        {
            Debug.Log($"{gameObject.name} + Draw");
            
        }
        
            

        protected override void Heal()
        {
            Debug.Log($"{gameObject.name} + Heal");
            
        }

        
    }
}

