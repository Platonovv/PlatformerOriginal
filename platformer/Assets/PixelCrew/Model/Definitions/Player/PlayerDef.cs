using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Player
{
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int inventorySizeQuick;
        [SerializeField] private int inventorySizeBig;
        [SerializeField] private int _maxHealth;
        [SerializeField] private StatDef[] _stats;

        public int InventorySizeQuick => inventorySizeQuick;
        public int InventorySizeBig => inventorySizeBig;


        public StatDef[] Stats => _stats;

        public StatDef GetStat(StatId id)
        {
            foreach (var statDef in _stats)
            {
                if (statDef.ID == id)
                {
                    return statDef;
                }
            }
            return default;
        }

        
    }
}