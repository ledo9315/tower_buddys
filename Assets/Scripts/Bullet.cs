using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private float speed = 70f;
    [SerializeField] private float explosionRadius = 0f;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private float timeOfImpactEffect = 2f;

    public void Seek(Transform tar)
    {
        _target = tar;
    }

    private void Update()
    {
        if (!_target)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = _target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectIns, timeOfImpactEffect);
            Hit_target();
            return;
        }
        
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(_target);
    }

    void Hit_target()
    {

        if (explosionRadius > 0f)
        {
            Explode();
        } else
        {
            Damage(_target);
        }
        
        Destroy(gameObject);
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                Damage(col.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        Destroy(enemy.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
