using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine;

public class PlayerFPSGroundCheck : PlayerComponent
{    
    [HideInInspector]public bool isGrounded = false;
    /*[HideInInspector]*/ public bool tooSteep = false;

    [Header("Detection Dimensions")]
    public Transform floorCheckBox = null;

    [Header("Limit Detection")]
    [SerializeField, Tooltip("Layermask limits what objects will be detected (by layer)")]  //Don't forget to exclude the player's own collider.
    LayerMask lmask = 1;
    [SerializeField, Tooltip("Further limit what objects will be detected by using tags.")]
    List<string> floorTags = new List<string>();

    public float maxSlopeAngle = 35;
    [HideInInspector] public float currentSlopeAngle = 0;
    [HideInInspector] public Vector3 currentSlopeNormal = Vector3.up;
    [HideInInspector] public Vector3 slopeRight = Vector3.right;
    [HideInInspector] public Vector3 currentSlopeDir = Vector3.zero;

    [Header("Events")]
    public UnityEvent onLanding = null;
    public UnityEvent onGroundLeave = null;


    [HideInInspector] public Vector3[] floorcheck_bounds = new Vector3[4];   //starting from close-left, goes clockwise order (farleft next)
    void CheckGround()
    {
        //Cast a ray downwards from the character        
        RaycastHit hit = new RaycastHit();
        Vector3 halfExtents = floorCheckBox.localScale/2;        
        Vector3 startpos = floorCheckBox.position + (transform.up * halfExtents.y);
        //RaycastHit[] hits = Physics.BoxCastAll(startpos, halfExtents, -transform.up, Quaternion.identity, halfExtents.y/2, lmask, QueryTriggerInteraction.Ignore);        

        RaycastHit[] hits = new RaycastHit[5];
        Vector3 dir = -transform.up;
        float maxDistance = floorCheckBox.localScale.y;
        floorcheck_bounds[0] = (startpos - (transform.right * halfExtents.x)) - (transform.forward * halfExtents.z);
        floorcheck_bounds[1] = (startpos - (transform.right * halfExtents.x)) + (transform.forward * halfExtents.z);
        floorcheck_bounds[2] = (startpos + (transform.right * halfExtents.x)) + (transform.forward * halfExtents.z);
        floorcheck_bounds[3] = (startpos + (transform.right * halfExtents.x)) - (transform.forward * halfExtents.z);

        Physics.Raycast(startpos, dir, out hits[0], maxDistance, lmask, QueryTriggerInteraction.Ignore);
        for(int i = 0; i < floorcheck_bounds.Length; i++)
        {
            Physics.Raycast(floorcheck_bounds[i], dir, out hits[i+1], maxDistance, lmask, QueryTriggerInteraction.Ignore);
        }

        //Get a result from the array.
        foreach(RaycastHit h in hits)
        {
            if (h.transform != null)
            {
                hit = h;
                break;
            }
        }

        ////Get hit closest to ray origin
        //if (hits.Length > 0)
        //{
        //    List<float> distances = new List<float>();
        //    foreach (RaycastHit h in hits)
        //    {
        //        if (hit.transform == null)
        //            continue;

        //        distances.Add(h.distance);
        //    }            
        //    int lowestIndex = distances.IndexOf(distances.Min());
        //    hit = hits[lowestIndex];

        //    Debug.DrawRay(hit.point, hit.normal * 10, Color.green);            
        //}

        //On hit: 
        if (hit.collider != null && floorTags.Contains(hit.collider.tag))
        {
            currentSlopeNormal = hit.normal;

            //Get slope info
            currentSlopeAngle = Vector3.Angle(transform.up, hit.normal); //Calc angle between normal and character
            slopeRight = Vector3.Cross(hit.normal, Vector3.up).normalized;
            currentSlopeDir = Vector3.Cross(hit.normal, slopeRight).normalized; //get the direction from the slope downwards            
            
            //SLOPE IS TOO STEEP
            if (currentSlopeAngle > maxSlopeAngle)
            {
                tooSteep = true;
                if (isGrounded)
                    onGroundLeave?.Invoke();
                
                isGrounded = false;
                return;
            }

            //SLOPE IS FINE
            else if (!isGrounded)            
                onLanding?.Invoke();    //perhaps check if player had enough airtime to warrant a 'landing'          

            tooSteep = false;
            isGrounded = true;
        }
        //No hit:
        else
        {            
            //Was grounded before --> player leaves the ground now.
            if (isGrounded)                            
                onGroundLeave?.Invoke();            

            isGrounded = false;
            //tooSteep = false;       //reset to base value
            currentSlopeNormal = transform.up;
        }
    }





    public void FixNormal(Vector3 position, ref RaycastHit hit, int layermask)
    {        
        RaycastHit rayHit;
        Physics.Raycast(position, hit.point - position, out rayHit, 2 * hit.distance, layermask);
        hit.normal = rayHit.normal;
    }

    public override void OnPlayerUpdate()
    {
        //CheckGround();
    }

    public override void OnPlayerFixedUpdate()
    {
        CheckGround();
    }
    public override void OnPlayerStart(Player pPlayer)
    {
        if (floorCheckBox == null)
        {
            Debug.LogError("Error, the floorcheckbox has not been assigned so the player can't detect the floor", this);
            this.enabled = false;
            return;
        }
    }
}