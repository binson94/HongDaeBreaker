using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
    Queue<T> _poolQueue = new Queue<T>();
    T _prefab;

    public Pool(T prefab)
    {
        _prefab = prefab;
    }

    public T Get(RectTransform parent)
    {
        T ret;
        if (_poolQueue.Count <= 0)
            ret = Object.Instantiate(_prefab);
        else
            ret = _poolQueue.Dequeue();

        ret.gameObject.SetActive(false);
        ret.transform.SetParent(parent);
        ret.transform.localScale = new Vector3(1, 1, 1);
        return ret;
    }

    public void Release(T obj)
    {
        obj.gameObject.SetActive(false);
        _poolQueue.Enqueue(obj);
    }
}