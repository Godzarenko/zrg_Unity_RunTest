using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] int AddXP;
    [SerializeField] GameObject PickupFXPrefab;
    //public GameObject PickupFloatText;

    [SerializeField] string PickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.Instance.AddXP(AddXP);
            PoolObject FXPO = Pooler.GetObject(PickupFXPrefab);
            FXPO.transform.SetParent(PlayerController.Instance.PlayerCaret);
            FXPO.transform.localPosition = PickupFXPrefab.transform.localPosition; //get position from prefab
            gameObject.SetActive(false);
            MainCanvas.Instance.ShowAddedXP(AddXP);
            SoundsManager.CreateSoundOnCamera(PickupSound, 2, true);
        }
    }
}
