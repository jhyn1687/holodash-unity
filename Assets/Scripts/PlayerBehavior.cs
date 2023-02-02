using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private TrailRenderer tr;
    private SpriteRenderer sr;
    private Animator ani;

    private bool canDash = true;
    private bool isDashing;
    private float dashTime = 0.2f;
    private float dashCD = 1f;
    private KeyCode dashButton = KeyCode.LeftShift;

    private int jumpsLeft;

    private float horizontalInput;
    private float verticalInput;
    private Vector2 aimDirection;

    private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    private float originalGravity;

    private enum MovementState { idle, running, jumping, falling }
    private enum DashDirection { 
        North=90,
        South=-90,
        East=0, 
        West=180,
        NorthWest=135,
        SouthWest=-135,
        NorthEast=45,
        SouthEast=-45
    }

    [SerializeField] private Ability dash;
    [SerializeField] private ParticleSystem dashEffect;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private int extraJumps;
    [SerializeField] private int horizontalSpeed;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float dashVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        tr = GetComponent<TrailRenderer>();
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        jumpsLeft = extraJumps;
        dashTime = dash.castTime;
        dashCD = dash.castCooldown;
        dashButton = dash.keyPress;
        originalGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        bool jumpHold = Input.GetButton("Jump");
        bool jumpPress = Input.GetButtonDown("Jump");
        bool jumpRelease = Input.GetButtonUp("Jump");
        bool dash = Input.GetKeyDown(dashButton);
        if (IsGrounded()) {
            if (rb.velocity.y <= 0f) {
                jumpsLeft = extraJumps;
            }
            coyoteTimeCounter = coyoteTime;
        } else {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpHold) {
            jumpBufferCounter = jumpBufferTime;
        } else {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (isDashing)
        {
            return;
        }
        
        rb.velocity = new Vector2(horizontalInput * horizontalSpeed, rb.velocity.y);

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = mousePosition - (Vector2)this.transform.position;
        if (dash && canDash) {
            DashDirection dir = aimDirection.x < 0 ? DashDirection.West : DashDirection.East;
            if (horizontalInput > 0f) {
                dir = DashDirection.East;
                if (verticalInput > 0f) {
                    dir = DashDirection.NorthEast;
                } else if (verticalInput < 0f) {
                    dir = DashDirection.SouthEast;
                }
            } else if (horizontalInput < 0f) {
                dir = DashDirection.West;
                if (verticalInput > 0f) {
                    dir = DashDirection.NorthWest;
                } else if (verticalInput < 0f) {
                    dir = DashDirection.SouthWest;
                }
            } else if (verticalInput < 0f) {
                dir = DashDirection.South;
            } else if (verticalInput > 0f) {
                dir = DashDirection.North;
            }
            StartCoroutine(Dash((int)dir * Mathf.Deg2Rad));
            return;
        }
        
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        } 
        else if (jumpPress && jumpsLeft > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
            jumpsLeft--;
        }
        
        if (jumpRelease && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }
        
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        if(isDashing)
        {
            return;
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(new Vector2(coll.bounds.center.x, coll.bounds.center.y - .1f), new Vector2(coll.bounds.size.x -.1f, coll.bounds.size.y - .2f), 0f, Vector2.down, .1f, groundLayer);
        return raycastHit2D.collider != null;
    }
    
    private void UpdateAnimation() {
        MovementState animationState;
        // flip sprite if aiming in certain direction
        if (aimDirection.x < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

        // if player presses horizontal movement buttons
        // change to running animation
        if (horizontalInput > 0f) {
            animationState = MovementState.running;
        } else if (horizontalInput < 0f) {
            animationState = MovementState.running;
        } else {
            animationState = MovementState.idle;
        }

        // if midair and rising/falling, change to
        // jump/fall animations
        if (rb.velocity.y > .1f) {
            animationState = MovementState.jumping;
        } else if (rb.velocity.y < -.1f) {
            animationState = MovementState.falling;
        }

        ani.SetInteger("State", (int)animationState);
    }

    IEnumerator Dash(float angle)
    {
        // setting params for during dash
        ParticleSystem.EmissionModule em = dashEffect.emission;
        canDash = false;
        isDashing = true;
        Vector2 originalVelocity = rb.velocity;
        em.rateOverTime = 100;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(Mathf.Cos(angle) * dashVelocity, Mathf.Sin(angle) * dashVelocity);
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        // resetting params
        tr.emitting = false;
        em.rateOverTime = 0;
        rb.velocity = new Vector2(originalVelocity.x, 0);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCD);
        em.rateOverTime = 0;
        canDash = true;
    }

    private void OnEndZoneReached() {
        this.transform.position = new Vector2(2, 2);
    }

    private void OnReset() {
        // stop any dashes, and also reset everything that may have been changed in dash.
        StopAllCoroutines();
        jumpsLeft = extraJumps;
        dashTime = dash.castTime;
        dashCD = dash.castCooldown;
        dashButton = dash.keyPress;
        isDashing = false;
        canDash = true;
        tr.emitting = false;
        ParticleSystem.EmissionModule em = dashEffect.emission;
        em.rateOverTime = 0;
        this.transform.position = new Vector2(2, 2);
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = originalGravity;
    }

    private void OnEnable() {
        GameManager.OnReset += OnReset;
        EndzoneScript.EndzoneReached += OnEndZoneReached;
    }
    private void OnDisable() {
        GameManager.OnReset -= OnReset;
        EndzoneScript.EndzoneReached -= OnEndZoneReached;
    }
    // hackjob knockback
    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject collided = collision.gameObject;
        if (collided.CompareTag("Enemy")) {
            rb.velocity = new Vector2(0f, 10f);
        }
    }
}
