using System;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace PixelCrew.Components.Movement
{
    public class CircularMovementComponent : MonoBehaviour
    {
        [SerializeField] Transform center;
        [SerializeField] private float radius = 0f;
        [SerializeField] private float angularSpeed = 0f;

        private float _positionX;
        private float _positionY;
        private float _angle = 0f;



        private void Update()
        {
            var position = center.position;
            _positionX = position.x + Mathf.Cos(_angle) * radius;
            _positionY = position.y + Mathf.Sin(_angle) * radius;
            transform.position = new Vector2(_positionX, _positionY);
            _angle += Time.deltaTime * angularSpeed;
            if (_angle >360f)
            {
                _angle = 0f;
            }
            

        }
    }
}