using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnDelay : MonoBehaviour
{
    public float StartDelay = 2;
    float _dly = 2;

    private void Update()
    {
        _dly-=Time.deltaTime;
        if (_dly <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        _dly = StartDelay;
    }
}
