using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SphereSize : MonoBehaviour
{
    [SerializeField] private Vector3 _center;
    [SerializeField] private float _radius;

    public float RawRadius => _radius;
    public float Radius => transform.localScale.z * _radius;
    public Vector3 Center => transform.position + _center;
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Center, Radius);
    }

    public bool InsideSphere(Vector3 position)
    {
        float x = Mathf.Pow(position.x - _center.x, 2);
        float z = Mathf.Pow(position.z - _center.z, 2);
        return x + z <= Mathf.Pow(Radius, 2);

    }
}
