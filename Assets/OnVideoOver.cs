using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Video;
using UnityEngine.Events;

[RequireComponent(typeof(VideoPlayer))]
public class OnVideoOver : MonoBehaviour
{
    VideoPlayer player;
    bool videoStarted = false;

    public UnityEvent onVideoOver = null;

    private void Awake()
    {
        player = GetComponent<VideoPlayer>();
    }

    private void Update()
    {
        if (player.isPlaying)
        {
            videoStarted = true;
            return;

        }

        if (!player.isPlaying && videoStarted)
        {
            videoStarted = false;
            onVideoOver?.Invoke();
        }

        //videoPlayer.gameObject.SetActive(false);
    }
}
