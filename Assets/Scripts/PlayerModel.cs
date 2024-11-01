using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] PlayerModelLevelStruct[] Levels;
    bool gameOngoing = false;
    int lastLevel = 0;
    private void Awake()
    {
        LevelManager.Instance.OnLevelChanged.AddListener(LevelChanged);
        LevelManager.Instance.OnGameEnded.AddListener(GameEnded);
        LevelManager.Instance.OnGameContinue.AddListener(GameContinued);
        LevelManager.Instance.OnGameStarted.AddListener(GameStarted);
    }
    private void Start()
    {
        for(int i = 0; i < Levels.Length; i++)
        {
            Levels[i].Anmtr.keepAnimatorStateOnDisable = true;
        }
    }
    void GameStarted()
    {
        gameOngoing = true;
        Levels[lastLevel].Anmtr.SetBool("walk", true);
    }
    void GameEnded(bool win, int winlevel)
    {
        gameOngoing = false;
        if (win)
        {
            Levels[lastLevel].Anmtr.SetBool("win", true);
        }
        else
        {
            Levels[lastLevel].Anmtr.SetBool("fail", true);
        }
    }
    void GameContinued()
    {
        gameOngoing = true;
        Levels[lastLevel].Anmtr.SetBool("walk", true);
        Levels[lastLevel].Anmtr.SetBool("win", false);
        Levels[lastLevel].Anmtr.SetBool("fail", false);
    }
    void LevelChanged(int lvl)
    {
        
        for (int i = 0; i < Levels.Length; i++)
        {
            PlayerModelLevelStruct PM = Levels[i];
            if (i != lvl)
            {
                PM.GObject.SetActive(false);
                PM.Anmtr.ResetTrigger("spin"); //make sure consecutive changes wont break animation later
            }
            else
            {
                PM.GObject.SetActive(true);
                if (gameOngoing)
                {
                    //Play FX? FX play on enable inside model?
                    PM.Anmtr.SetTrigger("spin");
                    PM.Anmtr.SetBool("walk", true);
                }
                lastLevel = lvl;
            }
        }
    }



    [System.Serializable]
    public struct PlayerModelLevelStruct
    {
        public GameObject GObject;
        public Animator Anmtr;
    }
}
