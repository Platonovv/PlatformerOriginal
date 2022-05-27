using System;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repository.Items;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.HUD.QuickInventory
{
    public class InventoryItemWidget : MonoBehaviour, IItemRenderer<InventoryItemData>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selection;
        [SerializeField] private Text _value;
        
        private readonly CompositeDisposables _trash = new CompositeDisposables();


        private int _index;

        private void Start()
        {
            var session = GameSession.Instance;
            var index = session.QuickInventory.SelectedIndex;
            _trash.Retain(index.SubscribeAndInvoke(OnIndexChanged));
        }

        private void OnIndexChanged(int newValue, int _)
        {
            _selection.SetActive(_index == newValue);
        }


        public void SetData(InventoryItemData item, int index)
        {
            _index = index;
            if (item != null)
            {
                _icon.gameObject.SetActive(true);
                _value.gameObject.SetActive(true);
                var def = DefsFacade.I.Items.Get(item.Id);
                _icon.sprite = def.Icon;
                _value.text = def.HasTag(ItemTag.Stackable) ? item.Value.ToString() : string.Empty;
            }
            else
            {
                _icon.gameObject.SetActive(false);
                _value.gameObject.SetActive(false); 
            }

        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}