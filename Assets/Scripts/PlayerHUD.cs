using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] PlayerHUDLevelStruct[] Levels;
    [SerializeField] Animator HUDAnmtr;
    [SerializeField] UnityEngine.UI.Image MainSlider; //use image fill instead Slider
    float currentPercent;
    [SerializeField] bool SmoothSlider = true;
    [SerializeField] float SliderSpeed = 1f;
    private void Awake()
    {
        LevelManager.Instance.OnLevelChanged.AddListener(OnLeveChanged);
        LevelManager.Instance.OnXPChanged.AddListener(OnXPChanged);
        LevelManager.Instance.OnGameStarted.AddListener(GameStarted);
        LevelManager.Instance.OnGameEnded.AddListener(GameEnded);
    }
    private void Update()
    {
        if (SmoothSlider)
        {
            if (MainSlider.fillAmount != currentPercent)
            {
                MainSlider.fillAmount = Mathf.MoveTowards(MainSlider.fillAmount, currentPercent, SliderSpeed * Time.deltaTime);
            }
        }
    }
    public void OnXPChanged(float percent)
    {
        currentPercent = percent;
        if (!SmoothSlider)
        {
            MainSlider.fillAmount = currentPercent;
        }
    }

    public void OnLeveChanged(int lvl)
    {
        for(int i = 0; i < Levels.Length; i++)
        {
            if (i != lvl)
            {
                Levels[i].SliderDecorObject.SetActive(false);
            }
            else
            {
                Levels[i].SliderDecorObject.SetActive(true);
                MainSlider.color = Levels[i].SliderColor;
                HUDAnmtr.SetInteger("state", Levels[i].AnimatorState);
            }
        }
    }

    void GameStarted()
    {
        gameObject.SetActive(true);
        currentPercent = PlayerController.Instance.XPPercent;
        MainSlider.fillAmount = currentPercent;
    }
    private void OnEnable()
    {
        currentPercent = PlayerController.Instance.XPPercent;
        MainSlider.fillAmount = currentPercent;
    }
    void GameEnded(bool win, int winlevel)
    {
        gameObject.SetActive(false);
    }
    [Serializable]
    public struct PlayerHUDLevelStruct
    {
        public Color SliderColor;
        public int AnimatorState;
        public GameObject SliderDecorObject;
    }
}
