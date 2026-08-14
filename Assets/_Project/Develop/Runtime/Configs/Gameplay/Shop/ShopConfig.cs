using System.Collections.Generic;
using UnityEngine;

namespace _Project.Develop.Runtime.Configs.Gameplay.Shop
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Shop/NewShopConfig", fileName = "ShopConfig")]
    public class ShopConfig : ScriptableObject
    {
        [SerializeField] private List<ShopItemConfig> _items;

        public IReadOnlyList<ShopItemConfig> Items => _items;
    }
}
