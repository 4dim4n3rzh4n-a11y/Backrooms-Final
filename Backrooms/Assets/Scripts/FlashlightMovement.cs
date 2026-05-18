using UnityEngine;

public class FlashlightSway : MonoBehaviour
{
    [Header("Sway Settings")]
    public float swayAmount = 0.05f;
    public float swaySpeed = 6f;
    public float rotationSwayAmount = 3f;
    public float rotationSwaySpeed = 5f;

    [Header("Sensitivity")]
    public float mouseSensitivityMultiplier = 1.8f;  // ✅ more sensitive than camera

    [Header("Bob")]
    public float walkBobAmount = 0.04f;
    public float walkBobSpeed = 9f;
    public float sprintBobAmount = 0.09f;
    public float sprintBobSpeed = 15f;
    public float idleBobAmount = 0.012f;
    public float idleBobSpeed = 1.5f;

    private Vector3 defaultPos;
    private Quaternion defaultRot;
    private float bobTimer = 0f;
    private PlayerMovement playerMovement;

    void Start()
    {
        defaultPos = transform.localPosition;
        defaultRot = transform.localRotation;
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityMultiplier;

        // ✅ Position sway — hand drifts opposite to mouse direction, like inertia
        Vector3 swayPos = new Vector3(-mouseX, -mouseY, 0f) * swayAmount;

        // ✅ Rotation sway — hand tilts with mouse movement, like a real grip
        Quaternion swayRot = Quaternion.Euler(
            -mouseY * rotationSwayAmount,
             mouseX * rotationSwayAmount,
            -mouseX * rotationSwayAmount * 0.5f  // slight roll
        );

        // Bob
        bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
        bool isSprinting = playerMovement.isSprinting;

        float bobSpeed;
        float bobAmount;

        if (isSprinting)
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

        bobTimer += Time.deltaTime * bobSpeed;

        Vector3 bobOffset = new Vector3(
            Mathf.Cos(bobTimer) * bobAmount * 0.6f,         // horizontal
            Mathf.Abs(Mathf.Sin(bobTimer)) * bobAmount,     // ✅ Abs so it bounces up not through floor
            Mathf.Sin(bobTimer * 0.5f) * bobAmount * 0.3f   // slight forward/back
        );

        // Combine and smooth everything
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            defaultPos + swayPos + bobOffset,
            Time.deltaTime * swaySpeed
        );

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            defaultRot * swayRot,
            Time.deltaTime * rotationSwaySpeed
        );
    }
}