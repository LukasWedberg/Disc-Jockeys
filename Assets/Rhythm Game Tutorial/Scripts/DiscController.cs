using UnityEngine;

public class DiscController : MonoBehaviour
{
    private Vector3 previousMousePosition;

    void Update()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure it's 2D
        
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
        }

        // Store the mouse position for the next frame
        previousMousePosition = mousePosition;
    }
}