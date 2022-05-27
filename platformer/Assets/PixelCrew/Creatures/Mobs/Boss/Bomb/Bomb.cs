using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Creatures.Mobs.Boss.Bomb
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private float _ttl;
        [SerializeField] private UnityEvent _onDetonate;
        
        private Coroutine _coroutine;

        private void OnEnable()
        {
            TryStop();
            _coroutine = StartCoroutine(WaitAndDetonate());
        }

        private IEnumerator WaitAndDetonate()
        {
            yield return new WaitForSeconds(_ttl);
            Detonate();
            _coroutine = null;
        }

        public void Detonate()
        {
            _onDetonate?.Invoke();
        }

        private void OnDisable()
        {
            TryStop();
        }

        private void TryStop()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}