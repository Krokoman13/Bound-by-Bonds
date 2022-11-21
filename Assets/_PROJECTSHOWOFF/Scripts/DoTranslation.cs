using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class DoTranslation : MonoBehaviour
{
    [SerializeField] Transform tr = null;

    [SerializeField] float maxHeight = 10f;
    float currentSpeed = 0;
    [SerializeField] float accSpeed = 0.1f;

    [Header("Subtitles Move speed")]
    [SerializeField] float scrollSpeed = .1f;
    [SerializeField] float fastScrollSpeed = .2f;

    [SerializeField] UnityEvent onFinished = null;

    void FixedUpdate()
    {
        currentSpeed += accSpeed * Time.fixedDeltaTime;
        currentSpeed = Mathf.Min(currentSpeed, scrollSpeed);

        tr.Translate(Vector3.up * currentSpeed * Time.fixedDeltaTime, Space.World);


        if (tr.position.y > maxHeight)
        {
            onFinished?.Invoke();
            this.enabled = false;
        }

    }



}