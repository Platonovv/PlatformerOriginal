using System;
using System.Collections.Generic;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repository.Items;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Model.Data.Models
{
    public class QuickInventoryModel : IDisposable
    {
        private readonly PlayerData _data;

        public InventoryItemData[] QuickInventory { get; private set; }

        public readonly IntProperty SelectedIndex = new IntProperty();
        

        public event Action OnChanged;

        public InventoryItemData SelectedItem
        {
            get
            {
                if (QuickInventory.Length > 0 && QuickInventory.Length > SelectedIndex.Value)
                    return QuickInventory[SelectedIndex.Value];
                
                return null;
            }
        }

        public ItemDef SelectedDef => DefsFacade.I.Items.Get(SelectedItem?.Id);
        
        public QuickInventoryModel(PlayerData data)
        {
            _data = data;

            QuickInventory = new InventoryItemData[3];
            _data.Inventory.OnChanged += OnChangedInventory;
        }
        private void OnChangedInventory(string id, int value)
        {
            var allInventory = _data.Inventory.GetAll(ItemTag.Usable);
            for (var i = 0; i < QuickInventory.Length; i++)
            {
                if (QuickInventory[i] == null) continue;
                var isItemExist = false;
                foreach (var itemData in allInventory)
                {
                    if (itemData.Id == QuickInventory[i].Id)
                    {
                        isItemExist = true;
                    }
                }

                if (isItemExist == false)
                {
                    QuickInventory[i] = null;
                }
            }

            OnChanged?.Invoke();
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposables(() => OnChanged -= call);
        }

        

        public void AddQuickInventoryItem(InventoryItemData item)
        {
            if(item == null) return;
            if (!DefsFacade.I.Items.Get(item.Id).HasTag(ItemTag.Usable))  return;
            QuickInventory[SelectedIndex.Value] = item;
            OnChanged?.Invoke();
        }
        
        public void SetNextItem()
        {
            SelectedIndex.Value = (int) Mathf.Repeat(SelectedIndex.Value + 1, QuickInventory.Length);
        }

        public void Dispose()
        {
            _data.Inventory.OnChanged -= OnChangedInventory;
        }
    }
}