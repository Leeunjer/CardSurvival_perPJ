using UnityEngine;

namespace CardGame
{
    

    public class EnemyHpbar : HPBar
    {
        private Camera cam;

        void Awake()
        {
            cam =Camera.main;
        }

        



        
        void LateUpdate()
        {

            Vector3 direction = cam.transform.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        
        

    }



}