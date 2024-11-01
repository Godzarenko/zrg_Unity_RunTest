using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotor : MonoBehaviour
{
    public Vector3 Axis;
    public float Speed;
    public bool Local;

    void Update()
    {
        if (Local)
        {
            transform.Rotate(Axis, Speed * Time.deltaTime, Space.Self);
        }
        else
        {
            transform.Rotate(Axis, Speed * Time.deltaTime, Space.World);
        }
    }
}
