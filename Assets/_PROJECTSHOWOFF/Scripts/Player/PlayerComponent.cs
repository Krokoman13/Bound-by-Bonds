using UnityEngine;

/// <summary>
/// Inherit from this and receive access to all player-related actions, such as it's update, start or death actions.
/// </summary>
public abstract class PlayerComponent : MonoBehaviour
{
    [HideInInspector] public Player subject = null;
    void Awake()
    {
        //Player-Components are always attached to player object
        TryGetComponent<Player>(out subject);

        //Prepare delegates
        subject.onPlayerStart += OnPlayerStart;
        subject.onPlayerUpdate += OnPlayerUpdate;
        subject.onPlayerFixedUpdate += OnPlayerFixedUpdate;
        subject.onPlayerDeath += OnPlayerDeath;
    }


    //Start will pass a player-reference, for easy linking back.
    public virtual void OnPlayerStart(Player pPlayer) { }
    public virtual void OnPlayerUpdate() { }
    public virtual void OnPlayerFixedUpdate() { }
    public virtual void OnPlayerDeath() { }
}