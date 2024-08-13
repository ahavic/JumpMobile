using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericObjectPool<T> : MonoBehaviour where T : IPoolObject
{
    [Serializable]
    public class PoolInfo
    {
        public int poolSize;
        public GameObject prefab;
    }

    [SerializeField] PoolInfo[] infoPools = null;
    Dictionary<Type, GameObject> prefabDict = null;
    Dictionary<Type, LinkedList<T>> objectPools = null;

    protected virtual void Awake()
    {
        objectPools = new Dictionary<Type, LinkedList<T>>();
        prefabDict = new Dictionary<Type, GameObject>();
        
        if(infoPools != null || infoPools.Length > 0)
        {
            for(int i = 0; i < infoPools.Length; i++)
            {
                T info = (T)infoPools[i].prefab.GetComponent<IPoolObject>();
                if (info == null || objectPools.ContainsKey(info.GetType()))
                    continue;

                objectPools.Add(info.GetType(), new LinkedList<T>());
                prefabDict.Add(info.GetType(), infoPools[i].prefab);

                for(int j = 0; j < infoPools[i].poolSize; j++)
                {
                    IncreasePool(info.GetType());
                }
            }
        }

        DisablePooledObjects();
    }

    public IPoolObject GetFromPool(Type key)
    {
        if (!objectPools.ContainsKey(key))
            return null;        

        if(objectPools[key].First.Value.GetGameObject().activeSelf)        
            IncreasePool(key);

        IPoolObject pooledObj = objectPools[key].First.Value;
        objectPools[key].RemoveFirst();
        objectPools[key].AddLast((T)pooledObj);

        return pooledObj;
    }

    void IncreasePool(Type key)
    {
        if (!objectPools.ContainsKey(key))
            return;

        GameObject go = Instantiate(prefabDict[key]);
        objectPools[key].AddFirst(go.GetComponent<T>());
    }

    /// <summary>
    /// Disable pooled objects
    /// </summary>
    /// <param name="key">if no key for pool, disable all objects in all pools</param>
    public void DisablePooledObjects(Type key = null)
    {
        if (key != null && objectPools.ContainsKey(key))
        {
            LinkedListNode<T> pooledObjNode = objectPools[key].First;
            while (pooledObjNode != null)
            {
                pooledObjNode.Value.GetGameObject().SetActive(false);
                pooledObjNode = pooledObjNode.Next;
            }
        }
        else
        {
            foreach (var kvp in objectPools)
            {
                LinkedListNode<T> pooledObjNode = objectPools[kvp.Key].First;
                while (pooledObjNode != null)
                {
                    pooledObjNode.Value.GetGameObject().SetActive(false);
                    pooledObjNode = pooledObjNode.Next;
                }
            }
        }
    }
}
