using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    public float beatTempo;
    public float speed = 1f; // Speed multiplier, set in Inspector

    public bool hasStarted;

    private float scrollSpeed;

    // Start is called before the first frame update
    void Start()
    {
        scrollSpeed = beatTempo / 60f * speed;
        transform.position = new Vector3(0f, transform.position.y * speed, 0f);

        // Adjust the Y position of all child objects based on the speed
        if (speed > 1f) // Only modify positions when speed multiplier is greater than 1
        {
            foreach (Transform child in transform)
            {
                Vector3 childPosition = child.localPosition;
                childPosition.y *= speed;
                child.localPosition = childPosition;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted)
        {
            transform.position -= new Vector3(0f, scrollSpeed * Time.deltaTime, 0f);
        }
    }
}