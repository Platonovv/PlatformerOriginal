using UnityEngine;

namespace PixelCrew.Components.Health
{
    public class ChangeHealthComponent: MonoBehaviour

    {
        [SerializeField] private int _healDelta;

        public void ChangeHealth(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.ChangeHealth(_healDelta);
                Debug.Log($"{target.name}, health: {healthComponent.Health} ");
            }
            
        }
    }
}