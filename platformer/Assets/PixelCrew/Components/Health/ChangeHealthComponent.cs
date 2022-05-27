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


        public void SetDelta(int delta)
        {
            _healDelta = delta;
        }
        private void Start()
        {
            _session = GameSession.Instance;
            _session.StatsModel.OnUpgraded += OnHeroHealDeltaUpgrade;
            OnHeroHealDeltaUpgrade(StatId.RangeDamage);
        }
        

        public void ChangeHealth(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.ChangeHealth(_healDelta);
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