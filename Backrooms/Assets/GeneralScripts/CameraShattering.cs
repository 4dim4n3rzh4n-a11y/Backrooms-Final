using UnityEngine;

public class CameraBob : MonoBehaviour
{
    [Header("Bob Settings")]
    public float walkBobSpeed = 10f;
    public float walkBobAmount = 0.05f;
    public float sprintBobSpeed = 16f;
    public float sprintBobAmount = 0.1f;
    public float idleBobSpeed = 2f;
    public float idleBobAmount = 0.01f;

    private float timer = 0f;
    private Vector3 defaultPos;
    private PlayerMovement playerMovement;

    void Start()
    {
        defaultPos = transform.localPosition;
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        float bobSpeed;
        float bobAmount;

        bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        if (playerMovement.isSprinting)
        {
            bobSpeed = sprintBobSpeed;
            bobAmount = sprintBobAmount;
        }
        else if (isMoving)
        {
            bobSpeed = walkBobSpeed;
            bobAmount = walkBobAmount;
        }
        else
        {
            bobSpeed = idleBobSpeed;
            bobAmount = idleBobAmount;
        }

        timer += Time.deltaTime * bobSpeed;

        Vector3 newPos = defaultPos;
        newPos.y += Mathf.Sin(timer) * bobAmount;
        newPos.x += Mathf.Cos(timer / 2f) * bobAmount * 0.5f; // ✅ slight horizontal sway too

        transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, Time.deltaTime * 10f);
    }
}