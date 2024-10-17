using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DrawColliderBoundingBox : MonoBehaviour
{
    private BoxCollider boxCollider;
    void OnDrawGizmos()
    {
        boxCollider = GetComponent<BoxCollider>();
        Vector3 size = boxCollider.size;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
