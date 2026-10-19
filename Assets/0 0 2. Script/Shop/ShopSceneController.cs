using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardGame
{
    // Shop 씬의 항상 활성화된 Manager 오브젝트에 하나만 부착합니다.
    // View=Canvas의 ShopPanelView, Catalog=생성한 ShopCatalog 에셋을 연결하세요.
    // Initial Gold는 플레이어 첫 Shop 방문에만 적용됩니다. Return Scene Name은 Field입니다.
    // Intro에서 플레이어를 생성한 뒤 진입해야 합니다. Shop 단독 실행은 구매를 차단합니다.
    // Shop/Field 빌드 씬 등록과 Field의 Shop 진입 연결은 별도 작업입니다(이 스크립트가 수정하지 않음).
    public sealed class ShopSceneController : MonoBehaviour
    {
        [SerializeField] private ShopPanelView _view;
        [SerializeField] private ShopCatalogData _catalog;
        [SerializeField, Min(0)] private int _initialGold = 100;
        [SerializeField] private string _returnSceneName = "Field";
        private ShopPresenter _presenter;
        private bool _started;

        private void Start() { _started = true; Initialize(); }
        private void OnEnable() { if (_started) Initialize(); }
        private void Initialize()
        {
            if (_view == null) { Debug.LogError("ShopSceneController에 View를 연결하세요.", this); return; }
            try
            {
                if (_catalog == null) throw new InvalidOperationException("Shop Catalog를 연결하세요.");
                if (!_view.ValidateConfiguration(_catalog.Products?.Length ?? 0, out string error))
                    throw new InvalidOperationException(error);
                PlayerData player = PlayerDataManager.Instance != null ? PlayerDataManager.Instance.currentPlayer : null;
                ShopRuntimeState state = ShopRuntimeState.ForPlayer(player, _initialGold);
                _presenter = new ShopPresenter(state.GetShop(_catalog), _view, ExitToField);
            }
            catch (Exception exception)
            {
                _view.SetBusy(true);
                _view.ShowMessage(exception.Message);
                Debug.LogError($"Shop 초기화 실패: {exception.Message}", this);
            }
        }
        private void ExitToField()
        {
            if (string.IsNullOrWhiteSpace(_returnSceneName) || !Application.CanStreamedLevelBeLoaded(_returnSceneName))
            {
                _presenter.CancelExit("복귀 씬이 빌드 씬 목록에 등록되어 있지 않습니다.");
                return;
            }
            Utils.FadeOutIn(() =>
            {
                if (this == null || !isActiveAndEnabled) return;
                try { SceneManager.LoadScene(_returnSceneName); }
                catch (Exception exception) { _presenter?.CancelExit($"씬 이동 실패: {exception.Message}"); }
            });
        }
        private void OnDisable()
        {
            _presenter?.Dispose();
            _presenter = null;
            if (_view != null) _view.SetBusy(true);
        }
    }
}
