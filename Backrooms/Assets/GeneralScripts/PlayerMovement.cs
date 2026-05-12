using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float sprintMultiplier = 2f;
    public float jumpHeight = 2f;
    public float gravity = -20f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Sprint")]
    public float sprintDuration = 10f;
    public float sprintCooldown = 30f;

    [Header("Footsteps")]
    public AudioClip walkingSound;
    public float walkVolume = 0.5f;
    public float sprintVolume = 0.8f;

    [Header("Landing")]
    public AudioClip landingSound;
    public float landingVolume = 0.7f;
    public float landingDelay = 0.3f;

    [Header("Breathing")]
    public AudioClip breathingSound;
    public float breathingVolume = 0.6f;
    public float breathingFadeSpeed = 2f;
    public float breathingFadeOutDuration = 3f;

    private AudioSource footstepSource;
    private AudioSource landingSource;
    private AudioSource breathingSource;
    private CharacterController charCtrl;
    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;
    private float landingTimer = 0f;
    private bool hasJumped = false;     // ✅ tracks if player has jumped

    public bool isSprinting = false;
    private bool isOnCooldown = false;
    private float sprintTimer = 0f;
    private float cooldownTimer = 0f;

    void Start()
    {
        charCtrl = GetComponent<CharacterController>();

        footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.clip = walkingSound;
        footstepSource.loop = true;
        footstepSource.spatialBlend = 0f;
        footstepSource.playOnAwake = false;

        landingSource = gameObject.AddComponent<AudioSource>();
        landingSource.loop = false;
        landingSource.spatialBlend = 0f;
        landingSource.playOnAwake = false;
        landingSource.volume = landingVolume;

        breathingSource = gameObject.AddComponent<AudioSource>();
        breathingSource.clip = breathingSound;
        breathingSource.loop = true;
        breathingSource.spatialBlend = 0f;
        breathingSource.playOnAwake = false;
        breathingSource.volume = 0f;
    }

    void Update()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask, QueryTriggerInteraction.Ignore);

        // ✅ Landing: only plays if player actually jumped, with delay
        if (!wasGrounded && isGrounded && hasJumped)
        {
            landingTimer = landingDelay;
            hasJumped = false;
        }

        if (landingTimer > 0f)
        {
            landingTimer -= Time.deltaTime;
            if (landingTimer <= 0f)
                landingSource.PlayOneShot(landingSound);
        }

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

        if (Input.GetKeyUp(KeyCode.LeftShift) && isSprinting)
        {
            isSprinting = false;
            isOnCooldown = true;
            cooldownTimer = sprintCooldown * (1f - (sprintTimer / sprintDuration));
        }

        // Movement
        float currentSpeed = isSprinting ? speed * sprintMultiplier : speed;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.right * h + transform.forward * v;
        charCtrl.Move(move * currentSpeed * Time.deltaTime);

        // Footstep audio
        bool isMoving = (h != 0 || v != 0) && isGrounded;

        if (isMoving)
        {
            if (!footstepSource.isPlaying)
                footstepSource.Play();

            footstepSource.pitch = isSprinting ? 1.5f : 1f;
            footstepSource.volume = isSprinting ? sprintVolume : walkVolume;
        }
        else
        {
            if (footstepSource.isPlaying)
                footstepSource.Stop();
        }

        // Breathing audio
        if (isSprinting)
        {
            if (!breathingSource.isPlaying)
                breathingSource.Play();

            breathingSource.volume = Mathf.MoveTowards(
                breathingSource.volume,
                breathingVolume,
                breathingFadeSpeed * Time.deltaTime
            );
        }
        else
        {
            breathingSource.volume = Mathf.MoveTowards(
                breathingSource.volume,
                0f,
                (breathingVolume / breathingFadeOutDuration) * Time.deltaTime
            );

            if (breathingSource.volume == 0f && breathingSource.isPlaying)
                breathingSource.Stop();
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            hasJumped = true;   // ✅ flag that we jumped so landing sound can play
        }

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