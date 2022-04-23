using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;

namespace PixelCrew.Creatures
{


   public class Hero : Creature
   {
      [SerializeField] private CheckCircleOverlap _interactionCheck;
      [SerializeField] private LayerMask _interactionLayer;
      [SerializeField] private LayerCheck _wallCheck;
      
      [SerializeField] private float _slamDownVelocity;
      [SerializeField] private float _interactionRadius;
      
   

      [SerializeField] private float _groundCheckRadius;
      [SerializeField] private Vector3 _groundCheckPositiondDelta;

      [SerializeField] private Cooldown _throwCooldown;
      [SerializeField] private AnimatorController _armed;
      [SerializeField] private AnimatorController _disarmed;


      [Space] [Header("Particles")]
      [SerializeField] private ParticleSystem _hitParticles;
      
      protected static readonly int ThrowKey = Animator.StringToHash("throw");

     
      private bool _allowDoubleJump;
      private bool _isOnWall;

      
      private GameSession _session;
      private float _defaultGravityScale;
    


      protected override void Awake()
      {
        base.Awake();
        
         _defaultGravityScale = Rigidbody.gravityScale;
      }
      
      private void Start()
      {
         _session = FindObjectOfType<GameSession>();
         var health = GetComponent<HealthComponent>();
         
         health.SetHealth(_session.Data.Hp);
         UpdateHeroWeapon();
      }
      
      public void OnHealthChanged(int currentHealth)
      {
         _session.Data.Hp = currentHealth;
      }
      

      protected override void Update()
      {
         base.Update();

         if (_wallCheck.IsTouchingLayer && Direction.x == transform.localScale.x)
         {
            _isOnWall = true;
            Rigidbody.gravityScale = 0;
         }
         else
         {
            _isOnWall = false;
            Rigidbody.gravityScale = _defaultGravityScale;
         }
      }

      private void FixedUpdate()
      {
         var xVelocity = Direction.x * _speed;
         var yVelocity = CalculateYVelocity();
         Rigidbody.velocity = new Vector2(xVelocity, yVelocity);


         Animator.SetBool(IsGroundKey, IsGrounded);
         Animator.SetBool(IsRunning, Direction.x != 0);
         Animator.SetFloat(VerticalVelocity, Rigidbody.velocity.y);


         UpdateSpriteDirection(Direction);
      }


      protected override float CalculateYVelocity()
      {
         var isJumpPressing = Direction.y > 0;

         if (IsGrounded || _isOnWall)
         {
            _allowDoubleJump = true;
         }

         if (!isJumpPressing && _isOnWall)
         {
            return 0f;
         }
         return base.CalculateYVelocity();
      }

      protected override float CalculateJumpVelocity(float yVelocity)
      {
          if (! IsGrounded && _allowDoubleJump)
         {
            _particles.Spawn("Jump");
            _allowDoubleJump = false;
            return _jumpSpeed;
         }

         return base.CalculateJumpVelocity(yVelocity);

      }

      public void AddCoin(int coins)
      {
         _session.Data.Coins += coins;
         Debug.Log($"{gameObject.name}, total coins : {_session.Data.Coins} ");

      }




      public override void TakeDamage()
      {
         base.TakeDamage(); 
         if (_session.Data.Coins > 0)
         {
            SpawnCoins();
         }

      }

      private void SpawnCoins()
      {
         var numCoinsToDisoise = Math.Min(_session.Data.Coins, 5);
         _session.Data.Coins -= numCoinsToDisoise;

         var burst = _hitParticles.emission.GetBurst(0);
         burst.count = numCoinsToDisoise;
         _hitParticles.emission.SetBurst(0, burst);

         _hitParticles.gameObject.SetActive(true);
         _hitParticles.Play();
      }

      public void TakeHeal()
      {
         Animator.SetTrigger(Hit);

      }

      public void Interact()
      {
         _interactionCheck.Check();
      }

      private void OnCollisionEnter2D(Collision2D other)
      {
         if (other.gameObject.IsInLayer(_groundLayer))
         {
            var contact = other.contacts[0];
            if (contact.relativeVelocity.y >= _slamDownVelocity)
            {
               _particles.Spawn("footFall");
            }

            if (contact.relativeVelocity.y >= _damageVelocity)
            {
               GetComponent<HealthComponent>().ChangeHealth(-1);
            }
         }
      }
      

      public override void Attack()
      {
         if (!_session.Data.isArmed) return;
         
         base.Attack();
         
      }
      

      public void ArmHero()
      {
         _session.Data.isArmed = true;
         UpdateHeroWeapon();
         Animator.runtimeAnimatorController = _armed;
      }

      private void UpdateHeroWeapon()
      {
         Animator.runtimeAnimatorController = _session.Data.isArmed ? _armed : _disarmed;
      }


      public void OnDoThrow()
      {
         _particles.Spawn("Throw");
      }
      
      
      public void Throw()
      {
         if (_throwCooldown.IsReady && _session.Data.Sword >1) 
         {
            Animator.SetTrigger(ThrowKey);
            _session.Data.Sword -= 1;
            _throwCooldown.Reset();
            
         }
      }
      

      public void AddSword(int sword)
      {
         
            _session.Data.Sword += sword;
            Debug.Log($"{gameObject.name}, total sword : {_session.Data.Sword} ");

      }
   }
}
