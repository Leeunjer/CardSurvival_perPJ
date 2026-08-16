using DG.Tweening;
using UnityEngine;

public class TileRenderer : MonoBehaviour , IHoverable
{
    private Vector3 _tileUp;
    private Vector3 _defaultTilePos;

    
    void Start()
    {
        _defaultTilePos = transform.position;
        _tileUp = _defaultTilePos + Vector3.up;
    }


    public void OnClicked()
    {
        transform.DOMove(_defaultTilePos , 0.3f);
    }

    public void OnHoverEnter()
    {
        transform.DOMove(_tileUp , 0.3f);
    }

    public void OnHoverExit()
    {
        transform.DOMove(_defaultTilePos , 0.3f);
    }



}
