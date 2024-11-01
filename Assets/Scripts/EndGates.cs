using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGates : MonoBehaviour
{
    [SerializeField] int LevelToOpen;
    public float Multiplier;
    [SerializeField] Animator Anmtr;
    bool triggered = false;
    [SerializeField] string TriggerSound;
    [SerializeField] string WinSound;
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            if (other.CompareTag("Player"))
            {
                triggered = true;
                int plrLevel = PlayerController.Instance.CurrentLevel;
                if (plrLevel >= LevelToOpen)
                {
                    Anmtr.SetBool("open", true);
                    LevelManager.Instance.SetCurrentMultiplier(Multiplier);
                    SoundsManager.CreateSoundOnCamera(TriggerSound, 4, true);
                }
                else
                {
                    //end level
                    LevelManager.Instance.SignalGameEnd(true, plrLevel);
                    SoundsManager.CreateSoundOnCamera(WinSound, 10, true);
                }
            }
        }
    }
}
