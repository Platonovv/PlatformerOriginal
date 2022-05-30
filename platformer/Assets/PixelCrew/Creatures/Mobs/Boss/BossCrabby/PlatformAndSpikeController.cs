using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components.GoBased;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.BossCrabby
{
    public class PlatformAndSpikeController: MonoBehaviour
    {
        [SerializeField] private List<GameObject> _platforms;
        [SerializeField] private SpikeSequence[] _sequences;
        private Coroutine _coroutine;

        [ContextMenu("Spiking")]
        public void StartBombing()
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(SpikingSequence());
        }

        private IEnumerator SpikingSequence()
        {
            _platforms.ForEach(x => x.SetActive(true));
            
            foreach (var spikeSequence in _sequences)
            {
                foreach (var spawnComponent in spikeSequence.SpikePoint)
                {
                    spawnComponent.Spawn();
                }

                yield return new WaitForSeconds(spikeSequence.Delay);
            }
            
            
            _coroutine = null;

        }


        [Serializable]
        public class  SpikeSequence
        {
            [SerializeField] private SpawnComponent[] _spikePoints;
            [SerializeField] private float _delay;

            public SpawnComponent[] SpikePoint => _spikePoints;

            public float Delay => _delay;
        }
    }
}