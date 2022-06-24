using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnStart : MonoBehaviour
{
    [SerializeField] float delay = 0;
    [SerializeField] UnityEvent onStart = null;

    void Start()
    {
        Invoke("delayedInvoke", delay);
    }

    void delayedInvoke()
    {

        onStart?.Invoke();
    }
}