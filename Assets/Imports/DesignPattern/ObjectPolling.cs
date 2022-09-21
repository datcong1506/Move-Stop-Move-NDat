using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ObjectPolling
{
    private Dictionary<GameObject, byte> _avaiableObjects;
    private Dictionary<GameObject, byte> _unAvaibaleObjects;
    [SerializeField] private GameObject _originPool;
    [SerializeField] private GameObject _owner;
    [SerializeField] private Transform _container;
    [SerializeField] public PollContainer _pollContainer;
    public ObjectPolling(GameObject owner, GameObject poolObject, int num = 20)
    {
        _avaiableObjects = new Dictionary<GameObject, byte>();
        _unAvaibaleObjects = new Dictionary<GameObject, byte>();


        _originPool = poolObject;
        _owner = owner;

        _container = new GameObject(_originPool.name + "_Polling_Container").transform;
        _container.gameObject.AddComponent<PollContainer>().Initial(_owner);
        _pollContainer = _container.GetComponent<PollContainer>();
        for (int i = 0; i < num; i++)
        {
            var newInstance = GameObject.Instantiate(_originPool);
            newInstance.AddComponent<PollObject>().Initial(this);
            newInstance.transform.SetParent(_container);
            _unAvaibaleObjects.Add(newInstance, 0);
            newInstance.SetActive(false);
        }
    }
    public int UnavaiableCount()
    {
        return _unAvaibaleObjects.Count;
    }
    public int AvaiableCount()
    {
        return _avaiableObjects.Count;
    }
    public void OnPollDisable(GameObject pollObject)
    {
        _avaiableObjects.Add(pollObject, 0);
        _unAvaibaleObjects.Remove(pollObject);
        pollObject.transform.SetParent(_container);
        if (pollObject.activeInHierarchy)
        {
            pollObject.SetActive(false);
        }
    }

    public void OnPollEnable(GameObject poGameObject)
    {
        _avaiableObjects.Remove(poGameObject);
        _unAvaibaleObjects.Add(poGameObject, 0);
    }

    public void OnPollDestroy(GameObject pollObject)
    {
        _avaiableObjects.Remove(pollObject);
    }


    public GameObject Instantiate()
    {
        if (_avaiableObjects.Count > 0)
        {
            var instance = _avaiableObjects.ElementAt(0).Key;
            instance.SetActive(true);
            return instance;
        }
        var newInstance = GameObject.Instantiate(_originPool);
        newInstance.AddComponent<PollObject>().Initial(this);
        newInstance.transform.SetParent(_container);
        _unAvaibaleObjects.Add(newInstance, 0);
        return newInstance;
    }

    public void DestroyPool()
    {
        for (int i = 0; i < _avaiableObjects.Count; i++)
        {
            GameObject.Destroy(_avaiableObjects.ElementAt(i).Key);
        }
        for (int i = 0; i < _unAvaibaleObjects.Count; i++)
        {
            GameObject.Destroy(_unAvaibaleObjects.ElementAt(i).Key);
        }
        GameObject.Destroy(_container.gameObject);

        _avaiableObjects.Clear();
        _unAvaibaleObjects.Clear();
    }

    public void Recycle()
    {
        var count = _unAvaibaleObjects.Count;
        for (int i = 0; i < count; i++)
        {
            _unAvaibaleObjects.ElementAt(0).Key.SetActive(false);
        }
    }
}

[DisallowMultipleComponent]
public class PollObject : MonoBehaviour
{
    [SerializeField] private ObjectPolling _objectPolling;



    public void Initial(ObjectPolling objectPolling)
    {
        _objectPolling = objectPolling;
    }

    private void OnDisable()
    {
        /*
        _objectPolling.OnPollDisable(gameObject);
    */
        if (_objectPolling != null)
        {
            if (_objectPolling._pollContainer != null)
            {
                if (_objectPolling._pollContainer.gameObject.activeInHierarchy)
                {
                    _objectPolling._pollContainer.StartCoroutine(OnDisableDelayOneFrame());
                }
            }
        }
    }

    IEnumerator OnDisableDelayOneFrame()
    {
        yield return new WaitForEndOfFrame();
        _objectPolling.OnPollDisable(gameObject);
    }

    private void OnDestroy()
    {
        _objectPolling.OnPollDestroy(gameObject);
    }

    private void OnEnable()
    {
        if (_objectPolling != null)
        {
            _objectPolling.OnPollEnable(gameObject);
        }
    }
}

public class PollContainer : MonoBehaviour
{
    [SerializeField] private GameObject _owner;

    public void Initial(GameObject owner)
    {
        _owner = owner;
    }
}
