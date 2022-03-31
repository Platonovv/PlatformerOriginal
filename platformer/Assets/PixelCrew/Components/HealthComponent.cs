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


        public int Health => _health;
        
        

        public void ChangeHealth(int deltaHealth)
        {
            _health += deltaHealth;
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
        
    }
}

