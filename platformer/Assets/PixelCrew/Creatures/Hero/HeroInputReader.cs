using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace PixelCrew.Creatures.Hero
{


    public class HeroInputReader : MonoBehaviour
 {
    [SerializeField] private Hero _hero;
    public void OnMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        _hero.SetDirection(direction);
    }

    public void OnMousePos(InputAction.CallbackContext context)
    {
        var pos = context.ReadValue<Vector2>();

    }
    public void OnMouseButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("sda");
        }

        if (context.canceled)
        {
            Debug.Log("sss");
        }
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
        if (context.performed)
        {
            _hero.Attack();
        }

    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _hero.StartThrowing();
        }

        if (context.canceled)
        {
            _hero.UseInventory();
        }
        
    }
    
    /*public void OnUsePotion(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _hero.UsePotionR();
        }
    }*/
    
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _hero.Dash();
        }
        
        
    }
    
    
    public void OnNextItem(InputAction.CallbackContext context)
    {
        if (context.performed)
            _hero.NextItem();
        
    }
    
    public void OnNextItems(InputAction.CallbackContext context)
    {
        if (context.performed)
            _hero.NextItems();
        
    }
    
    public void OnButtonAddInQuickInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
            _hero.AddQuickInventory();
    }
 }
}