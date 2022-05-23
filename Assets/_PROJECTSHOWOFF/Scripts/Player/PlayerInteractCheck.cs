using System.Collections.Generic;
using UnityEngine;


public class PlayerInteractCheck : PlayerComponent
{
    [Header("Update method Interval"), SerializeField,
    Tooltip("how many frames should there be between each update call? value = 1 --> run every frame.")]
    float interval = 30;

    //Components            
    Transform camT = null;    //Reference to the Camera's transform.

    //Target result
    [HideInInspector] public Interactable highlightedTarget = null;

    //Variables
    [SerializeField] float targetSelectionRange = 50;
    [SerializeField] float targetRayRadius = 0.8f;
    [SerializeField] LayerMask lmask = 0;

    public delegate void InteractDelegate();
    public InteractDelegate onHighlightStart = null;
    public InteractDelegate onHighlightStop = null;


    public override void OnPlayerStart(Player pPlayer)
    {
        //Application.targetFrameRate = 60;
        camT = Camera.main.transform;
        highlightedTarget = null;
    }

    public override void OnPlayerUpdate()
    {
        if (camT == null)
        {
            this.enabled = false;
            return;
        }


        //Run based on interval to improve performance.
        if (Time.frameCount % interval != 0)
            return;

        TargetSelectionRay();
    }


    /// Casts a ray forward from the camera's position and will highlight objects it sees.    
    void TargetSelectionRay()
    {
        List<Interactable> interactables = new List<Interactable>();

        Debug.DrawLine(camT.position, camT.position + camT.forward * targetSelectionRange, Color.red);

        //Cast a thick ray forward and grab all the collisions along the way
        RaycastHit[] hits = Physics.SphereCastAll(camT.position, targetRayRadius, camT.forward, targetSelectionRange, lmask, QueryTriggerInteraction.Ignore);
        if (hits.Length > 0)
        {
            //Add any found interactables to list
            foreach (RaycastHit hit in hits)
            {
                Interactable interactable = hit.transform.GetComponent<Interactable>();
                if (interactable == null)
                    continue;

                interactables.Add(interactable);
            }


            //Did we hit an interactable?
            if (interactables.Count > 0)
            {
                //If so, detect the closest one.
                DetectingHit(GetClosestInteractable(interactables));
            }
            //No interactable found within the cast.
            else
            {
                //Stop highlighting any potential previous targets.                
                highlightedTarget = null;
                onHighlightStop?.Invoke();
            }
        }
        //HIT NOTHING
        else
        {
            //UN-HIGHLIGHT CURRENT INTERACTABLE            
            highlightedTarget?.OnHighlightStop();
            highlightedTarget = null;

            onHighlightStop?.Invoke();
        }

    }

    Interactable GetClosestInteractable(List<Interactable> _interactables)
    {
        Interactable result = null;

        //Compare distances and return the interactable with the closest.
        float closestDistance = Mathf.Infinity;
        Vector3 currentPos = camT.position;
        foreach (Interactable inter in _interactables)
        {
            Vector3 dir = inter.transform.position - currentPos;
            float dis = dir.sqrMagnitude;
            if (dis < closestDistance)
            {
                closestDistance = dis;
                result = inter;
            }
        }
        return result;
    }

    /// <summary>
    /// Used for various different scenarios regarding the target-highlighting.
    /// </summary>
    /// <param name="_hit">A raycast, sent from the camera center forward.</param>
    void DetectingHit(Interactable _interactable)
    {
        //Highlight exists
        if (highlightedTarget != null)
        {
            //Hitting anything BUT the same highlight...
            if (_interactable != highlightedTarget)
            {
                //Stop the current highlight-target... if it exists.
                highlightedTarget?.OnHighlightStop();
                onHighlightStop?.Invoke();
                highlightedTarget = null;

                //Set a new highlight-target.
                SetNew_Highlight(_interactable);
            }
        }
        //Highlight doesn't exist
        else
        {
            SetNew_Highlight(_interactable);
        }
    }

    void SetNew_Highlight(Interactable _interactable)
    {
        //Save variable for later use and set it to the new state by starting the method.
        highlightedTarget = _interactable;
        highlightedTarget.OnHighlightStart();
        onHighlightStart?.Invoke();
    }
}