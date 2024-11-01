using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_StepsSounds : MonoBehaviour
{
    [SerializeField] string StepSound;
    public void step()
    {
        SoundsManager.CreateSoundOnCamera(StepSound, 2, true);
    }
}
