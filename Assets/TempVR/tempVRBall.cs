using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempVRBall : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float Force = 75f;
    [SerializeField] GameObject _self;
    Rigidbody rb;
    float _deathTimer = 3f;
    float _currTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * Force, ForceMode.Impulse);
    }

    private void Update()
    {
        _currTime += Time.deltaTime;
        if (_currTime >= _deathTimer)
        {
            Destroy(_self);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) {
            Debug.Log("Wurde aufgerufen!");
            Destroy(collision.gameObject);
        }
    }
}
