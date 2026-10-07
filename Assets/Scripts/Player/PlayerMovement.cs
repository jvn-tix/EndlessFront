using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Walk Effects")]
    public ParticleSystem dustParticle;
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 movementDirection;
    private bool isGrounded;
    public bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>();
        
        if(context.started && isGrounded && movementDirection.x != 0)
        {
            PlayDust();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);

            PlayDust();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movementDirection.x * moveSpeed, rb.linearVelocity.y);

        HandleFlip();

        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void HandleFlip()
    {
        if (movementDirection.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (movementDirection.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;

        if(isGrounded)
        {
            PlayDust();
        }
    }

    public void PlayDust()
    {
        if (dustParticle != null)
        {
            dustParticle.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", !isGrounded);

            PlayDust();
        }
    }
}