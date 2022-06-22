using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class State : ScriptableObject
{
    public StateMachine stateMachine;

    public abstract void Start();

    public abstract void Load();

    public abstract void Update();

    public abstract void Unload();

    public override bool Equals(object other)
    {
        if(GetType() != other.GetType()) return false;
        State otherState = (State)other;
        return otherState.name == name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), name);
    }
}