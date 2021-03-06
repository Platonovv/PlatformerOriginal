using System;
using System.Collections;
using PixelCrew.Components;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Components.Health;
using PixelCrew.Creatures.Hero.Features;
using PixelCrew.Effects.CameraRelated;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Model.Definitions.Repository;
using PixelCrew.Model.Definitions.Repository.Items;
using PixelCrew.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PixelCrew.Creatures.Hero
{


   public class Hero : Creature, ICanAddInInventory
   {
      [SerializeField] private CheckCircleOverlap _interactionCheck;
      [SerializeField] private LayerMask _interactionLayer;
      [SerializeField] private ColliderCheck _wallCheck;
      
      [SerializeField] private float _slamDownVelocity;
      [SerializeField] private float _interactionRadius;


      [SerializeField] private float _groundCheckRadius;
      [SerializeField] private Vector3 _groundCheckPositiondDelta;

      [SerializeField] private Cooldown _throwCooldown;
      [SerializeField] private Cooldown _DashCooldown;
      [SerializeField] private RuntimeAnimatorController _armed;
      [SerializeField] private RuntimeAnimatorController _disarmed;


      [Header("Super throw")] [SerializeField]
      private Cooldown _superThrowCooldown;
      
      [SerializeField] private int _superThrowParticles;
      [SerializeField] private float _superThrowDelay;
      [SerializeField] private ProbabilityDropComponent _hitDrop;
      [SerializeField] private SpawnComponent _throwSpawner;
      [SerializeField] private HeroShield _shield;
      [SerializeField] private HeroFlashLight _flashLight;



      private static readonly int ThrowKey = Animator.StringToHash("throw");
      private static readonly int PotionKey = Animator.StringToHash("potion");
      private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");

     
      private bool _allowDoubleJump;
      private bool _isOnWall;
      private bool _superThrow;

      private Cooldown _speedUpCooldown = new Cooldown();
      private float _additionalSpeed;

      private CameraShakeEffect _cameraShake;
      private GameSession _session;
      private float _defaultGravityScale;
      private HealthComponent _health;

      private const string SwordId = "Sword";
      private const string BottleId = "BottleGreenHeal5";
      private const string Bottle10Id = "BottleHeal10";

      private int CoinsCount => _session.Data.Inventory.Count("Coin");
      private int SwordCount => _session.Data.Inventory.Count(SwordId);

      private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;
      
      private bool CanThrow
      {
         get
         {
            if (SelectedItemId == SwordId)
               return SwordCount > 1;
            
            var def = DefsFacade.I.Items.Get(SelectedItemId);
            return def.HasTag(ItemTag.Throwable);
         }
      }


      protected override void Awake()
      {
        base.Awake();
        
         _defaultGravityScale = Rigidbody.gravityScale;
      }
      
      private void Start()
      {
         _cameraShake = FindObjectOfType<CameraShakeEffect>();
         _session = GameSession.Instance;
         _health = GetComponent<HealthComponent>();
         _session.Data.Inventory.OnChanged += OnInventoryChanged;
         _session.Data.Inventory.OnChanged += AnotherHandler;
         _session.StatsModel.OnUpgraded += OnHeroUpgraded;
         
         _health.SetHealth(_session.Data.Hp.Value);
         UpdateHeroWeapon();
      }

      private void OnHeroUpgraded(StatId statId)
      {
         switch (statId)
         {
            case StatId.Hp:
               var health =(int) _session.StatsModel.GetValue(statId);
               _session.Data.Hp.Value = health;
               _health.SetHealth(health);
               break;
         }
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
         if(id == SwordId)
            UpdateHeroWeapon();
         
      }

      public void OnHealthChanged(int currentHealth)
      {
         _session.Data.Hp.Value = currentHealth;
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
          if (!IsGrounded && _allowDoubleJump && _session.PerksModel.IsDoubleJumpSupported && !_isOnWall) 
          {
             _session.PerksModel.Cooldown.Reset();
             _allowDoubleJump = false;
            DoJumpVfx();
            return _jumpSpeed; }
          return base.CalculateJumpVelocity(yVelocity);

      }


      public void AddInInventory(string id , int value)
      {
         _session.Data.Inventory.Add(id,value);
      }
      public void AddInInventoryBig(string id , int value)
      {
         _session.Data.Inventory.AddToBig(id,value);
      }
      
      public override void TakeDamage()
      {
         base.TakeDamage();
         _cameraShake.Shake();
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
         if (_superThrow && _session.PerksModel.IsSuperThrowSupported)
         {
            var throwableCount = _session.Data.Inventory.Count(SelectedItemId);
            var possibleCount = SelectedItemId == SwordId ? throwableCount - 1 : throwableCount;
            
            var numThrows = Mathf.Min(_superThrowParticles, possibleCount);
            _session.PerksModel.Cooldown.Reset();
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
         
         
         var throwableId = _session.QuickInventory.SelectedItem.Id;
         var throwableDef = DefsFacade.I.Throwable.Get(throwableId);
         _throwSpawner.SetPrefab(throwableDef.Projectile);
         var instance = _throwSpawner.SpawnInstance();
         ApplyRangeDamageStat(instance);
   
         _session.Data.Inventory.Remove(throwableId, 1);
      }

      private void ApplyRangeDamageStat(GameObject projectile)
      {
         var hpChange = projectile.GetComponent<ChangeHealthComponent>();
         var damageValue =(int) _session.StatsModel.GetValue(StatId.RangeDamage);
         damageValue = ModifyDamageByCrit(damageValue);
         hpChange.SetDelta(-damageValue);
      }

      private int ModifyDamageByCrit(int damage)
      {
         var critChange = _session.StatsModel.GetValue(StatId.CriticalDamage);
         if (Random.value * 100 <= critChange)
         {
             return damage * 2;
         }
         return damage;
      }


      /* public void UsePotionR()
      {
         if (SelectedItemId == BottleId)
         {
            var def = DefsFacade.I.Items.Get(SelectedItemId);
            def.HasTag(ItemTag.Usable);
            var currentHealth = 5;
            _session.Data.Hp.Value += currentHealth;
            GetComponent<HealthComponent>().ChangeHealth(+5);
             _session.Data.Inventory.Remove(BottleId, 1);
            
            
         }

         else
         {
            if (SelectedItemId != Bottle10Id) return;
            var def = DefsFacade.I.Items.Get(SelectedItemId);
            def.HasTag(ItemTag.Usable);
            var currentHealth = 10;
            _session.Data.Hp.Value += currentHealth;
            GetComponent<HealthComponent>().ChangeHealth(+10);
            _session.Data.Inventory.Remove(Bottle10Id, 1);
         } 
         
         Debug.Log($"Health changed, current Health : {_session.Data.Hp.Value}");

      }*/

      public void StartThrowing()
      {
         _superThrowCooldown.Reset();
      }

      public void UseInventory()
      {
         var isThrowable = _session.QuickInventory.SelectedDef.HasTag(ItemTag.Throwable);
         if (IsSelectedItem(ItemTag.Throwable))
         {
            PerformThrowing();
         }
         
         else if (IsSelectedItem(ItemTag.Potion))
         {
            UsePotion();
         }
      }

      private void UsePotion()
      {
         var potion = DefsFacade.I.Potions.Get(SelectedItemId);

         switch (potion.Effect)
         {
            case Effect.AddHp:
               _session.Data.Hp.Value += (int) potion.Value;
               Debug.Log($"Health changed, current Health : {_session.Data.Hp.Value}");
               break;
            case Effect.SpeedUp:
               _speedUpCooldown.Value= _speedUpCooldown.RemainingTime + potion.Time;
               _additionalSpeed = Mathf.Max(potion.Value, _additionalSpeed);
               _speedUpCooldown.Reset();
               Debug.Log($"Speed changed, current Speed : {_speed + _additionalSpeed} ");
               break;
         }
         
         _session.Data.Inventory.Remove(potion.Id, 1);
      }

      protected override float CalculateSpeed()
      {
         if (_speedUpCooldown.IsReady)
            _additionalSpeed = 0f;


         var defaultSpeed = _session.StatsModel.GetValue(StatId.Speed) + _additionalSpeed;
         return defaultSpeed + _additionalSpeed;
      }

      private bool IsSelectedItem(ItemTag tag)
      {
         return _session.QuickInventory.SelectedDef.HasTag(tag);

      }
      private bool IsSelectedItems(ItemTag tag)
      {
         return _session.BigInventory.SelectedDef.HasTag(tag);

      }
      private void PerformThrowing()
      {
         if (!_throwCooldown.IsReady || !CanThrow) return;

         if (_superThrowCooldown.IsReady) _superThrow = true;

         Animator.SetTrigger(ThrowKey);
         _throwCooldown.Reset();
      }

      public void Dash()
      {
         if (_DashCooldown.IsReady && _session.PerksModel.IsDashSupported)
         {
            var newPosition = Rigidbody.position + new Vector2(_Dash * transform.localScale.x, 0);
            Rigidbody.MovePosition(newPosition);
            _session.PerksModel.Cooldown.Reset();

            
            _DashCooldown.Reset();
         }
      }

      public void NextItem()
      {
         _session.QuickInventory.SetNextItem();
      }
      public void NextItems()
      {
         _session.BigInventory.SetNextItem();
      }
      
      public void AddQuickInventory()
      {
         var bigInventorySelectedItem = _session.BigInventory.GetSelectedItem();
         _session.QuickInventory.AddQuickInventoryItem(bigInventorySelectedItem);
      }

      public void UsePerk()
      {
         if (_session.PerksModel.IsShieldSupported)
         {
            _shield.Use();
            _session.PerksModel.Cooldown.Reset();
         }
      }


      public void ToggleFlashlight()
      {
         var isActive = _flashLight.gameObject.activeSelf;
         _flashLight.gameObject.SetActive(!isActive);
      }
   }
}
