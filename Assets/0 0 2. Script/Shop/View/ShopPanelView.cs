using System;
using System.Collections.Generic;
using CardGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 기존 컴포넌트 호환을 위해 전역 네임스페이스를 유지합니다.
// 항상 활성화된 Canvas에 부착하세요. 숨길 [Image] ShopUI 자체에는 부착하지 않습니다.
// Panel Root=[Image] ShopUI, Gold Text=Gold TMP, Message Text=별도 안내 TMP,
// Item Views=CardGrid 슬롯들을 순서대로, Close Button=패널 닫기 버튼으로 연결합니다.
// Open Button과 Exit Button은 Panel Root 바깥에 배치해 패널을 닫아도 사용할 수 있게 합니다.
// Open Button은 새로 만든 상점 열기 버튼, Exit Button은 기존 [Button]Exit입니다.
// 버튼 OnClick은 코드에서 등록하므로 Inspector에 같은 동작을 중복 연결하지 마세요.
public sealed class ShopPanelView : MonoBehaviour, IShopView
{
    [SerializeField] private GameObject _panelRoot;
    [SerializeField] private TMP_Text _goldText;
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private ShopItemView[] _itemViews = Array.Empty<ShopItemView>();
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _exitButton;
    private bool _ready;
    private bool _busy;
    public event Action<int> PurchaseRequested;
    public event Action CloseRequested;
    public event Action ExitRequested;

    public bool ValidateConfiguration(int productCount, out string error)
    {
        error = "ShopPanelView의 UI 참조와 슬롯 구성을 확인하세요.";
        if (_panelRoot == null || _goldText == null || _messageText == null
            || _openButton == null || _closeButton == null || _exitButton == null
            || _itemViews == null || _itemViews.Length < productCount) return false;
        if (transform.IsChildOf(_panelRoot.transform)
            || _openButton.transform.IsChildOf(_panelRoot.transform)
            || _exitButton.transform.IsChildOf(_panelRoot.transform))
        {
            error = "View, Open Button, Exit Button은 Panel Root 바깥에 배치하세요.";
            return false;
        }
        var unique = new HashSet<ShopItemView>();
        foreach (ShopItemView item in _itemViews)
            if (item == null || !item.IsConfigured || !unique.Add(item)) return false;
        error = null;
        return true;
    }

    private void OnEnable()
    {
        if (_openButton != null) _openButton.onClick.AddListener(OpenPanel);
        if (_closeButton != null) _closeButton.onClick.AddListener(RequestClose);
        if (_exitButton != null) _exitButton.onClick.AddListener(RequestExit);
        if (_itemViews != null)
            foreach (ShopItemView item in _itemViews)
                if (item != null) item.PurchaseRequested += RequestPurchase;
    }
    private void OnDisable()
    {
        if (_openButton != null) _openButton.onClick.RemoveListener(OpenPanel);
        if (_closeButton != null) _closeButton.onClick.RemoveListener(RequestClose);
        if (_exitButton != null) _exitButton.onClick.RemoveListener(RequestExit);
        if (_itemViews != null)
            foreach (ShopItemView item in _itemViews)
                if (item != null) item.PurchaseRequested -= RequestPurchase;
    }
    public void ShowGold(int gold) { _goldText.text = $"Gold : {gold}"; }
    public void ShowMessage(string message)
    {
        if (_messageText != null) _messageText.text = message;
    }
    public void ShowProducts(ShopItemDisplay[] products)
    {
        _ready = true;
        for (int i = 0; i < _itemViews.Length; i++)
        {
            if (i < products.Length) _itemViews[i].Render(products[i]);
            _itemViews[i].gameObject.SetActive(i < products.Length);
        }
    }
    public void SetPanelVisible(bool visible)
    {
        if (_panelRoot != null) _panelRoot.SetActive(visible);
        if (_openButton != null) _openButton.gameObject.SetActive(!visible);
    }
    public void SetBusy(bool busy)
    {
        _busy = busy;
        if (_openButton != null) _openButton.interactable = !busy;
        if (_closeButton != null) _closeButton.interactable = !busy;
        if (_exitButton != null) _exitButton.interactable = !busy;
        if (_itemViews != null)
            foreach (ShopItemView item in _itemViews)
                if (item != null) item.SetBusy(busy);
    }
    private void OpenPanel() { if (_ready && !_busy) SetPanelVisible(true); }
    private void RequestClose() { if (_ready && !_busy) CloseRequested?.Invoke(); }
    private void RequestExit() { if (!_busy) ExitRequested?.Invoke(); }
    private void RequestPurchase(int id) { if (_ready && !_busy) PurchaseRequested?.Invoke(id); }
}
