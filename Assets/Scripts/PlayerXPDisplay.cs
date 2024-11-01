using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerXPDisplay : MonoBehaviour
{
    [SerializeField] Animator PositiveAnimator;
    [SerializeField] Animator NegativeAnimator;

    float cache_Positive = 0;
    int trg_Positive = 0;
    float cache_Negative = 0;
    int trg_Negative = 0;

    [SerializeField] TMPro.TextMeshProUGUI PositiveText;
    [SerializeField] TMPro.TextMeshProUGUI NegativeText;

    float time_Positive = 99;
    float time_Negative = 99;

    [SerializeField] float CacheTime = 3;
    private void Update()
    {
        if (time_Positive < CacheTime)
        {
            cache_Positive = Mathf.MoveTowards(cache_Positive, trg_Positive, 20 * Time.deltaTime);
            PositiveText.text = "+" + Mathf.FloorToInt(cache_Positive).ToString();
            time_Positive += Time.deltaTime;
        }
        if (time_Negative < CacheTime)
        {
            cache_Negative = Mathf.MoveTowards(cache_Negative, trg_Negative, 20 * Time.deltaTime);
            NegativeText.text = Mathf.FloorToInt(cache_Negative).ToString();
            time_Negative += Time.deltaTime;
        }
    }
    private void OnDisable()
    {
        cache_Positive = 0;
        trg_Positive = 0;
        time_Positive = 99;
        cache_Negative = 0;
        trg_Negative = 0;
        time_Negative = 99;
        PositiveAnimator.ResetTrigger("pulse");
        NegativeAnimator.ResetTrigger("pulse");
    }
    public void DisplayXP(int xp)
    {
        if (xp > 0)
        {
            if(time_Positive > CacheTime)
            {
                cache_Positive = 0;
                trg_Positive = 0;
            }
            time_Positive = 0;
            trg_Positive += xp;
            PositiveAnimator.SetTrigger("pulse");
        }
        else
        {
            if (time_Negative > CacheTime)
            {
                cache_Negative = 0;
                trg_Negative = 0;
            }
            time_Negative = 0;
            trg_Negative += xp;
            NegativeAnimator.SetTrigger("pulse");
        }
    }
}
