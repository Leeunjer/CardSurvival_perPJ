using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    // 부착하지 않습니다. ShopSceneController가 현재 PlayerData에 맞춰 사용합니다.
    // Shop 밖을 수정하지 않기 위한 임시 골드/재고 저장소입니다. 씬 재방문에는 유지되고,
    // 새 PlayerData/Play 시작 시 초기화됩니다. 디스크 저장은 하지 않습니다.
    // Initial Gold는 해당 플레이어의 첫 Shop 진입에만 적용됩니다.
    public sealed class ShopRuntimeState
    {
        private static ShopRuntimeState _current;
        private readonly PlayerData _player;
        private readonly CardData _runtimeDeck;
        private readonly Dictionary<CardItem, CardItem> _ownedCards = new Dictionary<CardItem, CardItem>();
        private readonly Dictionary<ShopCatalogData, ShopModel> _shops = new Dictionary<ShopCatalogData, ShopModel>();
        public int Gold { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetState()
        {
            if (_current != null && _current._runtimeDeck != null)
                UnityEngine.Object.Destroy(_current._runtimeDeck);
            _current = null;
        }

        public static ShopRuntimeState ForPlayer(PlayerData player, int initialGold)
        {
            if (player == null || player.cardDeck == null || player.cardDeck.cards == null)
                throw new ArgumentException("플레이어와 기본 덱을 먼저 초기화해야 합니다.");
            if (initialGold < 0) throw new ArgumentOutOfRangeException(nameof(initialGold));
            if (_current == null || !ReferenceEquals(_current._player, player))
            {
                ResetState();
                _current = new ShopRuntimeState(player, initialGold);
            }
            return _current;
        }

        private ShopRuntimeState(PlayerData player, int initialGold)
        {
            _player = player;
            Gold = initialGold;
            _runtimeDeck = ScriptableObject.CreateInstance<CardData>();
            _runtimeDeck.name = "Shop Runtime Deck";
            _runtimeDeck.hideFlags = HideFlags.DontSave;
            var cards = new List<CardItem>();
            foreach (CardItem source in player.cardDeck.cards)
            {
                if (source == null) continue;
                CardItem copy = CopyCard(source, source.CardCount);
                cards.Add(copy);
                _ownedCards[source] = copy;
            }
            _runtimeDeck.cards = cards.ToArray();
            player.cardDeck = _runtimeDeck;
        }

        public ShopModel GetShop(ShopCatalogData catalog)
        {
            if (catalog == null) throw new ArgumentNullException(nameof(catalog));
            if (!_shops.TryGetValue(catalog, out ShopModel model))
            {
                model = new ShopModel(catalog, this);
                _shops.Add(catalog, model);
            }
            return model;
        }

        internal bool CanAdd(CardItem source)
        {
            return ReferenceEquals(_player.cardDeck, _runtimeDeck) && source != null
                && (!_ownedCards.TryGetValue(source, out CardItem owned)
                    || (owned.CardCount >= 0 && owned.CardCount < int.MaxValue));
        }

        // 검증된 구매만 동기적으로 반영합니다. 원본 CardData/CardItem은 변경하지 않습니다.
        internal void Purchase(CardItem source, int price)
        {
            if (price < 0 || Gold < price || !CanAdd(source))
                throw new InvalidOperationException("구매 조건이 유효하지 않습니다.");
            if (_ownedCards.TryGetValue(source, out CardItem owned)) owned.CardCount++;
            else
            {
                CardItem copy = CopyCard(source, 1);
                var cards = new List<CardItem>(_runtimeDeck.cards) { copy };
                _ownedCards.Add(source, copy);
                _runtimeDeck.cards = cards.ToArray();
            }
            Gold -= price;
        }

        private static CardItem CopyCard(CardItem source, int count)
        {
            return new CardItem
            {
                cardName = source.cardName, icon = source.icon, cost = source.cost,
                effactType = source.effactType, CardCount = count,
                CardMaxCounct = source.CardMaxCounct,
                // 효과 정의는 공유하되 상점에서는 실행/변경하지 않습니다.
                cardEffect = source.cardEffect
            };
        }
    }
}
