using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    private bool canDash = true;
    private bool isDashing;
    private float dashTime = 0.2f;
    private float dashCD = 1f;
    private int jumpsLeft;

    [SerializeField] private int numJumps;
    [SerializeField] private int horizontalSpeed;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float dashVelocity;
    [SerializeField] private LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        jumpsLeft = numJumps;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        bool jump = Input.GetButtonDown("Jump");
        bool dash = Input.GetButtonDown("Fire3");
        rb.velocity = new Vector2(horizontal * horizontalSpeed, rb.velocity.y);
        if(IsGrounded() && rb.velocity.y <= 0)
        {
            jumpsLeft = numJumps;
        }
        if (jump && jumpsLeft > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
            jumpsLeft -= 1;
        }
        if (dash && canDash)
        {
            StartCoroutine(Dash());
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
    
    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * dashVelocity, 0f);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }
}
