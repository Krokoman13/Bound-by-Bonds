using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVoidDetect : PlayerComponent
{
    Vector3 spawnPos = Vector3.zero;
    [SerializeField] int interval = 30;
    [SerializeField] float voidHeight = -10;
    public override void OnPlayerStart(Player pPlayer)
    {
        spawnPos = transform.position;
    }

    public override void OnPlayerUpdate()
    {
        if (Time.frameCount % interval != 0)
            return;

        if(transform.position.y <= voidHeight)
        {
            transform.position = spawnPos;
        }
    }
}