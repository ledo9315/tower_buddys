using UnityEngine;

public class RaycastTeleporter : MonoBehaviour
{
    public float raycastDistance = 1000f; // The distance the raycast will travel
    public Transform raycastOrigin; // The origin from where the ray is cast (can be set in the inspector)

    void Update()
    {
        // If raycastOrigin is not assigned, use the object's position as the origin
        if (raycastOrigin == null)
        {
            raycastOrigin = transform;
        }

        // Create a ray from the origin in the forward direction
        Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        
        // Store the RaycastHit result
        RaycastHit hit;

        // Cast the ray and check if it hits any collider
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Check if the object has the "teleportable" tag
            if (hit.collider.CompareTag("Teleportable"))
            {
                Debug.Log("Hit the tower wuhu");
            }
        }

        // Draw the ray in the scene view (it will be red)
        Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * raycastDistance, Color.red);
    }
}