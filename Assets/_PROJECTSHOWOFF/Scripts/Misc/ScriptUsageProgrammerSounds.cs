using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class ScriptUsageProgrammerSounds : MonoBehaviour
{
    FMOD.Studio.EVENT_CALLBACK dialogueCallback;

    string eventPrefix = "event:";
    public FMODUnity.EventReference eventName;
    public FMOD.Studio.EventInstance currentEventInstance = new FMOD.Studio.EventInstance();

#if UNITY_EDITOR
    void Reset()
    {
        eventName = FMODUnity.EventReference.Find($"{eventPrefix}/FMOD_Audio/Voice_NPC/Voice_Line_NPC_1");
    }
#endif

    void Start()
    {
        // Explicitly create the delegate object and assign it to a member so it doesn't get freed
        // by the garbage collected while it's being used
        dialogueCallback = new FMOD.Studio.EVENT_CALLBACK(DialogueEventCallback);
    }

    public void PlayAudioFMOD(string key, bool cancelPreviousEvent = true)
    {
        //Stop previous Audio Event
        if(cancelPreviousEvent) currentEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        //Create Instance
        currentEventInstance = FMODUnity.RuntimeManager.CreateInstance(eventName);

        // Pin the key string in memory and pass a pointer through the user data
        GCHandle stringHandle = GCHandle.Alloc(key);
        currentEventInstance.setUserData(GCHandle.ToIntPtr(stringHandle));

        currentEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform)); //What I added                        
        currentEventInstance.setCallback(dialogueCallback);
        currentEventInstance.start();
        currentEventInstance.release();
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT DialogueEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

        // Retrieve the user data
        IntPtr stringPtr;
        instance.getUserData(out stringPtr);

        // Get the string object
        GCHandle stringHandle = GCHandle.FromIntPtr(stringPtr);
        String key = stringHandle.Target as String;

        switch (type)
        {
            case FMOD.Studio.EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND:
                {
                    FMOD.MODE soundMode = FMOD.MODE.LOOP_NORMAL | FMOD.MODE.CREATECOMPRESSEDSAMPLE | FMOD.MODE.NONBLOCKING;
                    var parameter = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));

                    if (key.Contains("."))
                    {
                        FMOD.Sound dialogueSound;
                        var soundResult = FMODUnity.RuntimeManager.CoreSystem.createSound(Application.streamingAssetsPath + "/" + key, soundMode, out dialogueSound);
                        if (soundResult == FMOD.RESULT.OK)
                        {
                            parameter.sound = dialogueSound.handle;
                            parameter.subsoundIndex = -1;
                            Marshal.StructureToPtr(parameter, parameterPtr, false);
                        }
                    }
                    else
                    {
                        FMOD.Studio.SOUND_INFO dialogueSoundInfo;
                        var keyResult = FMODUnity.RuntimeManager.StudioSystem.getSoundInfo(key, out dialogueSoundInfo);
                        if (keyResult != FMOD.RESULT.OK)
                        {
                            break;
                        }
                        FMOD.Sound dialogueSound;
                        var soundResult = FMODUnity.RuntimeManager.CoreSystem.createSound(dialogueSoundInfo.name_or_data, soundMode | dialogueSoundInfo.mode, ref dialogueSoundInfo.exinfo, out dialogueSound);
                        if (soundResult == FMOD.RESULT.OK)
                        {
                            parameter.sound = dialogueSound.handle;
                            parameter.subsoundIndex = dialogueSoundInfo.subsoundindex;
                            Marshal.StructureToPtr(parameter, parameterPtr, false);
                        }
                    }
                    break;
                }
            case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND:
                {
                    var parameter = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));
                    var sound = new FMOD.Sound(parameter.sound);
                    sound.release();

                    break;
                }
            case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROYED:
                {
                    // Now the event has been destroyed, unpin the string memory so it can be garbage collected
                    stringHandle.Free();

                    break;
                }
        }
        return FMOD.RESULT.OK;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayAudioFMOD("Voice_Line_NPC_1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayAudioFMOD("AudioTest_2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayAudioFMOD("Marge_VoiceLine_01");
        }
    }


    //public static void PlayOneShotAttached(Guid guid, GameObject gameObject)
    //{        
    //    var instance = CreateInstance(guid);
    //    AttachInstanceToGameObject(instance, gameObject.transform, gameObject.GetComponent());
    //    instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform)); //What I added
    //    instance.start();
    //    instance.release();
    //}
}