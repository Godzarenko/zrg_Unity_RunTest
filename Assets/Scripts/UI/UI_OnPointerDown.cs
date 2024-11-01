using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_OnPointerDown : MonoBehaviour, IPointerDownHandler
{
    public UnityEngine.Events.UnityEvent PointerDown;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        PointerDown?.Invoke();
    }
}
