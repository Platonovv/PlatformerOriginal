using System;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.HUD.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItemWidget _prefab;


        private readonly CompositeDisposables _trash = new CompositeDisposables();


        private GameSession _session;

        private DataGroup<InventoryItemData, InventoryItemWidget> _dataGroup;

        private void Start()
        {
            _dataGroup = new DataGroup<InventoryItemData, InventoryItemWidget>(_prefab, _container);
            _session = GameSession.Instance;
            _trash.Retain(_session.QuickInventory.Subscribe(Rebuild));
            Rebuild();
        }

        private void Rebuild()
        {
            var inventory = _session.QuickInventory.QuickInventory;
            _dataGroup.SetData(inventory);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}