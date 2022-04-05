using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components;
using PixelCrew.Utils;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;

namespace PixelCrew
{


   public class Hero : MonoBehaviour
   {
      [SerializeField] private float _speed;
      [SerializeField] private float _jumpSpeed;
      [SerializeField] private float _damagejumpSpeed;
      [SerializeField] private float _slamDownVelocity;
      [SerializeField] private LayerMask _groundLayer;
      [SerializeField] private float _interactionRadius;
      [SerializeField] private LayerMask _interactionLayer;
      [SerializeField] private LayerCheck _groundCheck;

      [SerializeField] private float _groundCheckRadius;
      [SerializeField] private Vector3 _groundCheckPositiondDelta;

      [SerializeField] private SpawnComponent _footStepParticles;
      [SerializeField] private SpawnComponent _footJumpParticles;
      [SerializeField] private SpawnComponent _footFallParticles;
      [SerializeField] private ParticleSystem _hitParticles;



      private Collider2D[] _interactionResult = new Collider2D[1];
      private Rigidbody2D _rigidbody;
      private Vector2 _direction;
      private Animator _animator;
      private bool _isGrounded;
      private bool _allowDoubleJump;
      private bool _isJumping;

      private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
      private static readonly int IsRunning = Animator.StringToHash("is-running");
      private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
      private static readonly int Hit = Animator.StringToHash("hit");

      private int _coins;

      private void Awake()
      {
         _rigidbody = GetComponent<Rigidbody2D>();
         _animator = GetComponent<Animator>();
      }

      public void SetDirection(Vector2 direction)
      {
         _direction = direction;

      }

      private void Update()
      {
         _isGrounded = IsGrounded();
      }

      private void FixedUpdate()
      {
         var xVelocity = _direction.x * _speed;
         var yVelocity = CalculateYVelocity();
         _rigidbody.velocity = new Vector2(xVelocity, yVelocity);


         _animator.SetBool(IsGroundKey, _isGrounded);
         _animator.SetBool(IsRunning, _direction.x != 0);
         _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);


         UpdateSpriteDirection();
      }


      private float CalculateYVelocity()
      {
         var yVelocity = _rigidbody.velocity.y;
         var isJumpPressing = _direction.y > 0;

         if (_isGrounded)
         {
            _allowDoubleJump = true;
            _isJumping = false;
         }

         if (isJumpPressing)

         {
            _isJumping = true;

            yVelocity = CalculateJumpVelocity(yVelocity);
         }
         else if (_rigidbody.velocity.y > 0 && _isJumping)
         {
            yVelocity *= 0.5f;
         }

         return yVelocity;
      }

      private float CalculateJumpVelocity(float yVelocity)
      {
         var isFalling = _rigidbody.velocity.y <= 0.001f;
         if (!isFalling) return yVelocity;

         if (_isGrounded)
         {
            yVelocity += _jumpSpeed;
            _footJumpParticles.Spawn();
         }
         else if (_allowDoubleJump)
         {
            yVelocity += _jumpSpeed;
            _footJumpParticles.Spawn();
            _allowDoubleJump = false;

         }

         return yVelocity;

      }

      private void UpdateSpriteDirection()
      {
         if (_direction.x > 0)
         {
            transform.localScale = Vector3.one;
         }
         else if (_direction.x < 0)
         {
            transform.localScale = new Vector3(-1, 1, 1);
         }
      }


      private bool IsGrounded()
      {
         var hit = Physics2D.CircleCast(transform.position + _groundCheckPositiondDelta, _groundCheckRadius,
            Vector2.down, 0, _groundLayer);

         return _groundCheck.IsTouchingLayer;
      }

      private void OnDrawGizmos()
      {
         Gizmos.color = IsGrounded() ? Color.green : Color.red;
         Gizmos.DrawSphere(transform.position, 0.3f);
      }

      public void SaySomething()
      {
         Debug.Log("Something!");
      }


      public void AddCoin(int coins)
      {
         _coins += coins;
         Debug.Log($"{gameObject.name}, total coins : {_coins} ");

      }




      public void TakeDamage()
      {
         _isJumping = false;
         _animator.SetTrigger(Hit);
         _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damagejumpSpeed);

         if (_coins > 0)
         {
            SpawnCoins();
         }

      }

      private void SpawnCoins()
      {
         var numCoinsToDisoise = Math.Min(_coins, 5);
         _coins -= numCoinsToDisoise;
         Debug.Log($"{gameObject.name}, coins : {_coins} ");

         var burst = _hitParticles.emission.GetBurst(0);
         burst.count = numCoinsToDisoise;
         _hitParticles.emission.SetBurst(0, burst);

         _hitParticles.gameObject.SetActive(true);
         _hitParticles.Play();
      }

      public void TakeHeal()
      {
         _animator.SetTrigger(Hit);

      }

      public void Interact()
      {
         var size = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            _interactionRadius,
            _interactionResult,
            _interactionLayer);

         for (int i = 0; i < size; i++)
         {
            var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
            if (interactable != null)
            {
               interactable.Interact();
            }
         }
      }

      private void OnCollisionEnter2D(Collision2D other)
      {
         if (other.gameObject.IsInLayer(_groundLayer))
         {
            var contact = other.contacts[0];
            if (contact.relativeVelocity.y >= _slamDownVelocity)
            {
               _footFallParticles.Spawn();
            }
         }
      }


      public void SpawnFootDust()
      {
         _footStepParticles.Spawn();
      }



   }
}
