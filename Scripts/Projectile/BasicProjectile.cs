using System;
using System.Collections;
using System.Collections.Generic;
using ChittaExorcist.Common;
using ChittaExorcist.Common.Interfaces;
using ChittaExorcist.GameCore;
using ChittaExorcist.Structs;
using Unity.Mathematics;
using UnityEngine;

namespace ChittaExorcist.ProjectileSettings
{
    public class BasicProjectile : MonoBehaviour, IProjectile, IParryable
    {
        [SerializeField] protected bool canGetParried = true;
        
        [SerializeField] protected GameObject hitParticles;
        
        [SerializeField] protected float startGravityScale = 0.0f;
        [SerializeField] protected float finalGravityScale = 0.0f;
        
        #region w/ Projectile Interface

        public Vector2 StartPosition { get; private set;}
        public Vector2 StartDirection { get; private set;}
        public float TravelTime { get; private set;}
        public AnimationCurve SpeedCurve { get; private set;}
        public float Damage { get; private set;}
        public float PoiseDamage { get; private set;}
        public float KnockbackStrength { get; private set;}
        public Vector2 KnockbackDirection { get; private set;}

        #endregion

        #region w/ Parryable Interface

        public bool IsParried { get; private set; }
        public bool IsSceneTrap { get; private set; }
        public Transform AttackTransform { get; set; }

        public void Parry()
        {
            IsParried = true;
        }
        
        public void CheckParryDetails(ParriedDetails parriedDetails)
        {
            if (parriedDetails.IsSetParriedProjectile)
            {
                ShouldChangeToParriedState = true;
                ParriedProjectileDetails = parriedDetails.ParriedProjectileDetails;
                // Debug.Log("Check Details");
            }
        }

        #endregion

        #region w/ Parry

        protected bool IsParriedState;
        protected bool ShouldChangeToParriedState;
        
