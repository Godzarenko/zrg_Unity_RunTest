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
    }
    private void Start()
    {
        
    }
    void GameStarted()
    {

    }
    void GameEnded(bool win, int winlevel)
    {
        if (win)
        {
            Levels[lastLevel].Anmtr.SetBool("win", true);
        }
        else
        {
            Levels[lastLevel].Anmtr.SetBool("fail", true);
        }
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
