using System;
using UnityEngine;

namespace PixelCrew.Effects
{
    public class ParallaxEffect : MonoBehaviour
    {
        [SerializeField] private float _effectValue;
        [SerializeField] private Transform _followTarget;

        private float _startX;
        private void Start()
        {
            _startX = transform.position.x;
        }


        private void LateUpdate()
        {
            var currenPosition = transform.position;
            var deltaX = _followTarget.position.x * _effectValue;
            transform.position = new Vector3(_startX + deltaX, currenPosition.y, currenPosition.z);
        }
    }
}