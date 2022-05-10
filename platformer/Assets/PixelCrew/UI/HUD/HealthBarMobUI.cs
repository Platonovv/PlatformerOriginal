using System;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.HUD
{
    public class HealthBarMobUI : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Vector3 _vector3;


        private void Update()
        {
            _slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + _vector3);
        }

        public void SetHealthValue(int currentHealth, int maxHealth)
        {
            _slider.gameObject.SetActive(currentHealth < maxHealth);
            _slider.value = currentHealth;
            _slider.maxValue = maxHealth;
        }
        
    }
}