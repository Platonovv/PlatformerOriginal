using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.HUD
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;
        [SerializeField] private CurrentPerkWidget _currentPerk;

        private GameSession _session;

        private readonly CompositeDisposables _trash = new CompositeDisposables();

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
             _session.Data.Hp.OnChanged += OnHealthChanged;
            
            _trash.Retain(_session.PerksModel.Subscribe(OnPerkChanged));
            
            OnHealthChanged(_session.Data.Hp.Value, 0);
            
            OnPerkChanged();
        }

        private void OnPerkChanged()
        {
            var usedPerkId = _session.PerksModel.Used;
            var hasPerk = !string.IsNullOrEmpty(usedPerkId);
            if(hasPerk)
            {
                var perkDef = DefsFacade.I.Perks.Get(usedPerkId);
                _currentPerk.Set(perkDef);
            }

            _currentPerk.gameObject.SetActive(hasPerk);
        }
        private void OnHealthChanged(int newValue, int oldValue)
        {
            var maxHealth = _session.StatsModel.GetValue(StatId.Hp);
            var value = (float) newValue / maxHealth;
            _healthBar.SetProgress(value);
        }

        public void OnSettings()
        {
            WindowUtils.CreateWindow("UI/InGameMenuWindow");
        }
        
        private void OnDestroy()
        {
            _trash.Dispose();
           _session.Data.Hp.OnChanged -= OnHealthChanged;
        }

        public void OnDebug()
        {
            WindowUtils.CreateWindow("UI/PlayerStatsWindow");
        }
    }
}