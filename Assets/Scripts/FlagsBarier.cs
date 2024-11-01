using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagsBarier : MonoBehaviour
{
    bool triggered;
    [SerializeField] Animator Anmtr;
    [SerializeField] string TriggerSound;
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            if (other.CompareTag("Player"))
            {
                triggered = true;
                Anmtr.SetBool("triggered", true);
                SoundsManager.CreateSoundOnCamera(TriggerSound, 2, true);
            }
        }
    }
}
