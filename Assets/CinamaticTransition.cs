using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

using UnityEngine.Video;

public class CinamaticTransition : MonoBehaviour
{
    [SerializeField] GameObject lookAt;
    [SerializeField] float delay;
    [SerializeField] VideoPlayer videoPlayer;

    [SerializeField] Transform teleportPoint;
    [SerializeField] GameObject spaceA;
    [SerializeField] GameObject spaceB;

    UnityEvent onLookedAt;
    UnityEvent onTeleported;
    UnityEvent onVideoDone;

    Transform toTeleport;

    bool videoStarted = false;

    public void Start()
    {
        if (spaceA != null && spaceB != null) spaceB.SetActive(false);
    }

    public void StartTransition()
    {
        lookAt.SetActive(true);
        StartCoroutine(DelayedFunction(() => OnLookedAt(), delay));
    }

    public void StartTransition(Transform pToTeleport)
    {
        toTeleport = pToTeleport;

        StartTransition();
    }

    IEnumerator DelayedFunction(Action action, float seconds)
    {
        //Debug.Log("Started");
        yield return new WaitForSeconds(seconds);
        action();
    }

    private void OnLookedAt()
    {
        lookAt.SetActive(false);
        videoPlayer.gameObject.SetActive(true);
        Teleport(toTeleport.parent.parent);
        onLookedAt?.Invoke();
    }

    private void Teleport(Transform toTeleport)
    {
        toTeleport.position = teleportPoint.position;
        toTeleport.rotation = teleportPoint.rotation;

        if (spaceA == null || spaceB == null) return;

        spaceA.gameObject.SetActive(false);
        spaceB.gameObject.SetActive(true);
        teleportPoint.gameObject.SetActive(true);
    }

    private void OnDrawGizmos()
    {    
        Gizmos.color = Color.red;
        DrawCameraGizmo(lookAt.transform);
        DrawCameraGizmo(teleportPoint);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(lookAt.transform.position, teleportPoint.position);
    }

    private void DrawCameraGizmo(Transform cameraPoint)
    {
        Camera camera = Camera.main;

        Matrix4x4 temp = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(cameraPoint.position, cameraPoint.rotation, Vector3.one);
        if (camera.orthographic)
        {
            float spread = camera.farClipPlane - camera.nearClipPlane;
            float center = (camera.farClipPlane + camera.nearClipPlane) * 0.5f;
            Gizmos.DrawWireCube(new Vector3(0, 0, center), new Vector3(camera.orthographicSize * 2 * camera.aspect, camera.orthographicSize * 2, spread));
        }
        else
        {
            Gizmos.DrawFrustum(Vector3.zero, camera.fieldOfView, camera.nearClipPlane + 1f, camera.nearClipPlane, camera.aspect); ;
        }
        Gizmos.matrix = temp;
    }

    private void Update()
    {
        if (!videoPlayer.gameObject.activeSelf) return;

        if (videoPlayer.isPlaying)
        {
            videoStarted = true;
            return;

        }

        if (!videoPlayer.isPlaying && videoStarted)
        {
            videoStarted = false;
            videoPlayer.gameObject.SetActive(false);
            OnVideoDone();
        }

        //videoPlayer.gameObject.SetActive(false);
    }

    private void OnVideoDone()
    {
        StartCoroutine(DelayedFunction(() => teleportPoint.gameObject.SetActive(false), delay/2));
        onVideoDone?.Invoke();
    }
}
