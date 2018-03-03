using System;
using UnityEngine;

public class ChildTrigger : MonoBehaviour {

    event Action<Collider> _onEnter = delegate { };
    event Action<Collider> _onStay = delegate { };
    event Action<Collider> _onExit = delegate { };

    public event Action<Collider> OnEnter
    {
        add { _onEnter += value; }
        remove { _onEnter -= value; }
    }

    public event Action<Collider> OnStay
    {
        add { _onStay += value; }
        remove { _onStay -= value; }
    }

    public event Action<Collider> OnExit
    {
        add { _onExit += value; }
        remove { _onExit -= value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        _onEnter(other);
    }

    private void OnTriggerStay(Collider other)
    {
        _onStay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _onExit(other);
    }
}
