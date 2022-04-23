using PixelCrew.Creatures;
using UnityEngine;

namespace PixelCrew.Components
{
    public class AddSwordComponent : MonoBehaviour
    {
        [SerializeField] private int _sword;

        public void AddSword(GameObject target)
        {
            var hero = target.GetComponent<Hero>();
            if (hero == null) return;
            hero.AddSword(_sword);
        }
    }
}
