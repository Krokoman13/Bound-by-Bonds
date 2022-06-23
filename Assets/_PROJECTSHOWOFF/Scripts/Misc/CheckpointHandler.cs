using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{
    string checkpointPrefName = "Checkpoint";

    public List<Checkpoint> checkpoints = new List<Checkpoint>();
    //public UnityEvent on

    public void SetCheckpoint(int newCheckpoint)
    {
        PlayerPrefs.SetInt(checkpointPrefName, newCheckpoint);
    }
}

public struct Checkpoint
{

}