using UnityEngine;

public class EnemyChaseAI : MonoBehaviour
{
    [Header("Atribut Pergerakan")]
    public float moveSpeed = 3f;
    public float stoppingDistance = 5f; // Jarak ideal musuh berhenti untuk nembak

    private Transform playerTransform;
    private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void FixedUpdate()
    {
        if (playerTransform == null || rb == null) return;

        // Hitung jarak ke Player
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        // 1. Jika jarak masih JAUH -> MAJU KEJAR
        if (distanceToPlayer > stoppingDistance)
        {
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
            animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        }
        // 2. Jika sudah DEKAT -> BERHENTI
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        // Flip Sprite menghadap Player
        if (direction.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}