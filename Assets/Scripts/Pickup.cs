using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int AddXP;
    public GameObject PickupFXPrefab;
    //public GameObject PickupFloatText;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.Instance.AddXP(AddXP);
            PoolObject FXPO = Pooler.GetObject(PickupFXPrefab);
            FXPO.transform.SetParent(PlayerController.Instance.PlayerCaret);
            FXPO.transform.localPosition = PickupFXPrefab.transform.localPosition; //get position from prefab
            //PoolObject TXPO = Pooler.GetObject(PickupFloatText);
            //TXPO.transform.position = transform.position;
            //if(TXPO.TryGetComponent<FloatText>(out FloatText FTX))
            //{
            //    FTX.text.text = Mathf.FloorToInt( AddXP ).ToString();
            //}
            gameObject.SetActive(false);
            MainCanvas.Instance.ShowAddedXP(AddXP);
        }
    }
}
