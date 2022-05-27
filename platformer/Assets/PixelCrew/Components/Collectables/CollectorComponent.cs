﻿using System.Collections.Generic;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using UnityEngine;

namespace PixelCrew.Components.Collectables
{
    public class CollectorComponent : MonoBehaviour, ICanAddInInventory

    {
        [SerializeField] private List<InventoryItemData> _items = new List<InventoryItemData>();
        public void AddInInventory(string id, int value)
        {
            _items.Add(new InventoryItemData(id) {Value = value});
        }

        public void AddInInventoryBig(string id, int value)
        {
            _items.Add(new InventoryItemData(id) {Value = value});
        }


        public void DropInInventory()
        {
            var session = GameSession.Instance;
            foreach (var inventoryItemData in _items)
            {
                session.Data.Inventory.Add(inventoryItemData.Id, inventoryItemData.Value);
            }
            
            _items.Clear();
        }
        
    }
}