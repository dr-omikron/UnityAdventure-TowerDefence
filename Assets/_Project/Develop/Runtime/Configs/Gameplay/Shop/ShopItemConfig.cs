using System;
using _Project.Develop.Runtime.Configs.Gameplay.Entities;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using UnityEngine;

namespace _Project.Develop.Runtime.Configs.Gameplay.Shop
{
    [Serializable]

    public class ShopItemConfig
    {
        [field: SerializeField] public EntityConfig EntityConfig { get; private set; }
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public CurrencyType CurrencyType { get; private set; }
        [field: SerializeField, Min(0)] public int Cost { get; private set; }
    }
}
