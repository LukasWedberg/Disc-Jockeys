using UnityEngine;

public class DiscController : MonoBehaviour
{
    private Vector3 previousMousePosition;
    private float rotationalVelocity;
    private bool isSpinning = false;

    [Header("Settings")]
    public float mouseMovementMultiplier = -10f;
    public float mouseWheelMultiplier = 100f;
    public float speedThreshold = 100f;
    public float spinDetectionRadius = 1.0f;
    public bool isSpinningLeft = false;
    public bool isSpinningRight = false;


    void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(0, 0, rotationalVelocity * Time.deltaTime);
            return;
        }

        float rotationAmount = 0f;
        
        // Use screen position directly
        Vector2 mousePosition = Input.mousePosition;
        
        if (previousMousePosition != Vector3.zero)
        {
            float mouseMovementX = mousePosition.x - previousMousePosition.x;
            rotationAmount += mouseMovementX * mouseMovementMultiplier;
        }
        previousMousePosition = mousePosition;

        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        rotationAmount += mouseWheel * mouseWheelMultiplier;

        transform.Rotate(0, 0, rotationAmount);
        rotationalVelocity = rotationAmount / Time.deltaTime;
        float rotationSpeed = Mathf.Abs(rotationalVelocity);

        if (rotationSpeed >= speedThreshold && IsSpinObjectNearby())
        {
            isSpinning = true;
            isSpinningLeft = rotationAmount > 0;
            isSpinningRight = rotationAmount < 0;
        }
    }

    private bool IsSpinObjectNearby()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, spinDetectionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Spin"))
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spin"))
        {
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