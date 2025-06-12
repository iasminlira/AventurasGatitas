using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CatController : MonoBehaviour
{
    [Header("Movimento")]
    public float maxSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 15f;
    public float airControlMultiplier = 0.5f;

    [Header("Pulo")]
    public float jumpForce = 7f;
    public float gravityScale = 2f;
    public float lowJumpMultiplier = 3f;
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.1f;

    [Header("Referências")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public StarManager starManager; // ⭐ Referência para usar estrelas

    private Animator anim;
    private Rigidbody rb;

    private float moveInput;
    private float currentSpeed;

    private bool isGrounded;
    private bool wasGrounded;
    private bool isJumping;
    private bool hasDoubleJumped;

    private float coyoteTimer;
    private float jumpBufferTimer;

    public bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    void Update()
    {
        if (!canMove) return;

        moveInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Coyote Time
        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
            hasDoubleJumped = false; // Reset pulo duplo ao tocar o chão
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        // Jump Buffer
        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;

        // Pulo com buffer e coyote time
        if (jumpBufferTimer > 0 && coyoteTimer > 0)
        {
            jumpBufferTimer = 0;
            coyoteTimer = 0;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0f);
            isJumping = true;
        }
        // Pulo duplo gastando estrela
        else if (jumpBufferTimer > 0 && !isGrounded && !hasDoubleJumped && starManager.UseStar())
        {
            jumpBufferTimer = 0;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0f);
            isJumping = true;
            hasDoubleJumped = true;
        }

        // Pulo variável (segura = alto / solta = curto)
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, 0f);
            isJumping = false;
        }

        HandleRotation();
        HandleAnimation();
    }

    void FixedUpdate()
    {
        float targetSpeed = moveInput * maxSpeed;
        float accelRate = isGrounded ? acceleration : acceleration * airControlMultiplier;

        if (moveInput == 0)
            accelRate = deceleration;

        currentSpeed = Mathf.MoveTowards(rb.velocity.x, targetSpeed, accelRate * Time.fixedDeltaTime);

        Vector3 velocity = rb.velocity;
        velocity.x = currentSpeed;
        velocity.z = 0f;
        rb.velocity = velocity;

        // Gravidade ajustada
        float gravity = Physics.gravity.y * gravityScale;
        if (rb.velocity.y < 0)
            gravity *= lowJumpMultiplier;

        rb.AddForce(Vector3.up * gravity, ForceMode.Acceleration);
    }

    void HandleAnimation()
    {
        bool justLanded = !wasGrounded && isGrounded;
        wasGrounded = isGrounded;

        if (anim)
        {
            anim.SetBool("IsGrounded", isGrounded);
            anim.SetFloat("Speed", Mathf.Abs(currentSpeed));

            if (justLanded)
                anim.SetTrigger("JustLanded");
        }
    }

    void HandleRotation()
    {
        if (moveInput != 0)
        {
            float yRot = moveInput > 0 ? 90f : -90f;
            transform.rotation = Quaternion.Euler(0f, yRot, 0f);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Star"))
    {
        starManager.CollectStar();
        Destroy(other.gameObject);
    }
}
    
}
