using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateController : MonoBehaviour
{
    [SerializeField] protected int _state;

    public int State
    {
        get
        {
            return _state;
        }
        set
        {
            var oldState = _state;
            var newState = value;
            _state = value;
            OnChangeStateEvent?.Invoke(oldState, newState);
        }
    }
    public UnityEvent<int, int> OnChangeStateEvent;
}
