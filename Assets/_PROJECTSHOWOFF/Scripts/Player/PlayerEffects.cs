using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine;

[RequireComponent(typeof(PlayerStamina))]
public class PlayerEffects : PlayerComponent
{
    [SerializeField] CinemachineVirtualCamera vCam = null;
    CinemachineBasicMultiChannelPerlin perlin = null;
    PlayerFPSSprinting sprinting = null;
    [SerializeField] float sprintFov = 90f;
    private float baseFov = 60f;
    [SerializeField] float fovChangeSpeed = 8f;
    PlayerStamina stamina = null;

    [Header("PostProcessing Volume")]
    [SerializeField] Volume volume = null;
    VolumeProfile vp = null;

    Vignette vignette = null;
    //private float vignette_intensity = 0;
    [Header("Vignette")]
    [SerializeField] Vector2 vignette_intensity_Range = new Vector2(0.35f, 1f);
    //private float vignette_smoothness = 0;
    [SerializeField] Vector2 vignette_smoothness_Range = new Vector2(.3f, 1f);

    ColorAdjustments cAdjustments = null;
    //private float adjustments_saturation = 0;
    [Header("Color Adjustments")]
    [SerializeField] Vector2 adjustments_saturation_Range = new Vector2(-.85f, 0);

    [Header("CameraShake")]
    public NoiseSettings noise_medium = null;    

    public override void OnPlayerStart(Player pPlayer)
    {
        TryGetComponent<PlayerStamina>(out stamina);
        TryGetComponent<PlayerFPSSprinting>(out sprinting);

        perlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();        

        if (vCam != null)
            baseFov = vCam.m_Lens.FieldOfView;

        if (volume == null)
            return;
        vp = volume.profile;


        //VIGNETTE        
        vp.TryGet(out vignette);
        vp.TryGet(out cAdjustments);
    }

    public override void OnPlayerFixedUpdate()
    {
        //Camera based
        if (vCam != null) Effect_VCam();


        //Volume based
        if (volume == null)
            return;

        if (vignette != null) Effect_Vignette();
        if (cAdjustments != null) Effect_ColorAdjustments();
    }


    void Effect_VCam()
    {        
        if (sprinting.isSprinting && vCam.m_Lens.FieldOfView != sprintFov)
        {
            vCam.m_Lens.FieldOfView = Mathf.Lerp(vCam.m_Lens.FieldOfView, sprintFov, fovChangeSpeed * Time.fixedDeltaTime);
        }
        else if (vCam.m_Lens.FieldOfView != baseFov)
        {
            vCam.m_Lens.FieldOfView = Mathf.Lerp(vCam.m_Lens.FieldOfView, baseFov, fovChangeSpeed * Time.fixedDeltaTime);
        }
    }

    void Effect_Vignette()
    {
        float reversePercent = 1 - stamina.staminaPercent;

        //Intensity
        float diff = (vignette_intensity_Range.y - vignette_intensity_Range.x);
        float newValue = vignette_intensity_Range.x + (diff * reversePercent);
        vignette.intensity.Override(newValue);

        //Smoothness
        diff = (vignette_smoothness_Range.y - vignette_intensity_Range.x);
        newValue = vignette_smoothness_Range.x + (diff * reversePercent);
        vignette.smoothness.Override(newValue);
    }

    void Effect_ColorAdjustments()
    {
        //Intensity
        float diff = (adjustments_saturation_Range.y - adjustments_saturation_Range.x);
        float newValue = adjustments_saturation_Range.x + (diff * stamina.staminaPercent);
        cAdjustments.saturation.Override(newValue);
    }    

    void OnValidate()
    {
        if (vCam == null)
            vCam = FindObjectOfType<CinemachineVirtualCamera>();
    }
}