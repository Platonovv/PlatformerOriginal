using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions.Repository.Items;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Collectables
{
    public class AddInInventoryBig : MonoBehaviour
    {
        [InventoryId][SerializeField] private string _id;
        [SerializeField] private int _count;
        
        
        public void AddToBig(GameObject go)
        {
            var hero = go.GetInterface<ICanAddInInventory>();
            hero?.AddInInventoryBig(_id, _count);
        }
    }
}