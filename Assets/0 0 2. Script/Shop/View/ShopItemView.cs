using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    // Shop 씬 Canvas > [Image] ShopUI > CardGrid의 각 카드 슬롯에 부착합니다.
    // 슬롯 자식에 이름/가격/재고 TMP_Text와 구매 Button을 만들고 아래 필드에 연결하세요.
    // Icon은 카드 그림용 Image입니다. 전투용 Card 컴포넌트는 부착하지 않습니다.
    public sealed class ShopItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private TMP_Text _stock;
        [SerializeField] private Button _buyButton;
        private int _productId;
        private bool _canBuy;
        private bool _busy;
        public event Action<int> PurchaseRequested;
        public bool IsConfigured => _icon != null && _name != null && _price != null
            && _stock != null && _buyButton != null;

        private void OnEnable()
        {
            if (_buyButton != null) _buyButton.onClick.AddListener(RequestPurchase);
        }
        private void OnDisable()
        {
            if (_buyButton != null) _buyButton.onClick.RemoveListener(RequestPurchase);
        }
        public void Render(ShopItemDisplay item)
        {
            _productId = item.Id;
            _canBuy = item.CanBuy;
            _icon.sprite = item.Icon;
            _name.text = item.Name;
            _price.text = $"{item.Price} G";
            _stock.color = Color.black;
            _stock.text = item.Stock == 0 ? "품절" : $"재고 {item.Stock}";
            _buyButton.interactable = _canBuy && !_busy;
        }
        public void SetBusy(bool busy)
        {
            _busy = busy;
            if (_buyButton != null) _buyButton.interactable = _canBuy && !busy;
        }
        private void RequestPurchase()
        {
            if (!_busy && _canBuy) PurchaseRequested?.Invoke(_productId);
        }
    }
}
