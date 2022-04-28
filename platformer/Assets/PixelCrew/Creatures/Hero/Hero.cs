using System;
using System.Collections;
using PixelCrew.Components;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Components.Health;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEditor.Animations;
using UnityEngine;
using Random = System.Random;

namespace PixelCrew.Creatures.Hero
{


   public class Hero : Creature
   {
      [SerializeField] private CheckCircleOverlap _interactionCheck;
      [SerializeField] private LayerMask _interactionLayer;
      [SerializeField] private ColliderCheck _wallCheck;
      
      [SerializeField] private float _slamDownVelocity;
      [SerializeField] private float _interactionRadius;
      
   

      [SerializeField] private float _groundCheckRadius;
      [SerializeField] private Vector3 _groundCheckPositiondDelta;

      [SerializeField] private Cooldown _throwCooldown;
      [SerializeField] private AnimatorController _armed;
      [SerializeField] private AnimatorController _disarmed;


      [Header("Super throw")] [SerializeField]
      private Cooldown _superThrowCooldown;
      
      [SerializeField] private int _superThrowParticles;
      [SerializeField] private float _superThrowDelay;
      [SerializeField] private ProbabilityDropComponent _hitDrop;


      private static readonly int ThrowKey = Animator.StringToHash("throw");
      private static readonly int PotionKey = Animator.StringToHash("potion");
      private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");

     
      private bool _allowDoubleJump;
      private bool _isOnWall;
      private bool _superThrow;
      
      private GameSession _session;
      private float _defaultGravityScale;
      

      private int CoinsCount => _session.Data.Inventory.Count("Coin");
      private int SwordCount => _session.Data.Inventory.Count("Sword");
      
      private int BottleCount => _session.Data.Inventory.Count("Bottle");


      protected override void Awake()
      {
        base.Awake();
        
         _defaultGravityScale = Rigidbody.gravityScale;
      }
      
      private void Start()
      {
         _session = FindObjectOfType<GameSession>();
         var health = GetComponent<HealthComponent>();
         _session.Data.Inventory.OnChanged += OnInventoryChanged;
         _session.Data.Inventory.OnChanged += AnotherHandler;

         
         health.SetHealth(_session.Data.Hp);
         UpdateHeroWeapon();
      }


      private void OnDestroy()
      {
         _session.Data.Inventory.OnChanged -= OnInventoryChanged;
         _session.Data.Inventory.OnChanged -= AnotherHandler;
      }

      private void AnotherHandler(string id, int value)
      {
         Debug.Log($"Inventory changed {id} : {value}");
      }

      private void OnInventoryChanged(string id, int value)
      {
         if(id == "Sword")
            UpdateHeroWeapon();
      }

      public void OnHealthChanged(int currentHealth)
      {
         _session.Data.Hp = currentHealth;
      }
      

      protected override void Update()
      {
         base.Update();

         var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
         if (_wallCheck.IsTouchingLayer && moveToSameDirection)
         {
            _isOnWall = true;
            Rigidbody.gravityScale = 0;
         }
         else
         {
            _isOnWall = false;
            Rigidbody.gravityScale = _defaultGravityScale;
         }
         
         Animator.SetBool(IsOnWall,_isOnWall);
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
          if (! IsGrounded && _allowDoubleJump && !_isOnWall)
         {
            _allowDoubleJump = false;
            DoJumpVfx();
            return _jumpSpeed;
         }

         return base.CalculateJumpVelocity(yVelocity);

      }


      public void AddInInventory(string id , int value)
      {
         _session.Data.Inventory.Add(id,value);
      }
      
      public override void TakeDamage()
      {
         base.TakeDamage();
         if (CoinsCount > 0)
         {
            SpawnCoins();
         }

      }

      private void SpawnCoins()
      {
         var numCoinsToDispose = Math.Min(CoinsCount, 5);
         _session.Data.Inventory.Remove("Coins", numCoinsToDispose);

         _hitDrop.SetCount(numCoinsToDispose);
         _hitDrop.CalculateDrop();
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
         if (SwordCount <= 0) return;
         
         base.Attack();
         
      }

      private void UpdateHeroWeapon()
      {
         
         Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _disarmed;
      }


      public void OnDoThrow()
      {
         if (_superThrow)
         {
            var numThrows = Mathf.Min(_superThrowParticles, SwordCount - 1);
            StartCoroutine(DoSuperThrow(numThrows));
         }
         else
         {
            ThrowAndRemoveFromInventory();
         }

         _superThrow = false;
      }

      private IEnumerator DoSuperThrow(int numThrows)
      {
         for (int i = 0; i < numThrows; i++)
         {
            ThrowAndRemoveFromInventory();
            yield return new WaitForSeconds(_superThrowDelay);
         }
      }

      private void ThrowAndRemoveFromInventory()
      {
         Sounds.Play("Range");
         _particles.Spawn("Throw");
         _session.Data.Inventory.Remove("Sword", 1);
      }
      
      
      
      public void UsePotion()
      {
         
         if (BottleCount >= 1)
         {
            var currentHealth = 5;
            _session.Data.Hp += currentHealth;
            Animator.SetTrigger(PotionKey);
            _session.Data.Inventory.Remove("Bottle", 1);
         }

      }

      public void StartThrowing()
      {
         _superThrowCooldown.Reset();
      }

      public void PerformThrowing()
      {
         if (!_throwCooldown.IsReady || SwordCount <= 1) return;

         if (_superThrowCooldown.IsReady) _superThrow = true;

         Animator.SetTrigger(ThrowKey);
         _throwCooldown.Reset();

      }
   }
}
