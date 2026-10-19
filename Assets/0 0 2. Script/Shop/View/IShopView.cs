using System;

namespace CardGame
{
    // GameObject에 부착하지 않는 View 계약입니다. ShopPanelView가 구현합니다.
    public interface IShopView
    {
        event Action<int> PurchaseRequested;
        event Action CloseRequested;
        event Action ExitRequested;
        void ShowGold(int gold);
        void ShowProducts(ShopItemDisplay[] products);
        void ShowMessage(string message);
        void SetPanelVisible(bool visible);
        void SetBusy(bool busy);
    }
}
