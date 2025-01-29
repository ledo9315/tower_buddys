using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class tempVRGrabbable : MonoBehaviour
{
    [SerializeField] Transform _rotPoint;
    [SerializeField] Transform _selfTransform;
    [SerializeField] float _radius;
    // Update is called once per frame
    void Update()
    {
        //Code von https://discussions.unity.com/t/keeping-distance-between-two-gameobjects/47792
        
        _selfTransform.position = (_selfTransform.position - _rotPoint.position).normalized * _radius + _rotPoint.position;
    }

    public void wasGrabbed(HoverEnterEventArgs arg) {
        Debug.Log("I was grabbed!");    
    }
}
