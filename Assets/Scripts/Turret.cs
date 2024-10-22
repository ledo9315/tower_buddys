using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    private float _fireCountdown = 0f;
    
    [Header("Attributes")]
    
    [SerializeField] private float range = 15f;
    [SerializeField] private float fireRate = 1f;
    
    [Header("Unity Setup Fields")]
    
    public string enemyTag = "Enemy";
    [SerializeField] private Transform partToRotate;
    [SerializeField] private float turnSpeed = 10f;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    private void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (var enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
        
    }

    private void Update()
    {
        if (!target) return;

        Vector3 dir = target.position - transform.position;
        dir.y = 0;  // Höhe ignorieren, damit der Turm sich nur horizontal dreht

        // Berechne die Rotation in Richtung des Ziels
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        // Wende einen Rotations-Offset an, falls nötig
        Quaternion rotationWithOffset = lookRotation * Quaternion.Euler(0f, 90f, 0f);  // Passe 90f je nach Ausrichtung an

        // Setze die Rotation nur auf der Y-Achse
        partToRotate.rotation = Quaternion.Slerp(partToRotate.rotation, rotationWithOffset, Time.deltaTime * turnSpeed);

        if (_fireCountdown <= 0f)
        {
            Shoot();
            _fireCountdown = 1f / fireRate;
        }

        _fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject bulletGo = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGo.GetComponent<Bullet>();

        if (bullet)
        {
            bullet.Seek(target);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
