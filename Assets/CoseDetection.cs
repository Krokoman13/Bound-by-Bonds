using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class CoseDetection : MonoBehaviour
{
    [SerializeField] Transform denys;
    [SerializeField] Transform dana;

    [SerializeField] Transform collider;

    [SerializeField] float distance;

    private void FixedUpdate()
    {
        float currentDistance = Vector3.Distance(denys.position, dana.position);

        bool closeEnough = currentDistance < distance;

        collider.gameObject.SetActive(closeEnough);

        if (closeEnough)
        { 
            if (PlayerInfo.currentSibling == Sibling.Denys) collider.transform.position = denys.position;
            if (PlayerInfo.currentSibling == Sibling.Dana) collider.transform.position = denys.position;
        }
    }
}
