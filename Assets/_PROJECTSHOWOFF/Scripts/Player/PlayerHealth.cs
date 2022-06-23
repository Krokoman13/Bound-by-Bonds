using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerHealth : PlayerComponent
{            
    public UnityEvent onPlayerDeath = null;            

    [ContextMenu("Force Player Death")]
    public void PlayerDeath()
    {
        onPlayerDeath?.Invoke();             
    }

    void Update()
    {
        //force death
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerDeath();            
        }
    }

    public static bool TryToKill(Transform player, int depth = 2)
    {
        PlayerHealth playerHealth;
        playerHealth = FindPlayerHealth(player, depth);

        if (playerHealth != null)
        {
            playerHealth.PlayerDeath();
            return true;
        }

        return false;
    }

    public static PlayerHealth FindPlayerHealth(Transform player, int depth)
    {
        PlayerHealth outp;
        player.TryGetComponent<PlayerHealth>(out outp);
        if (outp == null) outp = findPlayerHealthHigher(player, depth);
        if (outp == null) outp = findPlayerHealthDeeper(player, depth);

        return outp;
    }

    private static PlayerHealth findPlayerHealthDeeper(Transform player, int depth)
    {
        PlayerHealth outp = null;

        if (depth < 0) return outp;
        depth--;

        foreach (Transform child in player)
        {
            child.TryGetComponent<PlayerHealth>(out outp);
            if (outp != null) break;
            outp = FindPlayerHealth(child, depth);
        }

        return outp;
    }


    private static PlayerHealth findPlayerHealthHigher(Transform player, int depth)
    {
        PlayerHealth outp = null;
        if (player.parent == null) return outp;

        if (depth < 0) return outp;
        depth--;

        player.parent.TryGetComponent<PlayerHealth>(out outp);
        if (outp == null) outp = findPlayerHealthHigher(player.parent, depth);

        return outp;
    }
}