using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SubtitlesHandler : MonoBehaviour
{
    TextMeshProUGUI subtitle_text = null;
    public Queue<Subtitle> subtitles = new Queue<Subtitle>();
    float subtitle_timer = 0;

    public bool runSubtitleTimer = true;
    public int currentQueueCount = 0;

    bool fadeTextActive = false;
    [SerializeField] float alphaSpeed = 8;
    float targetAlpha = 0;



    public void StartNextSubtitle()
    {
        //There's no next subtitle
        if (subtitles.Count < 1)
        {
            runSubtitleTimer = false;
            OnDisableSubtitles();
            return;
        }

        //Next subtitle        
        Subtitle newSub = subtitles.Dequeue();
        subtitle_text.text = newSub.getText;
        subtitle_timer = newSub.appearTime;
        runSubtitleTimer = true;
    }


    void AddNewSubtitle(Subtitle sub)
    {
        OnEnableSubtitles();
        subtitles.Enqueue(sub);
        runSubtitleTimer = true;
    }

    void FixedUpdate()
    {
        if (fadeTextActive)
        {
            Color newColor = subtitle_text.color;
            float newAlpha = subtitle_text.color.a;
            newAlpha = Mathf.Lerp(newAlpha, targetAlpha, alphaSpeed * Time.fixedDeltaTime);
            newColor.a = newAlpha;
            subtitle_text.color = newColor;
        }

        if (!runSubtitleTimer)
            return;

        currentQueueCount = subtitles.Count;

        subtitle_timer -= Time.fixedDeltaTime;
        if (subtitle_timer < 0)
        {
            StartNextSubtitle();
        }



    }

    [ContextMenu("Add test subtitle")]
    void TestSubtitles()
    {
        Subtitle newSub = new Subtitle();
        newSub.text = $"This is a test message. Here's a random number: {Random.Range(0, 5000)}";
        newSub.appearTime = 1;
        AddNewSubtitle(newSub);
    }

    public void AddSubtitleToQueue(Subtitle newSub)
    {                     
        AddNewSubtitle(newSub);
    }


    void OnEnableSubtitles()
    {
        fadeTextActive = true;
        targetAlpha = 1;
    }

    void OnDisableSubtitles()
    {
        fadeTextActive = true;
        targetAlpha = 0;
    }


    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestSubtitles();
        }
#endif
    }

    void Awake()
    {
        TryGetComponent<TextMeshProUGUI>(out subtitle_text);
        OnDisableSubtitles();
    }
}