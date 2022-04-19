using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private HealthChangeEvent _onChange;


        public int Health => _health;
        
        

        public void ChangeHealth(int deltaHealth)
        {
            _health += deltaHealth;
            _onChange?.Invoke(_health);
            
            if (deltaHealth <0)
            {
                _onDamage?.Invoke();
                
            } else if (deltaHealth>0)
            {
                _onHeal?.Invoke();
            } 
            if (_health <= 0)
            {
                _onDie?.Invoke();
                _health = 0;
            }
        }


#if UNITY_EDITOR
        [ContextMenu("Update Health")]
        private void UpdateHealth()
        {
            _onChange?.Invoke(_health);
        }
#endif

        public void SetHealth(int health)
        {
            _health = health;
        }
        
        
        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {
            
        }
    }
    
}

