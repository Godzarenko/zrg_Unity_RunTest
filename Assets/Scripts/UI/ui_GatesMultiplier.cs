using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_GatesMultiplier : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI multText;

    void Start()
    {
        EndGates EG = GetComponentInChildren<EndGates>();
        if (EG != null)
        {
            multText.text = string.Format("X{0,1:#.##}",EG.Multiplier);
        }
    }
}
