using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    public static BeatScroller instance;
    
    public float beatTempo;
    public float speed = 1f;
    public bool hasStarted;

    private float scrollSpeed;
    private Transform objectTransform;
    private Vector3 moveDirection;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        objectTransform = transform;
        scrollSpeed = beatTempo / 60f * speed;
        
        // Calculate the direction along the rotated plane
        moveDirection = Quaternion.Euler(objectTransform.eulerAngles) * Vector3.down;
        
        Vector3 localPos = objectTransform.localPosition;
        objectTransform.localPosition = new Vector3(localPos.x, localPos.y * speed, localPos.z);

        if (speed > 1f)
        {
            foreach (Transform child in transform)
            {
                Vector3 childLocalPos = child.localPosition;
                childLocalPos.y *= speed;
                child.localPosition = childLocalPos;
            }
        }
    }

    void Update()
    {
        if (hasStarted)
        {
            objectTransform.position += moveDirection * scrollSpeed * Time.deltaTime;
        }
    }
}