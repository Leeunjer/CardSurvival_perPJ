using System;
using UnityEngine;

namespace CardGame
{
    // 부착하지 않는 설정 에셋. Project > Create > CardGame > Shop Catalog로 생성한 뒤
    // Shop 씬 Manager의 ShopSceneController > Catalog에 연결합니다.
    // 같은 종류의 카드는 항상 같은 CardData 에셋과 CardIndex를 사용하세요.
    // 기본 덱에 있는 카드를 판매할 때도 기본 덱 원본 에셋을 지정해야 수량이 합쳐집니다.
    [CreateAssetMenu(fileName = "ShopCatalog", menuName = "CardGame/Shop Catalog")]
    public sealed class ShopCatalogData : ScriptableObject
    {
        [SerializeField] private ShopProductDefinition[] _products = Array.Empty<ShopProductDefinition>();
        public ShopProductDefinition[] Products => _products;
    }

    // ShopCatalogData의 Products에서 설정하는 상품 한 개. 별도 부착하지 않습니다.
    [Serializable]
    public sealed class ShopProductDefinition
    {
        public CardData cardData;
        [Min(0)] public int cardIndex;
        [Min(0)] public int purchasePrice = 10;
        [Min(0)] public int stock = 1;
        public CardItem Card => cardData != null && cardData.cards != null
            && cardIndex >= 0 && cardIndex < cardData.cards.Length ? cardData.cards[cardIndex] : null;
    }
}
