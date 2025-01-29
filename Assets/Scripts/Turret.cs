using System;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    private float _fireCountdown = 0f;
    
    [Header("Attributes")]
    [SerializeField] private float range = 15f;
    [SerializeField] private int fireDamage = 40;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float fieldOfView = 30f; // Sichtfeld (Grad)
    
    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";

    [SerializeField] private Transform GeneralTurretRotation;
    [SerializeField] private Transform ActiveTurretRotation;
    [SerializeField] private float turnSpeed = 10f;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] turretModel;
    
    // LineRenderer für den Bereich
    private LineRenderer lineRenderer;
    private bool isDamage = true;
    public int turretType = 0;
    public int[] upgradePrice = {50, 50, 50};

    private void Start()
    {
        // Erstelle und konfiguriere den LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.loop = true; // Kreis schließen

        InvokeRepeating(nameof(UpdateTarget), 0f, 0.1f);

        // Zeichne den Sichtbereich
        DrawFieldOfView();
    }

    private void Update()
    {
        if (lineRenderer != null)
        {
            DrawFieldOfView();
        }

        if (!target) return;

        Vector3 dir = target.position - transform.position;
        dir.y = 0; // Höhe ignorieren, damit der Turm sich nur horizontal dreht

        Quaternion lookRotation = Quaternion.LookRotation(dir);
        ActiveTurretRotation.rotation = Quaternion.Slerp(
            ActiveTurretRotation.rotation, 
            lookRotation, 
            Time.deltaTime * turnSpeed
        );

        if (_fireCountdown <= 0f)
        {
            switch (turretType)
            {
                case 0:
                    Shoot();
                    _fireCountdown = 1f / fireRate;
                    break;
                case 2:
                    Shoot();
                    _fireCountdown = 1f / fireRate;
                    break;
            }
        }
        _fireCountdown -= Time.deltaTime;
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (var enemy in enemies)
        {
            if (turretType != 1)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy > range) continue;

                Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
                Vector3 forwardDirection = GeneralTurretRotation.forward;
                float angleToEnemy = Vector3.Angle(forwardDirection, directionToEnemy);

                if (angleToEnemy <= fieldOfView && distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
            else
            {
                enemy.GetComponent<Enemy>().isHit(fireDamage);
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Shoot()
    {
        GameObject bulletGo = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGo.GetComponent<Bullet>();

        if (bullet)
        {
            bullet.Seek(target, fireDamage, isDamage);
        }
    }

    private void DrawFieldOfView()
    {
        // Berechne die Punkte des Sichtkegels
        int segments = 50; // Anzahl der Liniensegmente
        float angleStep = fieldOfView * 2 / segments;
        Vector3[] points = new Vector3[segments + 2]; // Zusätzlicher Punkt für die Mitte + letzter Punkt für den Abschluss

        points[0] = GeneralTurretRotation.position; // Das Zentrum des Sichtkegels basiert auf GeneralTurretRotation

        for (int i = 0; i <= segments; i++)
        {
            // Berechne den lokalen Winkel
            float angle = -fieldOfView + i * angleStep;
            float rad = Mathf.Deg2Rad * angle;

            // Erstelle eine lokale Richtung basierend auf dem Winkel
            Vector3 localDirection = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));

            // Transformiere die lokale Richtung basierend auf der Rotation von GeneralTurretRotation
            Vector3 worldDirection = GeneralTurretRotation.TransformDirection(localDirection);

            // Berechne die Endposition des Liniensegments
            points[i + 1] = GeneralTurretRotation.position + worldDirection * range;
        }

        // Setze die Punkte für den LineRenderer
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }


    public bool canUpgrade()
    {
        return (Shop.currentSelectedUpgrade % 3) switch
        {
            0 => upgradePrice[0] < PlayerStats.Money,
            1 => upgradePrice[1] < PlayerStats.Money,
            2 => upgradePrice[2] < PlayerStats.Money,
            _ => false
        };
    }

    public void tryUpgrade()
    {
        switch (Shop.currentSelectedUpgrade % 3)
        {
            case 0:
                PlayerStats.Money -= upgradePrice[0];
                upgradePrice[0] = Convert.ToInt32(upgradePrice[0] * 1.15);
                fireDamage += 20;
                break;
            case 1:
                PlayerStats.Money -= upgradePrice[1];
                upgradePrice[1] = Convert.ToInt32(upgradePrice[1] * 1.2);
                fieldOfView += 10f;
                break;
            case 2:
                PlayerStats.Money -= upgradePrice[2];
                upgradePrice[2] = Convert.ToInt32(upgradePrice[2] * 1.15);
                if (turretType == 1)
                {
                    range += 1;
                }
                else
                {
                    fireRate += 1f;
                }
                break;
        }
    }

    public void setTurretType(int newTurretType)
    {
        turretType = newTurretType;
        turretModel[turretType].SetActive(true);
        isDamage = turretType != 2;
        if (turretType == 1)
        {
            fireDamage = 2;
            range = 8f;
        }
        
    }
}
