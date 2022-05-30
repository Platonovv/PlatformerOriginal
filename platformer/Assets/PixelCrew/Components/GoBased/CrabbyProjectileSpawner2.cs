using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Creatures.Weapons;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class CrabbyProjectileSpawner2 : MonoBehaviour
    {
        [SerializeField] private CrabbyProjectileSpawners2[] _settings;
        public int Stage { get; set; }


        [ContextMenu("Launch!")]
        public void LaunchProjectiles()
        {
            StartCoroutine(SpawnProjectiles());
        }
        
        private IEnumerator SpawnProjectiles()
        {
            var setting = _settings[Stage];
            {
                for (int i = 0; i < setting.Count; i++)
                {
                    var position = new Vector3(0.88f,-1.60601f,0f);
                    var instance = SpawnUtils.Spawn(setting.Prefab.gameObject, position);
                    instance.GetComponent<GameObject>();
                    yield return new WaitForSeconds(setting.Delay);  
                }
            }
        }
    }

    [Serializable]
    public struct CrabbyProjectileSpawners2
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _count;
        [SerializeField] private float _delay;
        

        public GameObject Prefab => _prefab;

        public int Count => _count;

        public float Delay => _delay;
    }
}