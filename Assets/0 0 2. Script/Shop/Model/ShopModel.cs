using System;
using UnityEngine;

namespace CardGame
{
    // 부착하지 않는 구매 규칙 클래스. ShopRuntimeState가 카탈로그별로 생성/보관합니다.
    public sealed class ShopModel
    {
        private readonly ShopRuntimeState _state;
        private readonly CardItem[] _cards;
        private readonly int[] _prices;
        private readonly int[] _stocks;
        public int Gold => _state.Gold;
        public int ProductCount => _cards.Length;

        internal ShopModel(ShopCatalogData catalog, ShopRuntimeState state)
        {
            _state = state;
            ShopProductDefinition[] products = catalog.Products ?? Array.Empty<ShopProductDefinition>();
            _cards = new CardItem[products.Length];
            _prices = new int[products.Length];
            _stocks = new int[products.Length];
            for (int i = 0; i < products.Length; i++)
            {
                ShopProductDefinition product = products[i];
                if (product == null || product.Card == null || product.purchasePrice < 0 || product.stock < 0)
                    throw new ArgumentException($"Shop Catalog의 Products[{i}] 카드/가격/재고를 확인하세요.");
                _cards[i] = product.Card;
                _prices[i] = product.purchasePrice;
                _stocks[i] = product.stock;
            }
        }

        public ShopPurchaseResult CheckPurchase(int productId)
        {
            if (productId < 0 || productId >= ProductCount) return ShopPurchaseResult.InvalidProduct;
            if (_stocks[productId] <= 0) return ShopPurchaseResult.SoldOut;
            if (Gold < _prices[productId]) return ShopPurchaseResult.NotEnoughGold;
            if (!_state.CanAdd(_cards[productId])) return ShopPurchaseResult.InvalidDeck;
            return ShopPurchaseResult.Success;
        }

        public ShopPurchaseResult TryPurchase(int productId)
        {
            ShopPurchaseResult result = CheckPurchase(productId);
            if (result != ShopPurchaseResult.Success) return result;
            _state.Purchase(_cards[productId], _prices[productId]);
            _stocks[productId]--;
            return ShopPurchaseResult.Success;
        }

        public ShopItemDisplay GetDisplay(int productId)
        {
            if (productId < 0 || productId >= ProductCount) throw new ArgumentOutOfRangeException(nameof(productId));
            return new ShopItemDisplay(productId, _cards[productId].cardName, _cards[productId].icon,
                _prices[productId], _stocks[productId], CheckPurchase(productId) == ShopPurchaseResult.Success);
        }
    }

    // 별도 부착하지 않는 구매 결과/화면 전달 데이터입니다.
    public enum ShopPurchaseResult { Success, InvalidProduct, NotEnoughGold, SoldOut, InvalidDeck }

    public readonly struct ShopItemDisplay
    {
        public int Id { get; }
        public string Name { get; }
        public Sprite Icon { get; }
        public int Price { get; }
        public int Stock { get; }
        public bool CanBuy { get; }
        public ShopItemDisplay(int id, string name, Sprite icon, int price, int stock, bool canBuy)
        {
            Id = id; Name = name; Icon = icon; Price = price; Stock = stock; CanBuy = canBuy;
        }
    }
}
