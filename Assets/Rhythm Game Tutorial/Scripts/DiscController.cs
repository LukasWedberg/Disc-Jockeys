using UnityEngine;

public class DiscController : MonoBehaviour
{
    private Vector3 previousMousePosition;
    private float previousAngle;
    private float deltaAngle;
    private float rotationSpeed;
    private float rotationalVelocity;
    private bool isSpinning = false;

    [Header("Settings")]
    public float speedThreshold = 100f; // Speed threshold for spinning
    public float spinDetectionRadius = 1.0f; // Radius to detect Spin objects
    public bool isSpinningLeft = false; // Indicates spinning left
    public bool isSpinningRight = false; // Indicates spinning right

    void Update()
    {
        // If already spinning, ignore mouse inputs
        if (isSpinning)
        {
            // Apply momentum while spinning
            transform.Rotate(0, 0, rotationalVelocity * Time.deltaTime);
            return;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Calculate the vector from the disc to the mouse position
        Vector3 toCurrentMouse = mousePosition - transform.position;

        // Calculate the vector from the disc to the previous mouse position
        Vector3 toPreviousMouse = previousMousePosition - transform.position;

        // Calculate the angle between the two vectors
        float angle = Vector3.SignedAngle(toPreviousMouse, toCurrentMouse, Vector3.forward);

        // Rotate the disc
        transform.Rotate(0, 0, angle);

        // Only calculate deltaAngle and rotationSpeed if this is not the first frame
        if (previousMousePosition != Vector3.zero)
        {
            deltaAngle = angle - previousAngle;
            rotationSpeed = Mathf.Abs(deltaAngle) / Time.deltaTime;

            // Set rotational velocity based on the current frame's delta angle
            rotationalVelocity = deltaAngle / Time.deltaTime;

            // Determine spinning state
            if (rotationSpeed >= speedThreshold && IsSpinObjectNearby())
            {
                isSpinning = true;
                isSpinningLeft = deltaAngle > 0;
                isSpinningRight = deltaAngle < 0;
            }
        }

        // Store the mouse position for the next frame
        previousMousePosition = mousePosition;
    }

    private bool IsSpinObjectNearby()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, spinDetectionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Spin"))
            {
                return true; // A Spin object is nearby
            }
        }
        return false; // No Spin object nearby
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Spin"))
        {
            // Stop spinning when colliding with a Spin object
            isSpinning = false;
            isSpinningLeft = false;
            isSpinningRight = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spinDetectionRadius);
    }
}
