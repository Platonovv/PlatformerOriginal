using System;
using PixelCrew.Model;
using PixelCrew.Model.Definitions.Repository;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace PixelCrew.UI.HUD
{
    public class CurrentPerkWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _cooldownImage;


        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }

        public void Set(PerkDef perk)
        {
            _icon.sprite = perk.Icon;
        }

        private void Update()
        {
            var cooldown = _session.PerksModel.Cooldown;
            _cooldownImage.fillAmount = cooldown.RemainingTime / cooldown.Value;
        }
    }
}