        // Parry
        protected ParriedProjectileDetails ParriedProjectileDetails;
        private void SetParriedState()
        {
            if (canGetParried && ShouldChangeToParriedState)
            {
                IsParriedState = true;
                // ShouldChangeToParriedState = false;
                
                InteractableLayers = ParriedProjectileDetails.InteractableLayers;
                TravelTime = ParriedProjectileDetails.TravelTime;
                SpeedCurve = ParriedProjectileDetails.SpeedCurve;
                
                StartTime = Time.time;
                StartDirection = ProjectileUser.position - transform.position;
                float tempAngle = Mathf.Atan2(StartDirection.y, StartDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.identity;
                transform.rotation = Quaternion.AngleAxis(tempAngle, Vector3.forward);

                // TODO: 翻轉問題
                if (!ShouldFlip)
                {
                    if (GraphicSpriteRenderer != null)
                    {
                        GraphicSpriteRenderer.flipY = true;
                    }
                }
                else
                {
                    if (GraphicSpriteRenderer != null)
                    {
                        GraphicSpriteRenderer.flipY = false;
                    }
                }
                
                IsAttackOn = true;
            }
            else if (canGetParried)
            {
                ReturnProjectile();
            }
        }

        #endregion

        // protected bool GetParried;

        #region w/ Variables

        // direction
        protected int FacingDirection;

        // time
        protected float StartTime;
        protected float Duration => Time.time - StartTime;

        // gravity
        protected bool UseFallGravity;
        
        // graphic flip
        protected bool ShouldFlip;
        
        // can attack
        protected bool IsAttackOn;

        // use particle system
        protected bool IsUseParticleSystem;
        
        // use animator
        protected bool IsUseAnimator;
        
        // whether hit particles have been generated
        protected bool HasSpawnHitParticle;

        protected Vector2 Workspace;
        
        // interactive layers
        protected LayerMask InteractableLayers;
        
        // user
        protected Transform ProjectileUser;

        // detected which can hit
        protected Collider2D[] DetectedCollider2Ds;

        // if use animator, check animation finish or not to return object
        protected bool IsAnimationFinished;
        
        #endregion

        #region w/ Components

        protected Rigidbody2D Rigidbody2D;
        
        protected Collider2D Collider2D;

        protected ParticleSystem ParticleComp;
        
        protected GameObject Graphic;

        protected SpriteRenderer GraphicSpriteRenderer;

        #endregion

        #region w/ Animator

        protected Animator AnimatorComp;
        protected ProjectileAnimationEventHandler EventHandler;
        public void AnimationFinishTrigger()
        {
            IsAnimationFinished = true;
        }

        #endregion

        #region w/ Set Projectile

        public virtual void SetProjectileStartingState(ProjectileDetails details)
        {
            // interface
            StartPosition = details.StartPosition;
            StartDirection = details.StartDirection;
            TravelTime = details.TravelTime;
            SpeedCurve = details.SpeedCurve;
            Damage = details.Damage;
            PoiseDamage = details.PoiseDamage;
            KnockbackStrength = details.KnockbackStrength;
            KnockbackDirection = details.KnockbackDirection;
            
            StartTime = Time.time;
            
            UseFallGravity = details.UseFallGravity;
            ShouldFlip = details.ShouldFlip;
            ProjectileUser = details.ProjectileUser;
            InteractableLayers = details.InteractableLayers;

            // Attack Check
            IsAttackOn = true;

            // TODO: For Parry ?
            IsParriedState = false;
            ShouldChangeToParriedState = false;
            IsParried = false;

            IsAnimationFinished = false;


            if (IsUseAnimator)
            {
                AnimatorComp.SetBool("hit", false);
            }
            
            // Flip
            if (ShouldFlip)
            {
                if (GraphicSpriteRenderer != null)
                {
                    GraphicSpriteRenderer.flipY = true;
                }
            }
            else
            {
                if (GraphicSpriteRenderer != null)
                {
                    GraphicSpriteRenderer.flipY = false;
                }
            }

            transform.position = details.StartPosition;
            
            // 跟著旋轉
            float tempAngle = Mathf.Atan2(details.StartDirection.y, details.StartDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(tempAngle, Vector3.forward);

            // Fall
            Rigidbody2D.gravityScale = UseFallGravity ? finalGravityScale : startGravityScale;

            // RaycastHit2D[] hits =
            //     Physics2D.LinecastAll(transform.position, ProjectileUser.position, InteractableLayers);
            //
            // CheckHitCollider(hits);

            HasSpawnHitParticle = false;
        }

        #endregion

        #region w/ Check Hit

        protected virtual void CheckHitCollider(RaycastHit2D[] hits)
        {
            if (!IsAttackOn) return;

            FacingDirection = (int) (Vector2.right * Rigidbody2D.velocity).normalized.x;

            // Debug.Log($" {gameObject.name} Projectile Facing Direction: {FacingDirection}");
            
            foreach (var hit in hits)
            {
                if (hit.collider.transform.TryGetComponent(out IDamageable damageable))
                {
                    // damageable.Damage(Damage);
                    if (hit.transform.CompareTag("Player"))
                    {
                        AttackTransform = transform;
                        damageable.Damage(Damage, this);
                    }
                    else
                    {
                        damageable.Damage(Damage);
                    }
                    HitDamageable(hit);
                    IsAttackOn = false;
                }

                if (hit.collider.transform.TryGetComponent(out IKnockbackable knockbackable))
                {
                    // knockbackable.Knockback(KnockbackDirection, KnockbackStrength, FacingDirection);
                    if (hit.transform.CompareTag("Player"))
                    {
                        AttackTransform = transform;
                        knockbackable.Knockback(KnockbackDirection, KnockbackStrength, FacingDirection, this);
                    }
                    else
                    {
                        knockbackable.Knockback(KnockbackDirection, KnockbackStrength, FacingDirection);
                    }
                    IsAttackOn = false;
                }

                if (hit.transform.CompareTag("Ground"))
                {
                    HitGround(hit);
                    IsAttackOn = false;
                }

                if (!IsAttackOn)
                {
                    if (ShouldChangeToParriedState)
                    {
                        IsAttackOn = true;
                        ShouldChangeToParriedState = false;
                    }
                    return;
                }
            }
        }

        protected virtual void CheckHitBox()
        {
            if (!IsAttackOn)
            {
                return;
            }
            
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right,
                (Time.fixedDeltaTime * Rigidbody2D.velocity.magnitude) + 0.1f, InteractableLayers);

            // Debug.DrawLine(transform.position,
            //     transform.position + transform.right * ((Time.fixedDeltaTime * Rigidbody2D.velocity.magnitude) + 0.1f),
            //     Color.cyan);
            
            // Debug.Log("start: " + transform.position);
            // Debug.Log("end: " + (transform.position +
            //           transform.right * ((Time.fixedDeltaTime * Rigidbody2D.velocity.magnitude) + 0.1f)));
            
            // OverlapBox ?
            
            CheckHitCollider(hits);
        }

        #endregion
        
        #region w/ Hit Particles

        // TODO: 要翻轉嗎
        protected virtual void SpawnHitParticles()
        {
            if (HasSpawnHitParticle) return;
        
            HasSpawnHitParticle = true;

            if (hitParticles != null)
            {
                var hitParticle = ObjectPoolManager.Instance.GetObject(hitParticles);
                hitParticle.transform.position = transform.position;
                hitParticle.transform.rotation = transform.rotation;
            }
        }

        #endregion
        
        #region w/ Hit Which Target

        protected virtual void HitDamageable(RaycastHit2D hit)
        {
            if (IsParried && !IsParriedState)
            {
                SetParriedState();
                return;
            }

            if (ShouldChangeToParriedState)
            {
                return;
            }
            // Debug.Log("Ret");
            SpawnHitParticles();
            ReturnProjectile();
        }

        protected virtual void HitGround(RaycastHit2D hit)
        {
            SpawnHitParticles();
            ReturnProjectile();
        }

        protected virtual void ReturnProjectile()
        {
            if (Collider2D != null)
            {
                Collider2D.enabled = false;
            }
            
            // 停止粒子
            if (IsUseParticleSystem)
            {
                ParticleComp.Stop();
            }

            // 有結束動畫 => 使用 animator
            // 無結束動畫 => 讓 graphic 直接消失
            if (IsUseAnimator)
            {
                AnimatorComp.SetBool("hit", true);
            }
            else
            {
                if (Graphic != null)
                {
                    Graphic.SetActive(false);
                }
            }

            if (IsUseParticleSystem || IsUseAnimator)
            {
                return;
            }
            
            ObjectPoolManager.Instance.ReturnObject(gameObject);
        }

        protected virtual void CheckProjectileStateToReturn()
        {
            if (!IsUseAnimator && !IsUseParticleSystem) return;
            
            // 等兩者都結束
            if (IsUseParticleSystem && IsUseAnimator)
            {
                if (!ParticleComp.IsAlive() && IsAnimationFinished)
                {
                    ObjectPoolManager.Instance.ReturnObject(gameObject);
                }
            }
            // 等粒子結束
            else if (IsUseParticleSystem)
            {
                if (!ParticleComp.IsAlive())
                {
                    ObjectPoolManager.Instance.ReturnObject(gameObject);
                }
            }
            // 等動畫結束
            else if (IsUseAnimator)
            {
                if (IsAnimationFinished)
                {
                    ObjectPoolManager.Instance.ReturnObject(gameObject);
                }
            }
        }

        #endregion
        
        #region w/ Unity Callback Functions

        protected virtual void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            TryGetComponent(out Collider2D);

            // check if use particle system
            if (TryGetComponent<ParticleSystem>(out ParticleComp))
            {
                IsUseParticleSystem = true;
            }
            
            // 注意 Graphic 放在子物件第一個以供查找
            Graphic = transform.GetChild(0).gameObject;

            if (Graphic == null) return;

            // get sprite renderer
            Graphic.TryGetComponent<SpriteRenderer>(out GraphicSpriteRenderer);

            // check if use animator on graphic
            if (Graphic.TryGetComponent<Animator>(out AnimatorComp))
            {
                IsUseAnimator = true;
                if (!Graphic.TryGetComponent<ProjectileAnimationEventHandler>(out EventHandler))
                {
                    Debug.LogWarning($"No ProjectileAnimationEventHandler script on ${transform.name} !");
                }
            }
        }

        protected virtual void Update()
        {
            // SetProjectileAngle();
        
            // Debug.Log("Is Attack On"+ IsAttackOn);
            
            // 超過時間即返回物件
            if (Time.time >= StartTime + TravelTime)
            {
                ReturnProjectile();
            }

            // 是否能攻擊
            if (IsAttackOn)
            {
                Rigidbody2D.velocity = StartDirection.normalized * SpeedCurve.Evaluate(Duration);
            }
            else
            {
                // 停下
                Rigidbody2D.velocity = Vector2.zero;
            }

            CheckProjectileStateToReturn();
        }

        protected virtual void FixedUpdate()
        {
            if (IsAttackOn)
            {
                CheckHitBox();
            }
        }

        private void OnEnable()
        {
            if (Graphic != null)
            {
                Graphic.SetActive(true);
            }

            if (Collider2D != null)
            {
                Collider2D.enabled = true;
            }
            
            if (EventHandler != null)
            {
                EventHandler.OnFinish += AnimationFinishTrigger;
            }
        }

        private void OnDisable()
        {
            if (EventHandler != null)
            {
                EventHandler.OnFinish -= AnimationFinishTrigger;
            }
        }

        #endregion
    }
}
