using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FailScreen : MonoBehaviour
{
    [SerializeField] GameObject ADSection;
    [SerializeField] GameObject NoADSection;
    private void OnEnable()
    {
        //if(AdsManager.AdReady)
        ADSection.SetActive(true);
        NoADSection.SetActive(false);
        //else
        //NoADSection.SetActive(true);
    }

    public void ADClick()
    {
        //AdsManager.ShowRewarded("ad_reward_continue",AdSucess,AdFail);
        ADSuccess();
    }
    public void ADSuccess()
    {
        LevelManager.Instance.ContinueGame();
    }
    public void ADFail()
    {

    }
    public void RetryClick()
    {
        LevelManager.Instance.RestartGame();
    }
}
