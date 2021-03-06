using System.Collections.Generic;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.UI.HUD.QuickInventory;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PixelCrew.UI.HUD.BigInventory
{
    public class BigInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItemWidgetBig _prefab;

        private Animator _animator;
        private static readonly int Hide = Animator.StringToHash("Hide"); 
        private readonly CompositeDisposables _trash = new CompositeDisposables();


        private GameSession _session;
        private DataGroup<InventoryItemData, InventoryItemWidgetBig> _dataGroup;

        

        private void Start()
        {
            _dataGroup = new DataGroup<InventoryItemData, InventoryItemWidgetBig>(_prefab, _container);
            _session = GameSession.Instance;
            _animator = GetComponent<Animator>();
            _trash.Retain(_session.BigInventory.Subscribe(Rebuild));
            Rebuild();
        }

        private void Rebuild()
        {
            var inventory = _session.BigInventory.Inventory;
            _dataGroup.SetData(inventory);
        }

        public void Close()
        {
            _animator.SetTrigger(Hide);
        }
        private void OnDestroy()
        {
            _trash.Dispose();
        }
        
    }
}