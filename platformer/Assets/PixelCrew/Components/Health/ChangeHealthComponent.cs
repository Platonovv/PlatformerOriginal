using System;
using PixelCrew.Model;
using PixelCrew.Model.Definitions.Player;
using UnityEngine;

namespace PixelCrew.Components.Health
{
    public class ChangeHealthComponent: MonoBehaviour

    {
        [SerializeField] private int _healDelta;
        private GameSession _session;


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.StatsModel.OnUpgraded += OnHeroHealDeltaUpgrade;
            OnHeroHealDeltaUpgrade(StatId.RangeDamage);

            
        }
        

        public void ChangeHealth(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                var defaultChangeHeal = (int)_session.StatsModel.GetValue(StatId.RangeDamage);
                healthComponent.ChangeHealth(_healDelta/2 - defaultChangeHeal);
                Debug.Log($"{target.name}, health: {healthComponent.Health} ");
            }
            
        }
        
        private void OnHeroHealDeltaUpgrade(StatId statId)
        {
            switch (statId)
            {
               case StatId.RangeDamage:
                   var healDeltaUpgrade = (int) _session.StatsModel.GetValue(statId);
                   _healDelta = -healDeltaUpgrade;
                   
                   break;
            }
        }

        private void OnDestroy()
        {
            _session.StatsModel.OnUpgraded -= OnHeroHealDeltaUpgrade;
        }
    }
}