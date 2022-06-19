using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] List<State> states = new List<State>();

    int _currentStateID = 0;
    public State CurrentState
    {
        get { return states[_currentStateID]; }
    }

    private void Start()
    {
        foreach (State state in states)
        {
            state.stateMachine = this;
            state.Start();
        }
    }

    private void Update()
    {
        foreach (State state in states)
        {
            state.Update();
        }
    }

    private void switchToState(int i)
    {
        _currentStateID = i;
        CurrentState.Load();
    }

    public bool TryToSwtichToState(int i)
    {
        if (_currentStateID == i || i < 0 || i >= states.Count) return false;
        if (states[i] == null) return false;

        switchToState(i);
        return true;
    }

    public bool TryToSwtichToState(State nextState)
    {
        int i = 0;

        foreach (State state in states)
        {
            if (state.Equals(nextState))
            {
                switchToState(i);
                return true;
            }
            i++;
        }

        Debug.LogWarning(nextState.name + ", was unavailible");
        return false;
    }

    public bool TryToSwtichToState(string nextName)
    {
        int i = 0;

        foreach (State state in states)
        {
            if (state.name == nextName)
            {
                switchToState(i);
                return true;
            }
            i++;
        }

        Debug.LogWarning(nextName + ", was unavailible");
        return false;
    }

    public bool TryToSwtichToState<T>()
    {
        int i = 0;

        foreach (State state in states)
        {
            if (state.GetType() == typeof(T))
            {
                switchToState(i);
                return true;
            }
            i++;
        }

        Debug.LogWarning(typeof(T).ToString() + ", was unavailible");
        return false;
    }

    public bool TryToSwtichToState<T>(string name)
    {
        int i = 0;

        foreach (State state in states)
        {
            if (state.GetType() == typeof(T) && state.name == name)
            {
                switchToState(i);
                return true;
            }
            i++;
        }

        Debug.LogWarning(typeof(T).ToString() + ", "+ name + ", was unavailible");
        return false;
    }
}
