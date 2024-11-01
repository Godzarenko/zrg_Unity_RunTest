using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    static Pooler _instance;
    public static Pooler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<Pooler>();
            }
            return _instance;
        }
    }


    //static Dictionary<PoolObject, PoolerPool> Pools = new Dictionary<PoolObject, PoolerPool>();
    //static Dictionary<GameObject, PoolerPool> ObjectPools = new Dictionary<GameObject, PoolerPool>();

    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    //static void StaticReinit()
    //{
    //    Pools = new Dictionary<PoolObject, PoolerPool>();
    //    ObjectPools = new Dictionary<GameObject, PoolerPool>();
    //}

    Dictionary<PoolObject, PoolerPool> Pools = new Dictionary<PoolObject, PoolerPool>();
    Dictionary<GameObject, PoolerPool> ObjectPools = new Dictionary<GameObject, PoolerPool>();

    public static PoolObject GetObject(PoolObject poolPrefab)
    {
        if (Instance == null)
        {
            Debug.LogError("Pool not set up");
            return null;
        }
        if (!Instance.Pools.ContainsKey(poolPrefab))
        {
            Instance.Pools.Add(poolPrefab, new PoolerPool());
        }
        PoolerPool P = Instance.Pools[poolPrefab];
        if (P.DisabledObject.Count == 0)
        {
            //create new
            PoolObject NGO = Instantiate(poolPrefab);
            P.AddNewObject(NGO);
            return NGO;
        }
        else
        {
            PoolObject PO = P.DisabledObject[0];
            P.DisabledObject.RemoveAt(0);
            PO.gameObject.SetActive(true);
            return PO;
        }
    }
    public static PoolObject GetObject(GameObject prefab)
    {
        if (Instance == null)
        {
            Debug.LogError("Pool not set up");
            return null;
        }
        if(prefab.TryGetComponent<PoolObject>(out PoolObject PO))
        {
            //it has pool object redirect to pooled
            return GetObject(PO);
        }
        //it has no pool object, create object and add pool object
        if (!Instance.ObjectPools.ContainsKey(prefab))
        {
            Instance.ObjectPools.Add(prefab, new PoolerPool());
        }
        PoolerPool P = Instance.ObjectPools[prefab];
        if (P.DisabledObject.Count == 0)
        {
            //create new
            GameObject NGO = Instantiate(prefab);
            PoolObject nPO = NGO.AddComponent<PoolObject>();
            P.AddNewObject(nPO);
            return nPO;
        }
        else
        {
            PoolObject nPO = P.DisabledObject[0];
            P.DisabledObject.RemoveAt(0);
            nPO.gameObject.SetActive(true);
            return nPO;
        }
    }

    public class PoolerPool
    {
        public List<PoolObject> AllObject = new List<PoolObject>();
        public List<PoolObject> ActiveObject = new List<PoolObject>();
        public List<PoolObject> DisabledObject = new List<PoolObject>();

        public void FlushPool()
        {
            for (int i = 0; i < AllObject.Count; i++)
            {
                if (AllObject[i] != null)
                {
                    Destroy(AllObject[i].gameObject);
                }
            }
            AllObject.Clear();
            ActiveObject.Clear();
            DisabledObject.Clear();
        }
        public void AddNewObject(PoolObject PO)
        {
            PO.MyPool = this;
            AllObject.Add(PO);
            ActiveObject.Add(PO);
        }
        public void ObjectDisabled(PoolObject PO)
        {
            int activeIndex = ActiveObject.IndexOf(PO);
            if (activeIndex != -1)
            {
                ActiveObject.RemoveAt(activeIndex);
                DisabledObject.Add(PO);
            }
        }
        public void ObjectDestroyed(PoolObject PO)
        {
            AllObject.Remove(PO);
            int activeIndex = ActiveObject.IndexOf(PO);
            if (activeIndex != -1)
            {
                ActiveObject.RemoveAt(activeIndex);
            }
            else
            {
                int disabledIndex = DisabledObject.IndexOf(PO);
                if(disabledIndex != -1) {
                    DisabledObject.RemoveAt(disabledIndex);
                }
            }
        }
    }
}
