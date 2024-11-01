using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerCurrentXP : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI XPText;
    void Awake()
    {
        LevelManager.Instance.OnXPChanged.AddListener( XPChanged);
        XPText.text = PlayerController.Instance.CurrentXP.ToString();
    }

    public void XPChanged(float percent)
    {
        XPText.text = PlayerController.Instance.CurrentXP.ToString();
    }
}
