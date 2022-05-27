using System;
using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components
{
    public class RestoreStateComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        private GameSession _session;
        public string Id => _id;
        private void Start()
        {
            _session = GameSession.Instance;
            var isDestroyed = _session.RestoreState(Id);

            if (isDestroyed)
            {
                Destroy(gameObject);
            }
        }
    }
}