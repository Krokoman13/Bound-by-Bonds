using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfPlayer : MonoBehaviour
{
    [SerializeField] List<Object> toDestroy;
    [SerializeField] Sibling playerType;

    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerInfo.currentSibling == playerType)
        {
            if (toDestroy.Count < 1) toDestroy.Add(gameObject);

            foreach (Object objectToDestroy in toDestroy)
            {
                Destroy(objectToDestroy);
            }
        }

        Destroy(this);
    }
}
