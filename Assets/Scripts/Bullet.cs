using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private float speed = 70f;

    [SerializeField] private float explosionRadius = 0f;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private float timeOfImpactEffect = 2f;
    [SerializeField] private GameObject[] bullets;
    private int effectStrength = 40;
    private bool isDamage = true;
    public void Seek(Transform tar, int assignedEffectStrength, bool assignedIsDamage)
    {
        _target = tar;
        effectStrength = assignedEffectStrength;
        isDamage = assignedIsDamage;
        if (isDamage)
        {
            bullets[0].SetActive(true);
        }
        else
        {
            bullets[1].SetActive(true);
        }
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
        Debug.Log(isDamage);
        if (isDamage) {
            _target.GetComponent<Enemy>().isHit(effectStrength);
        }
        else
        {
            _target.GetComponent<Enemy>().isSlowed(effectStrength);
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
