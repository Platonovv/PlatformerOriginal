﻿using System;
using System.Collections.Generic;
using PixelCrew.Utils;
using UnityEditor;
using UnityEngine;

namespace PixelCrew
{
    public class CheckCircleOverlap : MonoBehaviour

    {
        [SerializeField] private float _radius = 1f;
        
        private readonly Collider2D[] _interactionResult = new Collider2D[5];

        
        public GameObject[] GetObjectsInRange()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _radius,
                _interactionResult);
            
            var overlaps = new List<GameObject>();
            for (var i = 0; i < size; i++)
            {
                overlaps.Add(_interactionResult[i].gameObject);
            }

            return overlaps.ToArray();
        }

        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }
    }
}