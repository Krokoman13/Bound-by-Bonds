using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalMap : MonoBehaviour
{
    [SerializeField]Transform mapT = null;     
    [SerializeField]Camera cam = null;

    [SerializeField]float distance = 1;
    [SerializeField] float followSpeed = 2f;    

    void FixedUpdate()
    {
        mapT.position = cam.transform.position;


        Vector3 targetAngle = cam.transform.forward;
        Vector3 nextAngle = Vector3.Lerp(mapT.forward, targetAngle, Time.fixedDeltaTime * followSpeed);


        mapT.forward = nextAngle;   
    }    
}