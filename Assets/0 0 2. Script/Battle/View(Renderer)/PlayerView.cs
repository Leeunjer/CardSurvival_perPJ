using UnityEngine;



namespace CardGame
{
    public class PlayerView : MonoBehaviour
    {
        
        private Animator _anim;

        private void Awake() {
            _anim = GetComponent<Animator>();
        }


        public void Attack1()
        {
            _anim.SetTrigger("Attack1");
        }

        public void Attack2()
        {
            _anim.SetTrigger("Attack2");
        }

        public void Attack3()
        {
            _anim.SetTrigger("Attack3");
        }
        
        public void BUFF()
        {
            _anim.SetTrigger("BUFF");
        }

        public void OnGuard()
        {
            _anim.SetBool("Guard" , true);
        }

        public void UnGuard()
        {
            _anim.SetBool("Guard" , false);
        }



    }

}

