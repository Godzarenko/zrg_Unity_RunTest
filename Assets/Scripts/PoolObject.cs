using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent ObjectEnabled;

    public Pooler.PoolerPool MyPool;

    private void OnDisable()
    {
        MyPool.ObjectDisabled(this);
    }
    private void OnEnable()
    {
        ObjectEnabled?.Invoke();
    }
    private void OnDestroy()
    {
        MyPool.ObjectDestroyed(this);
    }

}
