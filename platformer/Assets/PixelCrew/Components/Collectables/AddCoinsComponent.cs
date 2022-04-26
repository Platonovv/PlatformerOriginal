using PixelCrew.Creatures;
using PixelCrew.Creatures.Hero;
using UnityEngine;

namespace PixelCrew.Components.Collectables
{
    public class AddCoinsComponent : MonoBehaviour
    {
        [SerializeField] private int _coins;

        public void AddCoins(GameObject target)
        {
            var hero = target.GetComponent<Hero>();
            if (hero == null) return;
            hero.AddCoin(_coins);
        }
        

    }
}
