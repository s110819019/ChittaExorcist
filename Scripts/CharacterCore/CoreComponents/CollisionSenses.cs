using UnityEngine;

using ChittaExorcist.Common.Generics;

namespace ChittaExorcist.CharacterCore
{
    /// <summary>
    /// Core System 角色碰撞偵測
    /// </summary>
    public class CollisionSenses : CoreComponent
    {
        #region w/ Core Components
        
        // Movement
        private Movement _movement;
        private Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);

        #endregion

        #region w/ Check Transform and Distance

        [Header("Check Transforms")]
        [SerializeField] private Transform groundCheck;             // 地面判斷
        [Space]
        [SerializeField] private Transform wallCheck;               // 牆壁判斷
        [Space]
        [SerializeField] private Transform ledgeHorizontalCheck;    // 邊緣水平判斷
        [SerializeField] private Transform ledgeVerticalCheck;      // 邊緣垂直判斷
        [SerializeField] private Transform ledgeVerticalBackCheck;  // 邊緣垂直判斷(背後)
        [Space]
        [SerializeField] private Transform ceilingCheck;            // 天花板判斷
        
        [Header("Check Variables")]
        [SerializeField] private LayerMask whatIsGround;    // 地面圖層
        [SerializeField] private float groundCheckRadius;   // 地面判斷範圍
        [SerializeField] private float wallCheckDistance;   // 牆壁判斷距離
        [SerializeField] private float groundCheckLongDistance;
        
        // Check Transforms
        public LayerMask WhatIsGround { get => whatIsGround; private set => whatIsGround = value; }
        public Transform GroundCheck { get => GenericNotImplementedError<Transform>.TryGet(groundCheck, Core.transform.parent.name); private set => groundCheck = value; }
        public Transform WallCheck { get => GenericNotImplementedError<Transform>.TryGet(wallCheck, Core.transform.parent.name); private set => wallCheck = value; }
        public Transform LedgeHorizontalCheck { get => GenericNotImplementedError<Transform>.TryGet(ledgeHorizontalCheck, Core.transform.parent.name); private set => ledgeHorizontalCheck = value; }
        public Transform LedgeVerticalCheck { get => GenericNotImplementedError<Transform>.TryGet(ledgeVerticalCheck, Core.transform.parent.name); private set => ledgeVerticalCheck = value; }
        public Transform LedgeVerticalBackCheck { get => GenericNotImplementedError<Transform>.TryGet(ledgeVerticalBackCheck, Core.transform.parent.name); private set => ledgeVerticalBackCheck = value; }
        public Transform CeilingCheck { get => GenericNotImplementedError<Transform>.TryGet(ceilingCheck, Core.transform.parent.name); private set => ceilingCheck = value; }
        // Check Variables
        public float GroundCheckRadius { get => groundCheckRadius; private set => groundCheckRadius = value; }
        public float WallCheckDistance { get => wallCheckDistance; private set => wallCheckDistance = value; }
        public float GroundCheckLongDistance { get => groundCheckLongDistance; private set => groundCheckLongDistance = value; }
        
        #endregion

        #region w/ Check Bools
        
        // 地面上
        public bool Ground =>
            Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, WhatIsGround);
        // 牆在前
        public bool WallFront =>
            Physics2D.Raycast(WallCheck.position, Vector2.right * Movement.FacingDirection, WallCheckDistance, WhatIsGround);
        // 牆在後
        public bool WallBack =>
            Physics2D.Raycast(WallCheck.position, Vector2.right * -Movement.FacingDirection, WallCheckDistance, WhatIsGround);
        // 平行邊緣
        public bool LedgeHorizontal =>
            Physics2D.Raycast(LedgeHorizontalCheck.position, Vector2.right * Movement.FacingDirection, WallCheckDistance, WhatIsGround);
        // 垂直邊緣
        public bool LedgeVertical =>
            Physics2D.Raycast(LedgeVerticalCheck.position, Vector2.down, WallCheckDistance, WhatIsGround);
        // 垂直邊緣(背後)
        public bool LedgeVerticalBack =>
            Physics2D.Raycast((LedgeVerticalBackCheck.position), Vector2.down, WallCheckDistance, WhatIsGround);
        // 天花板
        public bool Ceiling => Physics2D.OverlapCircle(CeilingCheck.position, GroundCheckRadius, WhatIsGround);
        
        // public bool GroundLarge => Physics2D.OverlapCircle(GroundCheck.position, groundCheckLargeRadius, WhatIsGround);
        // public bool GroundLarge => Physics2D.Raycast(WallCheck.position, Vector2.down, groundCheckLargeRadius, WhatIsGround);
        
        // 
        public bool GroundLong =>
            Physics2D.Raycast((GroundCheck.position), Vector2.down, GroundCheckLongDistance, WhatIsGround);

        #endregion

        #region w/ Gizmos

        public void OnDrawGizmosSelected()
        {
            if (Core == null) return;
            
            // 地面判斷
            Gizmos.color = Color.yellow;
            if (groundCheck != null)
            {
                Gizmos.DrawWireSphere(GroundCheck.position, GroundCheckRadius);
                // Gizmos.DrawWireSphere(GroundCheck.position, groundCheckLargeRadius);
                Gizmos.DrawLine(GroundCheck.position, GroundCheck.position + (Vector3)(Vector2.down * GroundCheckLongDistance));
            }
            // 天花板判斷
            Gizmos.color = Color.yellow;
            if (ceilingCheck != null)
            {
                Gizmos.DrawWireSphere(CeilingCheck.position, GroundCheckRadius);
            }
            // 牆壁距離
            Gizmos.color = Color.green;
            if (wallCheck != null)
            {
                Gizmos.DrawLine(WallCheck.position, WallCheck.position + (Vector3)(Vector2.right * Movement.FacingDirection * WallCheckDistance));
                // Gizmos.DrawLine(WallCheck.position, WallCheck.position + (Vector3) (Vector2.down * groundCheckLargeRadius));
            }
            // 邊緣水平判斷
            Gizmos.color = Color.red;
            if (ledgeHorizontalCheck != null)
            {
                Gizmos.DrawLine(LedgeHorizontalCheck.position, LedgeHorizontalCheck.position + (Vector3)(Vector2.right * Movement.FacingDirection * WallCheckDistance));
            }
            // 邊緣垂直判斷
            Gizmos.color = Color.red;
            if (ledgeVerticalCheck != null)
            {
                Gizmos.DrawLine(LedgeVerticalCheck.position, LedgeVerticalCheck.position + (Vector3)(Vector2.down * WallCheckDistance));
            }
            if (ledgeVerticalBackCheck != null)
            {
                Gizmos.DrawLine(LedgeVerticalBackCheck.position, LedgeVerticalBackCheck.position + (Vector3)(Vector2.down * WallCheckDistance));
            }
        }

        #endregion
    }
}
