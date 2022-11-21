using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyEnableIfPlayer : MonoBehaviour
{
    [SerializeField] List<GameObject> gameObjects;
    [SerializeField] List<Behaviour> behaviours;
    [SerializeField] List<Rigidbody> rigidbodies;
    [SerializeField] Sibling sibling;


    Sibling currentSibling = Sibling.UNKNOWN;
    // Start is called before the first frame update
    void Awake()
    {
        setRightState();
    }

    private void FixedUpdate()
    {
        if (currentSibling == PlayerInfo.currentSibling) return;
        setRightState();
    }

    private void setRightState()
    {
        currentSibling = PlayerInfo.currentSibling;
        bool state = currentSibling == sibling;

        if ((gameObjects == null || gameObjects.Count <= 0)
            && (behaviours == null || behaviours.Count <= 0)
            && (rigidbodies == null || rigidbodies.Count <= 0))
        {
            gameObject.SetActive(state);
        }

        foreach (GameObject gmObjct in gameObjects)
        {
            if (gmObjct.activeSelf == state) continue;
            Debug.Log(gmObjct);
            gmObjct.SetActive(state);
        }

        foreach (Behaviour behaviour in behaviours)
        {
            if (behaviour.enabled == state) continue;
            behaviour.enabled = state;
        }

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            if (rigidbody.detectCollisions == state) continue;

            rigidbody.isKinematic = !state;
            rigidbody.detectCollisions = state;
        }
    }
}
