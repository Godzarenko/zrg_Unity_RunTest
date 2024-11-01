using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    static MainCanvas _instance;
    public static MainCanvas Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindFirstObjectByType<MainCanvas>();
            }
            return _instance;
        }
    }

    [SerializeField] List<GameObject> LevelUpText;

    [SerializeField] PlayerXPDisplay XPDisplay;

    [SerializeField] GameObject StartScreen;
    [SerializeField] GameObject GameScreen;
    [SerializeField] UI_WinScreen WinScreen;
    [SerializeField] UI_FailScreen FailScreen;

    bool gameOngoing;
    private void Awake()
    {
        LevelManager.Instance.OnLevelChanged.AddListener(LevelChanged);
        LevelManager.Instance.OnXPChanged.AddListener(XPChanged);
        LevelManager.Instance.OnGameStarted.AddListener(GameStarted);
        LevelManager.Instance.OnGameEnded.AddListener(GameEnded);

        LevelManager.Instance.OnGameRestart.AddListener(GameRestart);
        LevelManager.Instance.OnGameContinue.AddListener(GameContinue);
    }
    private void Update()
    {

    }
    void LevelChanged(int lvl)
    {
        if (!gameOngoing)
        {
            return;
        }
        for (int i = 0; i < LevelUpText.Count; i++)
        {
            if (i != lvl)
            {
                LevelUpText[i].SetActive(false);
            }
            else
            {
                LevelUpText[i].SetActive(true);
            }
        }
    }
    public void ShowAddedXP(int xp)
    {
        XPDisplay.DisplayXP(xp);
    }
    void XPChanged(float percent)
    {

    }
    public void StartGameClick()
    {
        LevelManager.Instance.StartGame();
    }
    void GameStarted()
    {
        gameOngoing = true;
        XPDisplay.gameObject.SetActive(true);
        StartScreen.SetActive(false);
        GameScreen.SetActive(true);
    }
    void GameEnded(bool win, int winlevel)
    {
        gameOngoing = false;
        XPDisplay.gameObject.SetActive(false);
        GameScreen.SetActive(false);
        if (!win)
        {
            FailScreen.gameObject.SetActive(true);
        }
        else
        {
            WinScreen.gameObject.SetActive(true);
        }
    }
    void GameContinue()
    {
        FailScreen.gameObject.SetActive(false);
        GameScreen.SetActive(true);
        XPDisplay.gameObject.SetActive(true);
        gameOngoing = true;
    }
    void GameRestart()
    {
        WinScreen.gameObject.SetActive(false);
        FailScreen.gameObject.SetActive(false);
        GameScreen.SetActive(false);
        XPDisplay.gameObject.SetActive(false);
        gameOngoing = false;
        StartScreen.SetActive(true);
    }
}
