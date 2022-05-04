using System;
using PixelCrew.Components.Audio;
using PixelCrew.Components.ColliderBased;
using UnityEngine;
using Object = System.Object;

namespace PixelCrew.Creatures.Mobs
{
    public class CloneMobsAI : MonoBehaviour
    {

        [SerializeField] public GameObject obj;
        [SerializeField] private ColliderCheck _vision;
        public float _NARUTA = 0;
        private PlaySoundsComponent _sounds;


        private void Awake()
        {
            _sounds = GetComponent<PlaySoundsComponent>();
        }

        public void Update()
        {
            if (_vision.IsTouchingLayer && _NARUTA >= 1)
            {
                _sounds.Play("Jutsu");    
                var naruto = Instantiate(obj, transform.position, Quaternion.identity);
                _NARUTA--;
                naruto.SetActive(true);
            }
            
        }
    }
}
