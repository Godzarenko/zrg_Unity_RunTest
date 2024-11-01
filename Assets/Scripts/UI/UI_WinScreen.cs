using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WinScreen : MonoBehaviour
{
    bool claimClicked = false;
    [SerializeField] GameObject RewardSection;
    [SerializeField] GameObject ADSection;
    [SerializeField] GameObject NoADSection;

    [SerializeField] GameObject GiftSection;

    [SerializeField] PingPongMultiplier PingPong;

    [SerializeField] List<TMPro.TextMeshProUGUI> NormalRewardTexts;
    [SerializeField] TMPro.TextMeshProUGUI MultipliedReward;
    private void OnEnable()
    {
        //if(AdsManager.AdReady)
        ADSection.SetActive(true);
        NoADSection.SetActive(false);
        //else
        //NoADSection.SetActive(true);

        RewardSection.SetActive(true);
        GiftSection.SetActive(false);

        for(int i = 0; i < NormalRewardTexts.Count; i++)
        {
            NormalRewardTexts[i].text = LevelManager.Instance.GetCurrentReward().ToString();
        }
    }
    private void Update()
    {
        if (ADSection.activeInHierarchy)
        {
            MultipliedReward.text = (LevelManager.Instance.GetCurrentReward() * PingPong.GetCurrentMultiplier()).ToString();
        }
    }
    public void ADClick()
    {
        if (claimClicked)
        {
            return;
        }
        PingPong.Pause();
        //AdsManager.ShowRewarded("ad_reward_claim",AdSucess,AdFail);
        ADSuccess();
    }
    float currentRewardMultiplier = 1;
    public void ADSuccess()
    {
        claimClicked = true;
        currentRewardMultiplier = (PingPong.GetCurrentMultiplier());
        StartCoroutine(SwitchScreenCor());
    }
    public void ADFail()
    {
        PingPong.Continue();
    }
    public void ClaimClick()
    {
        if (claimClicked)
        {
            return;
        }
        claimClicked = true;
        StartCoroutine(SwitchScreenCor());
        //give money
    }
    public void NextLevelClick()
    {
        LevelManager.Instance.NextLevel();
    }
    IEnumerator SwitchScreenCor()
    {
        yield return new WaitForSeconds(1.5f);
        RewardSection.SetActive(false);
        GiftSection.SetActive(true);
    }
}
