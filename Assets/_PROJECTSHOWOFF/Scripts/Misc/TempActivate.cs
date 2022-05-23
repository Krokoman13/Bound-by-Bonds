using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempActivate : MonoBehaviour
{

    [SerializeField] float seconds = 1;
    [SerializeField] GameObject toActivate;

    public void Activate(float pSeconds)
    {
        seconds = pSeconds;
        Activate();
    }

    public void Activate()
    {
        toActivate.SetActive(true);
        StartCoroutine(DelayedDeActivate());
    }

    IEnumerator DelayedDeActivate()
    {
        //Debug.Log("Started");
        yield return new WaitForSeconds(seconds);
        toActivate.SetActive(false);
    }
}
