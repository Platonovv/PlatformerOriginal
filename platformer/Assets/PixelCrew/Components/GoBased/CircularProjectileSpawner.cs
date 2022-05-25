using System;
using System.Collections;
using PixelCrew.Creatures.Weapons;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class CircularProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private CircularProjectileSettings[] _settings;
        public int Stage { get; set; }


        [ContextMenu("Launch!")]
        public void LaunchProjectiles()
        {
            StartCoroutine(SpawnProjectiles());
        }

        private IEnumerator SpawnProjectiles()
        {
            var setting = _settings[Stage];
            var sectorStep = 2 * Mathf.PI / setting.BurstCount;
            
            
            for (var i = 0; i < setting.ItemPerBurst; i++)
            {
                
                for (var j = 0; j < setting.BurstCount; j++)
                {
                    var angle = sectorStep * j;
                    var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                    var instance = SpawnUtils.Spawn(setting.Prefab.gameObject, transform.position);
                    var projectile = instance.GetComponent<DirectionalProjectile>();
                    projectile.Launch(direction);
                    yield return new WaitForSeconds(setting.Delay);
                }
            }
            
        }
    }

    [Serializable]
    public struct CircularProjectileSettings
    {
        [SerializeField] private DirectionalProjectile _prefab;
        [SerializeField] private int _itemPerBurst;
        [SerializeField] private int _burstCount;
        [SerializeField] private float _delay;

        public int ItemPerBurst => _itemPerBurst;

        public DirectionalProjectile Prefab => _prefab;

        public int BurstCount => _burstCount;

        public float Delay => _delay;
    }
}