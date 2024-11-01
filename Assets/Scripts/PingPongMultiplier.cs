using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongMultiplier : MonoBehaviour
{
    [SerializeField] Vector2 MinMaxAngle;
    [SerializeField] Transform Needle;
    [SerializeField] float NeedleSpeed;
    bool ping;
    bool run = true;
    float needleAng;
    [SerializeField] MultiplierSection[] Sections;
    [SerializeField] float Debug_Multiplier;
    private void OnDrawGizmos()
    {
        needleAng = Needle.transform.localEulerAngles.z;
        if (needleAng > 180)
        {
            needleAng -= 360;
        }
        if (needleAng < -180)
        {
            needleAng += 360;
        }
        Debug_Multiplier = GetCurrentMultiplier();
    }
    private void Start()
    {
        needleAng = MinMaxAngle.x;
    }
    private void Update()
    {
        if (run)
        {
            if (ping)
            {
                Vector3 LocalEuler = Needle.transform.localEulerAngles;
                needleAng = Mathf.MoveTowards(needleAng, MinMaxAngle.x, NeedleSpeed * Time.deltaTime);
                if (Mathf.Abs(needleAng - MinMaxAngle.x) < 1)
                {
                    ping = false;
                }
                LocalEuler.z = needleAng;
                Needle.transform.localEulerAngles = LocalEuler;
            }
            else
            {
                Vector3 LocalEuler = Needle.transform.localEulerAngles;
                needleAng = Mathf.MoveTowards(needleAng, MinMaxAngle.y, NeedleSpeed * Time.deltaTime);
                if (Mathf.Abs(needleAng - MinMaxAngle.y) < 1)
                {
                    ping = true;
                }
                LocalEuler.z = needleAng;
                Needle.transform.localEulerAngles = LocalEuler;
            }
        }
    }
    public void Pause()
    {
        run = false;
    }
    public void Continue()
    {
        run = false;
    }
    public float GetCurrentMultiplier()
    {
        if (Sections == null || Sections.Length == 0)
        {
            return 1;
        }

        float AngFromMin = needleAng - MinMaxAngle.x;
        for (int i = 0; i < Sections.Length; i++)
        {
            if(AngFromMin > Sections[i].AngleFromMin)
            {
                continue;
            }
            else
            {
                return Sections[i].Multiplier;
            }
        }
        return Sections[0].Multiplier;
    }
    [System.Serializable]
    public struct MultiplierSection
    {
        public float AngleFromMin;
        public float Multiplier;
    }
}
