using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sep_TestTrigger : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent onTrigger = null;
    public bool deleteAfterTrigger = false;


    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerSoldier")
        {            
            onTrigger?.Invoke();

            if (deleteAfterTrigger)
                Destroy(this);

        }
    }
}