using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Timer : MonoBehaviour
{
    [SerializeField] float StartTime = 5;
    float _time;
    public UnityEngine.Events.UnityEvent OnTimerElapsed;

    [SerializeField] UnityEngine.UI.Image FillImage;

    private void OnEnable()
    {
        _time = StartTime;
    }

    private void Update()
    {
        if (_time >= 0)
        {
            FillImage.fillAmount = _time / StartTime;
            _time -= Time.deltaTime;
            if (_time < 0)
            {
                OnTimerElapsed?.Invoke();
            }
        }
    }
}
