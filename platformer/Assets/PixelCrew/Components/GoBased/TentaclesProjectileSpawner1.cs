using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Creatures.Weapons;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class TentaclesProjectileSpawner1: MonoBehaviour
    {
        [SerializeField] private TentaclesProjectileSettings1[] _settings;
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
                    var position = new Vector3(17.52f,-5.64f,0f);
                    var instance = SpawnUtils.Spawn(setting.Prefab.gameObject, position);
                    var projectile = instance.GetComponent<DirectionalProjectile>();
                    projectile.Launch(position);
                    yield return new WaitForSeconds(setting.Delay);  
                }
            }
        }
    }

    [Serializable]
    public struct TentaclesProjectileSettings1
    {
        [SerializeField] private DirectionalProjectile _prefab;
        [SerializeField] private int _count;
        [SerializeField] private float _delay;
        

        public DirectionalProjectile Prefab => _prefab;

        public int Count => _count;

        public float Delay => _delay;
    }
}