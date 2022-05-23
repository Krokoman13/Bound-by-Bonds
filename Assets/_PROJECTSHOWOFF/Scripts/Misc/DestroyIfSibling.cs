using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class DestroyIfSibling : MonoBehaviour
{
    [SerializeField] List<Object> toDestroy;
    [SerializeField] Sibling sibling;

    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerInfo.currentSibling == sibling)
        {
            if (toDestroy.Count < 1) toDestroy.Add(gameObject);

            foreach(Object objectToDestroy in toDestroy)
            {
                Destroy(objectToDestroy);
            }
        }

        Destroy(this);
    }
}
