using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private TrailRenderer tr;

    private bool canDash = true;
    private bool isDashing;
    private float dashTime = 0.2f;
    private float dashCD = 1f;
    private KeyCode dashButton = KeyCode.LeftShift;

    private int jumpsLeft;

    private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

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
        jumpsLeft = extraJumps;
        dashTime = dash.castTime;
        dashCD = dash.castCooldown;
        dashButton = dash.keyPress;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        bool jumpPress = Input.GetButtonDown("Jump");
        bool jumpRelease = Input.GetButtonUp("Jump");
        bool dash = Input.GetKeyDown(dashButton);
        rb.velocity = new Vector2(horizontal * horizontalSpeed, rb.velocity.y);
        if(IsGrounded())
        {
            if(rb.velocity.y <= 0f)
            {
                jumpsLeft = extraJumps;
            }
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpPress)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
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
        if (dash && canDash)
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            float aimAngle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x);
            StartCoroutine(Dash(aimAngle));
        }
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
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayer);
        return raycastHit2D.collider != null;
    }
    
    IEnumerator Dash(float angle)
    {
        ParticleSystem.EmissionModule em = dashEffect.emission;
        ParticleSystem.ShapeModule sm = dashEffect.shape;
        sm.rotation = new Vector3(0f, -1f * angle * Mathf.Rad2Deg - 90f, 0f);
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        em.rateOverTime = 100;
        rb.gravityScale = 0f;
        
        rb.velocity = new Vector2(Mathf.Cos(angle) * dashVelocity, Mathf.Sin(angle) * dashVelocity);
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        tr.emitting = false;
        em.rateOverTime = 0;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject collided = collision.gameObject;
        if(collided.layer == LayerMask.NameToLayer("Enemy")) {
            collided.GetComponent<EnemyBehavior>().TakeDamage(10);
        }
    }
}