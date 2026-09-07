using DG.Tweening;
using UnityEngine;



namespace CardGame
{
[RequireComponent(typeof(SpriteRenderer))]
    public class TileEventRenderer : MonoBehaviour
    {
        public Vector2Int TileOffset;

        public GameObject tile;

        

        private SpriteRenderer _spriteRenderer;



        void OnEnable()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
        }
        void Start()
        {
        }


        public void MoveEventRenderer(GameObject dirPos , Vector2Int offset)
        {   
            tile = dirPos;
            TileOffset = offset;
            gameObject.transform.DOMove(tile.transform.position + new Vector3(0,0.6f,0) , 0.7f); 
            transform.SetParent(dirPos.transform);     
        }

        public void SetSprite(Sprite eventsptire)
        {
            
            _spriteRenderer.sprite = eventsptire;
            
        }
        public void DestroyThisObjct()
        {
           Destroy(gameObject); 
        }
    }

}

