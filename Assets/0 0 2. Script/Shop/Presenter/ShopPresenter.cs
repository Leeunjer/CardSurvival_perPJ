using System;

namespace CardGame
{
    // 부착하지 않는 일반 C# 클래스. ShopSceneController가 생성하고 Dispose합니다.
    public sealed class ShopPresenter : IDisposable
    {
        private readonly ShopModel _model;
        private readonly IShopView _view;
        private readonly Action _exit;
        private bool _disposed;
        private bool _exiting;

        public ShopPresenter(ShopModel model, IShopView view, Action exit)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _exit = exit ?? throw new ArgumentNullException(nameof(exit));
            _view.SetBusy(false);
            Refresh();
            _view.ShowMessage("");
            _view.SetPanelVisible(true);
            // 최초 렌더링이 성공한 뒤 구독해 초기화 실패 시 구독이 남지 않게 합니다.
            _view.PurchaseRequested += Buy;
            _view.CloseRequested += Close;
            _view.ExitRequested += Exit;
        }
        private void Refresh()
        {
            var products = new ShopItemDisplay[_model.ProductCount];
            for (int i = 0; i < products.Length; i++) products[i] = _model.GetDisplay(i);
            _view.ShowGold(_model.Gold);
            _view.ShowProducts(products);
        }
        private void Buy(int id)
        {
            if (_disposed || _exiting) return;
            ShopPurchaseResult result = _model.TryPurchase(id);
            Refresh();
            switch (result)
            {
                case ShopPurchaseResult.Success: _view.ShowMessage("카드를 구매했습니다."); break;
                case ShopPurchaseResult.NotEnoughGold: _view.ShowMessage("골드가 부족합니다."); break;
                case ShopPurchaseResult.SoldOut: _view.ShowMessage("품절된 상품입니다."); break;
                case ShopPurchaseResult.InvalidDeck: _view.ShowMessage("플레이어 덱에 카드를 추가할 수 없습니다."); break;
                default: _view.ShowMessage("유효하지 않은 상품입니다."); break;
            }
        }
        private void Close() { if (!_disposed && !_exiting) _view.SetPanelVisible(false); }
        private void Exit()
        {
            if (_disposed || _exiting) return;
            _exiting = true;
            _view.SetBusy(true);
            _exit();
        }
        public void CancelExit(string message)
        {
            if (_disposed) return;
            _exiting = false;
            _view.SetBusy(false);
            _view.SetPanelVisible(true);
            _view.ShowMessage(message);
        }
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _view.PurchaseRequested -= Buy;
            _view.CloseRequested -= Close;
            _view.ExitRequested -= Exit;
        }
    }
}
