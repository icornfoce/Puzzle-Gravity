using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBox : MonoBehaviour
{
    public enum GravityDirection
    {
        Down,
        Up,
        Left,
        Right,
        Forward,
        Back
    }

    [Header("Gravity Settings")]
    public GravityDirection direction = GravityDirection.Down;
    public float gravityIntensity = 20f;
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Custom gravity
    }

    void FixedUpdate()
    {
        Vector3 gravityVec = GetGravityVector();
        rb.AddForce(gravityVec * gravityIntensity, ForceMode.Acceleration);
    }

    public void SetGravityDirection(GravityDirection newDir)
    {
        direction = newDir;
    }

    public void FlipGravity()
    {
        switch (direction)
        {
            case GravityDirection.Down: direction = GravityDirection.Up; break;
            case GravityDirection.Up: direction = GravityDirection.Down; break;
            case GravityDirection.Left: direction = GravityDirection.Right; break;
            case GravityDirection.Right: direction = GravityDirection.Left; break;
            case GravityDirection.Forward: direction = GravityDirection.Back; break;
            case GravityDirection.Back: direction = GravityDirection.Forward; break;
        }
    }

    public void CycleGravity()
    {
        int next = ((int)direction + 1) % 6;
        direction = (GravityDirection)next;
    }

    private Vector3 GetGravityVector()
    {
        switch (direction)
        {
            case GravityDirection.Up: return Vector3.up;
            case GravityDirection.Down: return Vector3.down;
            case GravityDirection.Left: return Vector3.left;
            case GravityDirection.Right: return Vector3.right;
            case GravityDirection.Forward: return Vector3.forward;
            case GravityDirection.Back: return Vector3.back;
            default: return Vector3.down;
        }
    }
}
