using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class RaycastExample : MonoBehaviour
{
    // Raycast-Startposition (von diesem Punkt wird der Raycast abgestrahlt)
    public Transform raycastOrigin;
    
    // Raycast-Länge (wie weit der Raycast schießen soll)
    public float raycastDistance = 100f;

    // Farbe für die Darstellung des Raycasts
    public Color rayColor = Color.blue;

    // LineRenderer für die Visualisierung des Raycasts
    private LineRenderer lineRenderer;
    
    private bool isActive;
    private Vector3 hitLocation;
    
    private MeshRenderer TeleportRenderer;

    
    void Start()
    {
        // LineRenderer-Komponente hinzufügen, falls sie noch nicht existiert
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        
        // LineRenderer-Einstellungen
        lineRenderer.startWidth = 0.05f;  // Breite des Raycasts
        lineRenderer.endWidth = 0.05f;    // Breite des Raycasts am Ende
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Material für die Linie
        lineRenderer.startColor = rayColor;  // Startfarbe
        lineRenderer.endColor = rayColor;    // Endfarbe
    }
    
    void Update()
    {
        if (isActive)
        {
            raycastDistance = 100f;
            // Start des Raycasts
            Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        
            // Raycast-Informationen
            RaycastHit hit;

            // Zeichne den Raycast in der Szene in blauer Farbe
            Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * raycastDistance, rayColor);

            // Wenn der Raycast etwas trifft, gibt die Position des Treffpunkts aus
            // Überprüfe, ob der Raycast etwas trifft
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {

                    // Wenn der Raycast etwas trifft, visualisiere die Linie bis zum Trefferpunkt
                    lineRenderer.SetPosition(0, raycastOrigin.position);  // Startpunkt des Raycasts
                    lineRenderer.SetPosition(1, hit.point);               // Endpunkt des Raycasts (Treffpunkt)
                    if (hit.collider.CompareTag("Teleportable"))
                    {
                        if (TeleportRenderer != null && TeleportRenderer != hit.collider.gameObject.GetComponent<MeshRenderer>())
                        {
                            TeleportRenderer.enabled = false;
                        }

                        TeleportRenderer = hit.collider.gameObject.GetComponent<MeshRenderer>();
                        TeleportRenderer.enabled = true;
                        hitLocation = hit.collider.gameObject.transform.position;
                    }
                    else
                    {
                        if (TeleportRenderer != null)
                        {
                            TeleportRenderer.enabled = false;
                        }
                    }
            }
            else
            {
                // Wenn nichts getroffen wurde, zeichne den Ray bis zur maximalen Entfernung
                lineRenderer.SetPosition(0, raycastOrigin.position);
                lineRenderer.SetPosition(1, raycastOrigin.position + raycastOrigin.forward * raycastDistance);
            
                // Ausgabe, wenn kein Treffer erfolgt ist
                //Debug.Log("Kein Treffer.");
            }
        }
        else
        {
            lineRenderer.SetPosition(0, raycastOrigin.position);
            lineRenderer.SetPosition(1, raycastOrigin.position);        }
    }

    public void activateTeleporter()
    {
        isActive = true;
    }

    public void deactivateTeleporter()
    {
        isActive = false;
        if (TeleportRenderer != null)
        {
            TeleportRenderer.enabled = false;
        }
    }

    public Vector3 getRaycastHitLocation()
    {
        return hitLocation;
    }
}

