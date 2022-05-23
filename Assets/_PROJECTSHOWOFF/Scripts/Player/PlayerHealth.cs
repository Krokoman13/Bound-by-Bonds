using UnityEngine.Events;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public UnityEvent onPlayerDeath = null;
    public void PlayerDeath()
    {
        onPlayerDeath?.Invoke();
        int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
    }
}