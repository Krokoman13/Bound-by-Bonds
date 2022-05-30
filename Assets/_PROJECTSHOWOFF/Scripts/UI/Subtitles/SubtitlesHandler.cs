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



    [HideInInspector]public ScriptUsageProgrammerSounds currentFMOD_AudioSource = null;
    ReadSheet_Subtitles readSheet = null;


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


    public void AddSubtitleToQueue(int subtitleLine, ScriptUsageProgrammerSounds fmodSource)
    {
        Subtitle sub = readSheet.GetSubtitleFromIndex(subtitleLine);
        FindObjectOfType<ScriptUsageProgrammerSounds>().PlayAudioFMOD(sub.audioKey);

        OnEnableSubtitles();
        subtitles.Enqueue(sub);
        runSubtitleTimer = true;
    }



    //[ContextMenu("Add test subtitle")]
    //void TestSubtitles()
    //{
    //    Subtitle newSub = new Subtitle();
    //    newSub.text = $"This is a test message. Here's a random number: {Random.Range(0, 5000)}";
    //    newSub.appearTime = 1;
    //    AddNewSubtitle(newSub);
    //}

    public void AddSubtitleToQueue(int subtitleLine)
    {        
        Subtitle sub = readSheet.GetSubtitleFromIndex(subtitleLine);
        currentFMOD_AudioSource.PlayAudioFMOD(sub.audioKey);
        //FindObjectOfType<ScriptUsageProgrammerSounds>().PlayAudioFMOD(sub.audioKey);

        OnEnableSubtitles();
        subtitles.Enqueue(sub);
        runSubtitleTimer = true;
    }



    public void PlaySubtitleNow(int subtitleLine)
    {        
        Subtitle sub = readSheet.GetSubtitleFromIndex(subtitleLine);
        currentFMOD_AudioSource.PlayAudioFMOD(sub.audioKey);
        //FindObjectOfType<ScriptUsageProgrammerSounds>().PlayAudioFMOD(sub.audioKey);

        OnEnableSubtitles();
        subtitle_timer = 0;
        subtitles.Clear();
        subtitles.Enqueue(sub);
        runSubtitleTimer = true;
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


//    void Update()
//    {
//#if UNITY_EDITOR
//        if (Input.GetKeyDown(KeyCode.T))
//        {
//            TestSubtitles();
//        }
//#endif
//    }

    void Awake()
    {
        TryGetComponent<TextMeshProUGUI>(out subtitle_text);
        TryGetComponent<ReadSheet_Subtitles>(out readSheet);
        OnDisableSubtitles();
    }
    
}