using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevelOnStart : MonoBehaviour
{
    private void Start()
    {
        ButchersGames.LevelManager.Default.SelectLevel(0);
    }
    
}
