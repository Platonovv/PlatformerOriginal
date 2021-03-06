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
        private Coroutine _coroutine;
        public int Stage { get; set; }


        [ContextMenu("Launch!")]
        public void LaunchProjectiles()
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(SpawnProjectiles());
        }

        private IEnumerator SpawnProjectiles()
        {
            var setting = _settings[Stage];
            var sectorStep = 2 * Mathf.PI / setting.Sides;
            
            
            for (var i = 0; i < setting.ItemPerBurst; i++)
            {
                
                for (var j = 0; j < setting.Sides; j++)
                {
                    var angle = sectorStep * j;
                    var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                    var instance = SpawnUtils.Spawn(setting.Prefab.gameObject, transform.position);
                    var projectile = instance.GetComponent<DirectionalProjectile>();
                    projectile.Launch(direction);
                    yield return new WaitForSeconds(setting.Delay);
                }
            }

            _coroutine = null;
        }
    }

    [Serializable]
    public struct CircularProjectileSettings
    {
        [SerializeField] private DirectionalProjectile _prefab;
        [SerializeField] private int _itemPerBurst;
        [SerializeField] private int _sides;
        [SerializeField] private float _delay;

        public int ItemPerBurst => _itemPerBurst;

        public DirectionalProjectile Prefab => _prefab;

        public int Sides => _sides;

        public float Delay => _delay;
    }
}