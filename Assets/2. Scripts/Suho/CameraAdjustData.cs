using Cinemachine;
using UnityEngine;

public class CameraAdjustData
{
    public float DefaultFOV { get; private set; }
    public Transform DefaultTransform { get; private set; }
    public readonly float ZoomInFOVModifier  = 10f;
    public readonly float ZoomOutFOVModifier = 10f;
    public readonly float CameraShakeAmplitude  = 10f;
    public readonly float CameraShakeFrequency  = 10f;
    public readonly int MainPriority = 11;
    public readonly int SubPriority = 9;
    
    
    
    public CameraAdjustData(VirtualCameraController vCamController)
    {
        DefaultFOV = vCamController.vCam.m_Lens.FieldOfView;
        DefaultTransform = vCamController.transform;
        
    }
    
}
