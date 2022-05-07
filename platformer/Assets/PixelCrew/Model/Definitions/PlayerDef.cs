using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int _InventorySize;
        [SerializeField] private int _maxHealth;

        public int InventorySize => _InventorySize;

        public int MaxHealth => _maxHealth;

    }
}