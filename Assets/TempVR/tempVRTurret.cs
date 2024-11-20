using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempVRTurret : MonoBehaviour
{
    [SerializeField] Transform _shootingPoint;
    [SerializeField] GameObject _ball;
    float _shootingFrequency = 0.2f;
    float _currTime = 0f;

    private void Update()
    {
        _currTime += Time.deltaTime;
        if (_currTime >= _shootingFrequency) {
            _currTime = 0;
            shootBall();
        }
    }

    public void shootBall() {
        Instantiate(_ball, _shootingPoint.position, _shootingPoint.rotation);
    }
}
