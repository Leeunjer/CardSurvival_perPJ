using DG.Tweening;
using UnityEngine;

namespace CardGame
{
    public class TileRenderer : MonoBehaviour, IHoverable
    {
        private Vector3 _defaultTilePos;
        private bool _initialized;
        private bool _isHovered;
        private Tween _moveTween;

        [SerializeField] private float _fallDistance = 5f;
        [SerializeField] private float _fallDuration = 0.6f;

        public Vector2Int tileOffset { get; set; }
        public bool IsFallen { get; private set; }

        private void Start()
        {
            InitializePosition();
        }

        private void InitializePosition()
        {
            if (_initialized) return;
            _defaultTilePos = transform.position;
            _initialized = true;
        }

        public void OnClicked()
        {
            OnHoverExit();
        }

        public void OnHoverEnter()
        {
            if (IsFallen || _isHovered) return;
            InitializePosition();
            _isHovered = true;
            _moveTween?.Kill();
            _moveTween = transform.DOMove(_defaultTilePos + Vector3.up, 0.3f);
        }

        public void OnHoverExit()
        {
            if (IsFallen || !_isHovered) return;
            _isHovered = false;
            _moveTween?.Kill();
            _moveTween = transform.DOMove(_defaultTilePos, 0.3f);
        }

        public void Fall()
        {
            if (IsFallen) return;
            InitializePosition();
            IsFallen = true;
            _isHovered = false;
            _moveTween?.Kill();
            foreach (Collider tileCollider in GetComponentsInChildren<Collider>(true))
            {
                tileCollider.enabled = false;
            }
            _moveTween = transform.DOMove(_defaultTilePos + Vector3.down * _fallDistance, _fallDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() => gameObject.SetActive(false));
        }

        public void RestoreFallen()
        {
            IsFallen = true;
            _isHovered = false;
            _moveTween?.Kill();
            foreach (Collider tileCollider in GetComponentsInChildren<Collider>(true))
                tileCollider.enabled = false;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _moveTween?.Kill();
        }
    }
}
