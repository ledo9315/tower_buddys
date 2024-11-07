using System.Collections;
using UnityEngine;

public class VRTurretController : MonoBehaviour
{
    public Transform turretBase; 
    public Transform firePoint; 
    public GameObject projectilePrefab; 
    public float rotationSpeed = 5f;
    public float projectileSpeed = 20f;

    void Update()
    {
        RotateTurret();
        FireProjectile();
    }


    void RotateTurret()
    {
        Vector3 direction = turretBase.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        turretBase.rotation = Quaternion.Slerp(turretBase.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }


    void FireProjectile()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * projectileSpeed;
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            Destroy(other.gameObject); 
        }
    }
}
