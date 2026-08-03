
using DG.Tweening;
using UnityEngine;

namespace CardGame
{
    public class HexScript : MonoBehaviour , IHoverable
    {

    
    private bool _isPlayerOnHere = false;
    private bool _isHover = false;
        private Vector3 _tilePosDown, _tilePosUP;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tilePosDown = transform.position;
        _tilePosUP = transform.position + Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void OnHoverEnter()
    {
        
        if(_isPlayerOnHere)return;
        if(_isHover) return;
        _isHover = true;
        gameObject.transform.DOMove(_tilePosUP, 0.3f);
        GamePlayFieldManager.OnHoverExit += OnHoverExit;
        
    }
    
    public void OnHoverExit()
    {
        if(!_isHover) return;
        _isHover = false;
        gameObject.transform.DOMove(_tilePosDown, 0.3f);
        GamePlayFieldManager.OnHoverExit -= OnHoverExit;
    }

    public void OnClicked()
    {
        if (_isPlayerOnHere)
        {
            _isPlayerOnHere = false;
        }
        else
        {
            _isPlayerOnHere = true;
        }
    }
    }
}

