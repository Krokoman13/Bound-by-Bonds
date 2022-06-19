using UnityEngine.Events;
using UnityEngine;

public class FadeCanvasGroup : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup = null;
    [SerializeField] float fadeSpeed = 2;
    [SerializeField] bool isFading = false;
    public enum FadeState {ToOpaque, ToTransparent, NONE}
    FadeState currentState = FadeState.NONE;

    public UnityEvent onStartup = null;
    public UnityEvent onFadeToTransparent_Complete = null;
    public UnityEvent onFadeToOpaque_Complete = null;

    [SerializeField] bool ignoreFirstFadeEvent = false;    
    float targetAlpha = 0;

    float treshold = 0.01f;
    void Update()
    {
        if (!isFading)
            return;

        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup not assigned.", this);
            this.enabled = false;
            return;
        }


        if(targetAlpha > canvasGroup.alpha)        
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;        
        else        
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;        
        


        if (Mathf.Abs(targetAlpha - canvasGroup.alpha) < treshold)
        {
            isFading = false;
            InvokeEvent();            
        }
    }



    void InvokeEvent()
    {
        switch (currentState)
        {
            case FadeState.ToOpaque:
                onFadeToOpaque_Complete?.Invoke();
                break;
            case FadeState.ToTransparent:
                onFadeToTransparent_Complete?.Invoke();
                break;
            default:
                break;
        }
    }
    


    void StartAlpha(bool fadeToBlack)
    {
        if (fadeToBlack)
            targetAlpha = 1;
        else
            targetAlpha = 0;
    }

    [ContextMenu("Start Fade to Opaque")]
    public void StartFadeToOpaque()
    {
        StartAlpha(true);
        isFading = true;
        currentState = FadeState.ToOpaque;
    }

    [ContextMenu("Start Fade to Transparent")]
    public void StartFadeToTransparent()
    {
        isFading = true;
        StartAlpha(false);
        currentState = FadeState.ToTransparent;
    }


    [ContextMenu("Force to Opaque")]
    public void ForceToOpaque()
    {
        currentState = FadeState.ToOpaque;
        canvasGroup.alpha = 1;
        targetAlpha = 0;
        isFading = false;
    }

    [ContextMenu("Force to Transparent")]
    public void ForceToTransparent()
    {
        currentState = FadeState.ToTransparent;
        canvasGroup.alpha = 0;
        targetAlpha = 1;
        isFading = false;
    }


    public void StartFadeToOpaque(float delay)
    {
        Invoke("StartFadeToOpaque", delay);
    }

    public void StartFadeToTransparent(float delay)
    {
        Invoke("StartFadeToTransparent", delay);
    }

    void Awake()
    {
        onStartup?.Invoke();
    }

}