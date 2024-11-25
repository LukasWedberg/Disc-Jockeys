using UnityEngine;

public class DiscController : MonoBehaviour
{
    private Vector3 previousMousePosition;
    private float previousAngle;
    private float deltaAngle;
    private float rotationSpeed;
    private float rotationalVelocity;
    private bool isSpinning = false;
    private float velocityDecrement; 

    [Header("Settings")]
    public float speedThreshold = 100f; // Speed threshold for spinning
    public bool isSpinningLeft = false; // Indicates spinning left
    public bool isSpinningRight = false; // Indicates spinning right
    public float stopTimeInSeconds = 2f; // Time to stop spinning (set in Inspector)
    

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        if (Input.GetMouseButton(0))
        {
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

                // Calculate velocity decrement dynamically based on stopTimeInSeconds
                velocityDecrement = Mathf.Abs(rotationalVelocity) / stopTimeInSeconds;

                // Determine spinning state and direction
                isSpinning = rotationSpeed >= speedThreshold;
                if (isSpinning)
                {
                    isSpinningLeft = deltaAngle > 0;
                    isSpinningRight = deltaAngle < 0;
                }
                else
                {
                    isSpinningLeft = false;
                    isSpinningRight = false;
                }
            }

            previousAngle = angle;
        }
        else if (isSpinning)
        {
            // Apply momentum when the mouse button is released
            transform.Rotate(0, 0, rotationalVelocity * Time.deltaTime);

            // Gradually reduce rotational velocity to stop after `stopTimeInSeconds`
            rotationalVelocity = Mathf.MoveTowards(rotationalVelocity, 0, velocityDecrement * Time.deltaTime);

            // Stop spinning when velocity drops to zero
            if (Mathf.Abs(rotationalVelocity) < Mathf.Epsilon)
            {
                isSpinning = false;
                isSpinningLeft = false;
                isSpinningRight = false;
            }
        }

        // Store the mouse position for the next frame
        previousMousePosition = mousePosition;
    }
}
