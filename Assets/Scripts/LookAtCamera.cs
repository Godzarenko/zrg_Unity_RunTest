using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public bool UseCameraForward;
    public bool Flat;
    public bool Inverted;
    public Transform Cam;
    void Update()
    {
        if (Cam == null)
        {
            Cam = Camera.main.transform;
        }
        Vector3 Dir = Vector3.zero;
        if (UseCameraForward)
        {
            Dir = Cam.forward;
        }
        else
        {
            Dir = Cam.transform.position - transform.position;
        }
        if (Flat)
        {
            Dir.y = 0;
        }
        transform.rotation = Quaternion.LookRotation(Inverted ? -Dir : Dir);
    }
}
