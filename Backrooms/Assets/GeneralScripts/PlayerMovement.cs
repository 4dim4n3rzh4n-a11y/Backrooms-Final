using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f;
    public float sprintSpeed = 5f;
    public float jumpHeight = 1f;
    public float gravity = -20f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Sprint")]
    public float sprintDuration = 10f;      // how long sprint lasts
    public float sprintCooldown = 30f;      // delay before can sprint again

    private CharacterController charCtrl;
    private Vector3 velocity;
    private bool isGrounded;

    public bool isSprinting = false;
    private bool isOnCooldown = false;
    private float sprintTimer = 0f;
    private float cooldownTimer = 0f;

    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask, QueryTriggerInteraction.Ignore);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Sprint logic
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isOnCooldown)
        {
            isSprinting = true;
            sprintTimer = sprintDuration;
        }

        if (isSprinting)
        {
            sprintTimer -= Time.deltaTime;

            if (sprintTimer <= 0f)
            {
                isSprinting = false;
                isOnCooldown = true;
                cooldownTimer = sprintCooldown;
            }
        }

        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
                isOnCooldown = false;
        }

        // Also stop sprinting if player releases Shift early
        if (Input.GetKeyUp(KeyCode.LeftShift) && isSprinting)
        {
            isSprinting = false;
            isOnCooldown = true;
            cooldownTimer = sprintCooldown * (1f - (sprintTimer / sprintDuration));
        }

        // Movement
        float currentSpeed = isSprinting ? sprintSpeed : speed;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.right * h + transform.forward * v;
        charCtrl.Move(move * currentSpeed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        charCtrl.Move(velocity * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}