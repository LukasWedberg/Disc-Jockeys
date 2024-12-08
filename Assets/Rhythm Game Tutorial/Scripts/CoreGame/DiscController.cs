using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class DiscController : MonoBehaviour
{
    private Vector3 previousMousePosition;
    private float rotationalVelocity;
    public bool isSpinning = false;

    [Header("Settings")]
    public float mouseMovementMultiplier = -10f;
    public float mouseWheelMultiplier = 100f;
    public float speedThreshold = 100f;
    public float spinDetectionRadius = 1.0f;
    public bool isSpinningLeft = false;
    public bool isSpinningRight = false;


    //This is Lukas fixing the code for the spinning
    public float timeToStopSpinning = 1;
    public float spinTimer = 0;

    private BeatScroller beatScroller;

    public LayerMask layersToCheck;

    void FixedUpdate()
    {
        if (isSpinning)
        {
            //This is Lukas also fixing the code for the spinning.
            //I'm also gonna make it so that when the disc stops spinning,
            //it is automatically aligned with the nearest note

            spinTimer -= Time.deltaTime;


            float animProgress = Mathf.Clamp01(spinTimer) / timeToStopSpinning;

            float distToCheckAhead = FindObjectsOfType<RhythmManager>()[0].noteSpeed * animProgress;
            Vector3 spotToCheck = transform.position + Vector3.up * distToCheckAhead + Vector3.up;

            //To get the note that the player will collide with immediately after spinning, we need to get the distance to the farthest one.
            Transform collisionNote = null;
            float farthestDist = 0;
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(spotToCheck, spinDetectionRadius, layersToCheck))
            {
                if (Vector3.Distance(spotToCheck, collider.transform.position) > farthestDist)
                {
                    collisionNote = collider.transform;
                }
            }
            transform.Rotate(0, 0, rotationalVelocity * Time.deltaTime * animProgress);

            

            if (collisionNote != null)
            {
                SpriteRenderer testRenderer = collisionNote.GetComponent<SpriteRenderer>();

                if (testRenderer)
                {
                    //testRenderer.color = new UnityEngine.Color(0, 1, 0);
                }

                float targetRotation = Mathf.Atan2(collisionNote.position.y - spotToCheck.y, collisionNote.position.x - spotToCheck.x);

                if (collisionNote.name.Contains("Blue"))
                {
                    targetRotation += (Mathf.PI / 180 * 30);
                }


                
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, targetRotation * 180 / Mathf.PI), transform.rotation, Mathf.Pow(animProgress, .1f));


            }
            



            //This is the original rotation code.
            //transform.Rotate(0, 0, rotationalVelocity * Time.deltaTime);




            if (spinTimer <= 0)
            {

                previousMousePosition = Input.mousePosition;
                isSpinning = false;
            }





            return;
        }
        else
        {

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
                Debug.Log("Disc set to spin!");

                isSpinning = true;
                isSpinningLeft = rotationAmount < 0;
                isSpinningRight = rotationAmount > 0;

                spinTimer = timeToStopSpinning;

            }

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
        //if (collision.CompareTag("Spin"))
        //{
        //    isSpinning = false;
        //    isSpinningLeft = false;
        //    isSpinningRight = false;
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spinDetectionRadius);
    }
}
