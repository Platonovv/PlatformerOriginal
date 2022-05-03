using System;
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

        public void Update()
        {
            if (_vision.IsTouchingLayer && _NARUTA <= 1)
            {
                    var naruto = Instantiate(obj, transform.position, Quaternion.identity);
                    _NARUTA++;
                    naruto.SetActive(true);
            }
            
        }
    }
}
