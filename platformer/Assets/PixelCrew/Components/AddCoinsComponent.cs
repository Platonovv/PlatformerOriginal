using UnityEngine;

namespace PixelCrew.Components
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
