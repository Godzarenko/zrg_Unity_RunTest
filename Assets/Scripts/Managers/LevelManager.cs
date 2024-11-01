using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    static LevelManager _instance;
    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<LevelManager>();
            }
            return _instance;
        }
    }
    
    [SerializeField] string WinScreenSound;
    [SerializeField] string FailSound;
    [SerializeField] string StartSound;


    public bool IsGameOngoing()
    {
        return gameOngoing;
    }
    bool gameOngoing;
    public void StartGame()
    {
        if (!gameOngoing)
        {
            PlayerController.Instance.StartGame();
            OnGameStarted?.Invoke();
            gameOngoing = true;
            SoundsManager.CreateSoundOnCamera(StartSound, 4, true);
        }
    }

    public void SignalGameEnd(bool win, int winLevel)
    {
        if (gameOngoing)
        {
            gameOngoing = false;
            OnGameEnded?.Invoke(win, winLevel);
            if (win)
            {
                SoundsManager.CreateSoundOnCamera(WinScreenSound, 4, true);
            }
            else
            {
                SoundsManager.CreateSoundOnCamera(FailSound, 4, true);
            }
        }
    }

    public void RestartGame()
    {
        OnGameRestart?.Invoke();
        ButchersGames.LevelManager.Default.RestartLevel();
    }
    public void NextLevel()
    {
        OnGameRestart?.Invoke();
        ButchersGames.LevelManager.Default.NextLevel();
    }
    public void ContinueGame()
    {
        SoundsManager.CreateSoundOnCamera(StartSound, 4, true);
        gameOngoing = true;
        OnGameContinue?.Invoke();
    }

    public UnityEngine.Events.UnityEvent<bool, int> OnGameEnded;
    public UnityEngine.Events.UnityEvent OnGameStarted;
    public UnityEngine.Events.UnityEvent OnGameContinue;
    public UnityEngine.Events.UnityEvent OnGameRestart;

    public UnityEngine.Events.UnityEvent<int> OnLevelChanged;
    public UnityEngine.Events.UnityEvent<float> OnXPChanged;

    public void PlayerLevelChanged(int newLevel)
    {
        OnLevelChanged?.Invoke(newLevel);
    }
    public void PlayerXPChanged(float newPercent)
    {
        OnXPChanged?.Invoke(newPercent);
    }

    public void SetCurrentMultiplier(float mult)
    {
        currentMultiplier = mult;
    }
    float currentMultiplier = 1;
    public int GetCurrentReward()
    {
        return Mathf.FloorToInt(PlayerController.Instance.CurrentXP * currentMultiplier);
    }
}
