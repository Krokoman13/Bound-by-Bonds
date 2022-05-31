using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitleSender : MonoBehaviour
{
    [SerializeField] Sibling target;
    [SerializeField] int[] subtitleIndexes = new int[1] { 3 };
    public ScriptUsageProgrammerSounds fmodAudioSource = null;

    [SerializeField] bool skipQueue = true;

    public void SendSubtitle()
    {
        if (isBugged()) return;

        SubtitlesHandler subHandler = null;

        switch (target)
        {
            case Sibling.Dana:
                subHandler = DanaSubtitlesHandler.instance;
                //SoldierSubtitlesHandler.instance.AddSubtitleToQueue(subtitle);
                break;

            case Sibling.Denys:
                subHandler = DenysSubtitlesHandeler.instance;
                //OperatorSubtitlesHandeler.instance.AddSubtitleToQueue(subtitle);
                break;
        }

        int rndm = Random.Range(0, subtitleIndexes.Length);
        subHandler.currentFMOD_AudioSource = fmodAudioSource;

        if (skipQueue)
            subHandler.PlaySubtitleNow(subtitleIndexes[rndm]);
        else
            subHandler.AddSubtitleToQueue(subtitleIndexes[rndm]);
    }

    bool isBugged()
    {
        if (fmodAudioSource == null)
        {
            Debug.LogError("ERROR: FMOD_AUDIO-SOURCE NOT ASSIGNED.", this.gameObject);
            return true;
        }
        return false;
    }
}
