using PixelCrew.Model;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.Perks
{
    public class ManagePerksWindow : AnimatedWindow
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _useButton;
        [SerializeField] private ItemWidget _price;
        [SerializeField] private Text _infoText;
        [SerializeField] private Transform _perksContainer;

        private PredefinedDataGroup<string, PerkWidget> _dataGroup;
        private readonly CompositeDisposables _trash = new CompositeDisposables();
        private GameSession _session;

        protected override void Start()
        {
            base.Start();

            _dataGroup = new PredefinedDataGroup<string, PerkWidget>(_perksContainer);
        }

    }
}