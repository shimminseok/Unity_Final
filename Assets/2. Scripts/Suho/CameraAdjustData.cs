using Cinemachine;
using UnityEngine;

public class CameraAdjustData
{
    public float DefaultFOV { get; private set; }
    public Transform DefaultTransform { get; private set; }
    public readonly float ZoomInFOVModifier  = 10f;
    public readonly float ZoomOutFOVModifier = 10f;
    public readonly float CameraShakeAmplitude  = 20f;
    public readonly float CameraShakeFrequency  = 20f;
    
    
    public CameraAdjustData(CinemachineVirtualCamera virtualCamera)
    {
        DefaultFOV = virtualCamera.m_Lens.FieldOfView;
        DefaultTransform = virtualCamera.transform;
        
    }
    
}
