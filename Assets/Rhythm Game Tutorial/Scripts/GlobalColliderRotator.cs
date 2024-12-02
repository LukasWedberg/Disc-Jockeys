using UnityEngine;

public class GlobalColliderRotator : MonoBehaviour
{
    private static GlobalColliderRotator instance;
    public static GlobalColliderRotator Instance => instance;

    void Awake()
    {
        if (instance == null) instance = this;
        RotateAllColliders();
    }

    public void RotateAllColliders()
    {
        BoxCollider2D[] allColliders = FindObjectsOfType<BoxCollider2D>();
        Quaternion rotation = transform.rotation;
        
        foreach (BoxCollider2D collider in allColliders)
        {
            // Store original collider properties
            Vector2 originalSize = collider.size;
            Vector2 originalOffset = collider.offset;
            
            // Create a new GameObject as parent
            GameObject holder = new GameObject("ColliderHolder");
            holder.transform.position = collider.transform.position;
            holder.transform.rotation = rotation;
            
            // Parent the collider to the holder
            collider.transform.SetParent(holder.transform, true);
            
            // Adjust collider properties to maintain original coverage
            float angle = Mathf.Abs(rotation.eulerAngles.z * Mathf.Deg2Rad);
            collider.size = new Vector2(
                originalSize.x * Mathf.Cos(angle) + originalSize.y * Mathf.Sin(angle),
                originalSize.y * Mathf.Cos(angle) + originalSize.x * Mathf.Sin(angle)
            );
            collider.offset = rotation * originalOffset;
        }
    }
}