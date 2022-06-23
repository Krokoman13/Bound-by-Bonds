using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerFPSGroundCheck))]
[RequireComponent(typeof(PlayerFPSMovement))]
[RequireComponent(typeof(PlayerFPSSprinting))]
[RequireComponent(typeof(PlayerFPSCrouching))]
//[RequireComponent(typeof(SphereCollider))]
//[RequireComponent(typeof(PlayerFPSJump))]

public class PlayerStealth : PlayerComponent
{
    //Player Components
    PlayerFPSGroundCheck groundCheck = null;
    PlayerFPSMovement movement = null;
    PlayerFPSSprinting sprint = null;
    PlayerFPSCrouching crouch = null;
    //PlayerFPSJump jump = null;

    [HideInInspector] public float currentStealth = 1.5f;
    [HideInInspector] public float currentStealth_perc = 5;      //Current Stealth value in percent
    float targetStealth_perc = 0;                               //Target stealth value in percent (based on current action)
    [Header("Stealth Weight")]
    [SerializeField, Range(0.01f, 1f), Tooltip("Decides final stealth value. stealth-percent * stealthWeight. (should preferably be within 0-1)")]
    float stealthWeight = 0.3f;
    [Header("Stealth Change Speed")]
    [SerializeField] float stealthChangeSpeed = 5;

    [Header("Action Loudnesses")]
    [SerializeField] float maxNoise_idle = 5f;
    [SerializeField] float maxNoise_movement = 40;
    [SerializeField] float maxNoise_sprinting = 60;
    [SerializeField] float maxNoise_crouching = 10;
    [Space(3)]
    [SerializeField] float noise_landing = 50;

    //private SphereCollider soundCollider;
    //private List<NoiseReciever> noiseRecievers = new List<NoiseReciever>();

    public override void OnPlayerFixedUpdate()
    {
        DetermineStealth();
        //soundCollider.radius = currentStealth;

        //Run based on interval to improve performance.
        if (Time.frameCount % 10 != 0)
            return;

        Collider[] collided = Physics.OverlapSphere(transform.position, currentStealth);

        foreach (Collider col in collided)
        {
            float distance = Vector3.Distance(transform.position, col.transform.position);
            if (distance > currentStealth) continue;

            NoiseReciever noiseReciever;
            col.gameObject.TryGetComponent<NoiseReciever>(out noiseReciever);

            if (noiseReciever == null) continue;

            noiseReciever.RecieveNoise(transform.position); ;
        }
    }

    void DetermineStealth()
    {
        if (currentStealth_perc == targetStealth_perc)
            return;

        currentStealth_perc = Mathf.Lerp(currentStealth_perc, targetStealth_perc, stealthChangeSpeed * Time.fixedDeltaTime);
        currentStealth = currentStealth_perc * stealthWeight;

        //Idle
        if (movement.inputs == Vector2.zero)
        {
            targetStealth_perc = maxNoise_idle;
            return;
        }
        //Moving
        else
            targetStealth_perc = maxNoise_movement;

        //Sprinting
        if (sprint.isSprinting)
            targetStealth_perc = maxNoise_sprinting;
        //Crouching
        else if (crouch.isCurrentlyCrouching)
            targetStealth_perc = maxNoise_crouching;
    }

    //Called once when player lands on the ground
    public void LandingNoise()
    {
        currentStealth_perc = noise_landing;
    }


    public override void OnPlayerStart(Player pPlayer)
    {
        GetComponents();
        groundCheck.onLanding.AddListener(LandingNoise);
    }

    void GetComponents()
    {
        //Get all components
        TryGetComponent<PlayerFPSGroundCheck>(out groundCheck);
        TryGetComponent<PlayerFPSMovement>(out movement);
        TryGetComponent<PlayerFPSSprinting>(out sprint);
        TryGetComponent<PlayerFPSCrouching>(out crouch);
        //TryGetComponent<PlayerFPSJump>(out jump);
    }



    void OnValidate()
    {
        Clamp100(ref maxNoise_idle);
        Clamp100(ref maxNoise_movement);
        Clamp100(ref maxNoise_sprinting);
        Clamp100(ref maxNoise_crouching);
        Clamp100(ref noise_landing);

        ClampPositive(ref stealthChangeSpeed);

/*        if (soundCollider == null)
        {
            soundCollider = GetComponent<SphereCollider>();
            soundCollider.isTrigger = true;
        }*/
    }

    void Clamp100(ref float valueToClamp)
    {
        valueToClamp = Mathf.Clamp(valueToClamp, 0, 100);
    }

    void ClampPositive(ref float valueToClamp)
    {
        valueToClamp = Mathf.Max(0, valueToClamp);
    }

/*    private void OnTriggerEnter(Collider other)
    {
        NoiseReciever noiseReciever;

        other.gameObject.TryGetComponent<NoiseReciever>(out noiseReciever);
        if (noiseReciever == null) return;

        noiseRecievers.Add(noiseReciever);
    }

    private void OnTriggerExit(Collider other)
    {
        NoiseReciever noiseReciever;

        other.gameObject.TryGetComponent<NoiseReciever>(out noiseReciever);
        if (noiseReciever == null) return;

        noiseRecievers.Remove(noiseReciever);
    }*/
}