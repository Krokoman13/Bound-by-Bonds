using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limiter : PlayerComponent
{
    [SerializeField] Vector3 min;
    [SerializeField] Vector3 max;

    public override void OnPlayerFixedUpdate()
    {
        Vector3 currentPosition = transform.position;

        currentPosition.x = Mathf.Clamp(currentPosition.x, min.x, max.x);
        currentPosition.y = Mathf.Clamp(currentPosition.y, min.y, max.y);
        currentPosition.z = Mathf.Clamp(currentPosition.z, min.z, max.z);
        transform.position = currentPosition;
    }

    private void OnDrawGizmos()
    {
        Vector3 middle = min + (max - min)/2;
        Vector3 size = max - min;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(middle, size);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(min, 1);
        Gizmos.DrawSphere(max, 1);
    }
}
