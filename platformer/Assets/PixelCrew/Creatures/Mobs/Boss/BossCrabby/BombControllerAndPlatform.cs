﻿using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components.GoBased;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.BossCrabby
{
    public class BombControllerAndPlatform : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _platforms;
        [SerializeField] private BombSequence[] _sequences;
        private Coroutine _coroutine;

        [ContextMenu("Start bombing")]
        public void StartBombing()
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(BombingSequence());
        }

        private IEnumerator BombingSequence()
        {
            _platforms.ForEach(x => x.SetActive(true));
            
            foreach (var bombSequence in _sequences)
            {
                foreach (var spawnComponent in bombSequence.BombPoints)
                {
                    spawnComponent.Spawn();
                }

                yield return new WaitForSeconds(bombSequence.Delay);
            }

            _platforms.ForEach(x => x.SetActive(false));
            
            _coroutine = null;

        }


        [Serializable]
        public class  BombSequence
        {
            [SerializeField] private SpawnComponent[] _bombPoints;
            [SerializeField] private float _delay;

            public SpawnComponent[] BombPoints => _bombPoints;

            public float Delay => _delay;
        }
    }
}