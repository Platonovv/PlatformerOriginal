using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Player
{
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int _InventorySize;
        [SerializeField] private int _maxHealth;
        [SerializeField] private StatDef[] _stats;

        public int InventorySize => _InventorySize;

        public StatDef[] Stats => _stats;

        public StatDef GetStat(StatId id) => _stats.FirstOrDefault(x => x.ID == id);
    }
}