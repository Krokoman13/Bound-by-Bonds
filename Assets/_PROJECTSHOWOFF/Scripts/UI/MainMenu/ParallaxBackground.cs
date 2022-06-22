using UnityEngine;
using UnityEngine.Events;

public class ParallaxBackground : MonoBehaviour
{
    Transform t = null;
    Vector2 mousePos = Vector2.zero;

    public enum ParallaxType {MouseInput, Directional}
    public ParallaxType parallaxType = ParallaxType.MouseInput;
    public Vector3 parallaxDir = Vector3.right;
    Vector3 finalPos = Vector3.zero;
    float finalposCheckTreshold = 0.001f;


    [SerializeField] Camera cam = null;
    [SerializeField] float parallaxSpeed = 0.4f;
    [SerializeField] float parallaxIntensity = 1.5f;
    Vector3 startPos = Vector3.zero;    

    [SerializeField]UnityEvent onParallaxFinished = null;
    

    void FixedUpdate()
    {
        switch (parallaxType)
        {
            case ParallaxType.MouseInput:
                DoParallax_MouseInput();
                break;
            case ParallaxType.Directional:
                DoParallax_Directional();
                break;
            default:
                break;
        }
    }

    void DoParallax_MouseInput()
    {
        mousePos = Input.mousePosition;
        Vector3 pos = cam.ScreenToWorldPoint(mousePos);
        pos.z = 0;

        Vector3 dir = -(pos - startPos);
        Vector3 targetPoint = pos + (dir * parallaxIntensity);
        targetPoint.z = pos.z;
        t.position = Vector3.Lerp(startPos, targetPoint, Time.fixedDeltaTime * parallaxSpeed);
    }

    

    void DoParallax_Directional()
    {        
        t.position += parallaxDir * Time.fixedDeltaTime * parallaxSpeed;
                        
        float dis = Vector3.Distance(t.position, finalPos);        
        if (dis < finalposCheckTreshold)
        {
            t.position = finalPos;
            onParallaxFinished?.Invoke();
        }
    }

    void Start()
    {
        t = transform;        
        startPos = t.position;



        Vector3 pos = t.position;
        pos.z = 0;
        finalPos = pos + (parallaxDir * parallaxIntensity);
        finalPos.z = pos.z;
    }
}