using System;
using PixelCrew.Components.GoBased;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.LevelManegement
{
    [RequireComponent(typeof(SpawnComponent))]
    public class CheckPointComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField]private SpawnComponent _heroSpawned;
        [SerializeField] private UnityEvent _setChecked;
        [SerializeField] private UnityEvent _setUnchecked;

        public string Id => _id;
        
        private GameSession _session;
        
        private void Start()
        {
            _session = GameSession.Instance;
            if (_session.IsChecked(_id))
                _setChecked?.Invoke();
            else
                _setUnchecked?.Invoke();
        }

        public void Check()
        {
            _session.SetChecked(_id);
            _setChecked?.Invoke();
        }

        public void SpawnHero()
        {
            _heroSpawned.Spawn();
        }
}
}