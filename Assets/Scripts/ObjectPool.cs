using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Helper class for object pool design pattern to increase performance
public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject poolObjectPrefab;
    [SerializeField] private int poolCount = 30000;

    private readonly List<GameObject> pool = new List<GameObject>();
    private int poolIndex;

    private void Awake()
    {
        InitPool();
    }

    private void InitPool()
    {
        for (int i = 0; i < poolCount; i++)
        {
            var obj = GetNewObject;
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetAvailableObject
    {
        get
        {
            GameObject obj;
            for (int i = 0; i < pool.Count; i++)
            {
                obj = pool[poolIndex];
                poolIndex = (poolIndex + 1) % pool.Count;

                if (obj.gameObject.activeSelf == false)
                    return obj;
            }

            obj = GetNewObject;
            pool.Add(obj);

            return obj;

        }
    }

    private GameObject GetNewObject
    {
        get
        {
            var obj = Instantiate(poolObjectPrefab);
            obj.transform.SetParent(transform);
            return obj;
        }
    }

    public void DespawnAllObjects()
    {
        pool.ForEach(x => x.gameObject.SetActive(false));
    }
}
