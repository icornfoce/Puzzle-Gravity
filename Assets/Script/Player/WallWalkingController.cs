using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WallWalkingController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float groundAlignmentSpeed = 10f;
    public float jumpForce = 8f;
    
    [Header("Look Settings")]
    public Transform playerCamera;
    public float mouseSensitivity = 2f;
    public float verticalLookLimit = 85f;
    private float verticalRotation = 0f;

    [Header("Gravity & Surface Settings")]
    public float gravityForce = 20f;
    public float groundCheckDistance = 1.2f;
    public float wallCheckDistance = 0.8f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Vector3 currentSurfaceNormal = Vector3.up;
    private Vector3 targetUp = Vector3.up;
    private bool isGrounded = false;
    private bool shouldJump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; 
        rb.freezeRotation = true; 

        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>()?.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleCameraRotation();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            shouldJump = true;
        }
    }

    void FixedUpdate()
    {
        HandleSurfaceAlignment();
        HandleMovement();
        HandleJump();
        ApplyCustomGravity();
    }

    private void HandleCameraRotation()
    {
        if (playerCamera == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up, mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
        
        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleSurfaceAlignment()
    {
        RaycastHit hit;
        bool foundSurface = false;

        // 1. Check Forward (to flip onto walls we walk into)
        // We cast a bit forward from the center/bottom
        Vector3 forwardCheckPos = transform.position + (transform.up * 0.1f);
        if (Physics.Raycast(forwardCheckPos, transform.forward, out hit, wallCheckDistance, groundLayer))
        {
            targetUp = hit.normal;
            foundSurface = true;
        }
        // 2. Check Downward (to stay on current surface)
        else if (Physics.Raycast(transform.position, -transform.up, out hit, groundCheckDistance, groundLayer))
        {
            targetUp = hit.normal;
            foundSurface = true;
        }

        if (foundSurface)
        {
            // Smoothly interpolate the surface normal
            currentSurfaceNormal = Vector3.Lerp(currentSurfaceNormal, targetUp, Time.fixedDeltaTime * groundAlignmentSpeed);

            // Calculate change in rotation to align with new normal
            Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, currentSurfaceNormal);
            transform.rotation = slopeRotation * transform.rotation;
        }

        isGrounded = foundSurface;
    }

    private void HandleJump()
    {
        if (shouldJump)
        {
            // Remove previous vertical velocity before jumping to ensure consistent jump height
            Vector3 verticalVelocity = Vector3.Project(rb.linearVelocity, transform.up);
            rb.linearVelocity -= verticalVelocity;

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            shouldJump = false;
            isGrounded = false;
        }
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = (transform.right * moveX + transform.forward * moveZ).normalized;
        Vector3 targetVelocity = moveDir * moveSpeed;
        
        // Preserve vertical velocity (relative to player) for gravity/jumping effects
        Vector3 verticalVelocity = Vector3.Project(rb.linearVelocity, transform.up);
        rb.linearVelocity = targetVelocity + verticalVelocity;
    }

    private void ApplyCustomGravity()
    {
        // Apply gravity towards the surface we are currently aligning to
        rb.AddForce(-currentSurfaceNormal * gravityForce, ForceMode.Acceleration);
    }

    private void OnDrawGizmosSelected()
    {
        // For debugging in editor
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up * groundCheckDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + (transform.up * 0.1f), transform.forward * wallCheckDistance);
    }
}
