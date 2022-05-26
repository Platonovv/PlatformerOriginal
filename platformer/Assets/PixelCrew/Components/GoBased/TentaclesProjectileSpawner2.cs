using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Creatures.Weapons;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class TentaclesProjectileSpawner2: MonoBehaviour
    {
        [SerializeField] private TentaclesProjectileSettings2[] _settings;
        public int Stage { get; set; }


        [ContextMenu("Launch!")]
        public void LaunchTentacles()
        {
            StartCoroutine(SpawnProjectiles());
        }

        private IEnumerator SpawnProjectiles()
        {
            var setting = _settings[Stage];
            {
                for (int i = 0; i < setting.Count; i++)
                {
                    var position = new Vector3(14.45f,0.81f,0f);
                    var instance = SpawnUtils.Spawn(setting.Prefab.gameObject, position);
                    var projectile = instance.GetComponent<DirectionalProjectile>();
                    projectile.Launch(position);
                    yield return new WaitForSeconds(setting.Delay);  
                }
            }
        }
    }

    [Serializable]
    public struct TentaclesProjectileSettings2
    {
        [SerializeField] private DirectionalProjectile _prefab;
        [SerializeField] private int _count;
        [SerializeField] private float _delay;
        

        public DirectionalProjectile Prefab => _prefab;

        public int Count => _count;

        public float Delay => _delay;
    }
}