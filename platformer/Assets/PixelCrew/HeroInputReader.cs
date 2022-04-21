
using System.Numerics;
using PixelCrew.Creatures;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace PixelCrew
{


    public class HeroInputReader : MonoBehaviour
 {
    [SerializeField] private Hero _hero;
    public void OnMovement(InputAction.CallbackContext context)
    {
        var  direction = context.ReadValue<Vector2>();
        _hero.SetDirection(direction);
    }
    

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _hero.Interact();
        }

    }
    public void OnDoAttack(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _hero.Attack();
        }

    }
    
 }
}