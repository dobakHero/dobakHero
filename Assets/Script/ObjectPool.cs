using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public GameObject[] poolPrefabs;

    public int poolingCount;

    Dictionary<object, List<GameObject>> _pooledObjects = new Dictionary<object, List<GameObject>>();
    Dictionary<string, int> _nameToIndex = new Dictionary<string, int>();

    private void Awake()
    {
        Instance = this;

        CreateMultiplePoolObjects();
    }

    private void CreateMultiplePoolObjects()
    {
        for (var i = 0; i < poolPrefabs.Length; i++)
        {
            for (var j = 0; j < poolingCount; j++)
            {
                CreatePoolObjects(i);
            }
        }
    }

    private void CreatePoolObjects(int idx)
    {
        if (!_pooledObjects.ContainsKey(poolPrefabs[idx].name))
        {
            List<GameObject> newList = new();
            _pooledObjects.Add(poolPrefabs[idx].name, newList);
            _nameToIndex.Add(poolPrefabs[idx].name, idx);
        }

        GameObject newDoll = Instantiate(poolPrefabs[idx], Instance.transform);
        newDoll.SetActive(false);
        _pooledObjects[poolPrefabs[idx].name].Add(newDoll);
    }


    // 오브젝트를 풀에서 가져옴
    public GameObject GetPooledObject(string target)
    {

        if (_pooledObjects.ContainsKey(target))
        {
            for (var i = 0; i < _pooledObjects[target].Count; i++)
            {
                if(!_pooledObjects[target][i].activeSelf)
                {
                    _pooledObjects[target][i].SetActive(true);
                    return _pooledObjects[target][i];
                }
            }
            

            // 용량이 꽉차 새로운 오브젝트를 생성할 필요가 생김
            var beforeCreateCount = _pooledObjects[target].Count;

            CreatePoolObjects(_nameToIndex[target]);

            _pooledObjects[target][beforeCreateCount].SetActive(true);
            return _pooledObjects[target][beforeCreateCount];
        }
        else
        {
            return null;
        }
    }
    
    // 오브젝트를 해제해 풀로 되돌려 놓음
    public void ReleaseObjectToPool(GameObject go)
    {
        go.SetActive(false);
        go.transform.SetParent(Instance.transform);
        go.transform.localPosition = Vector3.zero;
    }
}
