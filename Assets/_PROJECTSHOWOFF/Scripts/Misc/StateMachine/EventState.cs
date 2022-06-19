using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;


[CreateAssetMenu(fileName = "New EventState", menuName = "StateMachine/Create new EventState)")]
public class EventState : State
{
    public UnityEvent onStart;
    public UnityEvent onLoad;
    public UnityEvent onUnLoad;
    public UnityEvent onUpdate;

    protected virtual void start()
    { 
    }

    public override void Start()
    {
        onStart.Invoke();
        start();
    }

    protected virtual void load()
    {
    }

    public override void Load()
    {
        onLoad.Invoke();
        load();
    }

    protected virtual void update()
    {
    }

    public override void Update()
    {
        onUpdate.Invoke();
        update();
    }
    protected virtual void unLoad()
    {
    }

    public override void Unload()
    {
        onUnLoad.Invoke();
        unLoad();
    }
}
