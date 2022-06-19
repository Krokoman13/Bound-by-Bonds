using UnityEngine;

public class TriggerSubtitle : MonoBehaviour
{
    [SerializeField, Tooltip("Pick one from random pool. Use 1 if it should be consistent.")]
    int[] subtitleIndexes = new int[1] {3};
    public ScriptUsageProgrammerSounds fmodAudioSource = null;

    [SerializeField]bool skipQueue = true;


    SubtitlesHandler subHandler = null;

    public void DO_SUBTITLE(bool useQueue)
    {
        if (isBugged()) return; 


        int rndm = Random.Range(0, subtitleIndexes.Length);
        subHandler.currentFMOD_AudioSource = fmodAudioSource;

        if (!useQueue)
            subHandler.PlaySubtitleNow(subtitleIndexes[rndm]);
        else
            subHandler.AddSubtitleToQueue(subtitleIndexes[rndm]);
    }



    void Awake()
    {
        subHandler = FindObjectOfType<SubtitlesHandler>();    
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