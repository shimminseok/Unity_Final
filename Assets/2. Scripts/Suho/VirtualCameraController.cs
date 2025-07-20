using Cinemachine;
using System;
using UnityEngine;

public class VirtualCameraController : MonoBehaviour
{
   [SerializeField]public CinemachineVirtualCamera Camera;
   private CameraAdjustData cameraAdjustData;
   private CinemachineBasicMultiChannelPerlin perlin;
   public Transform Target { get; set; }
   

   private void Awake()
   {
       Camera = GetComponent<CinemachineVirtualCamera>();
       cameraAdjustData = new CameraAdjustData(Camera);
       perlin = Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
   }

   public void FocusOnUnit()
   {
       Camera.LookAt = Target;
       // Camera.Follow = Target;
   }

   public void Unfocus()
   {
       Camera.LookAt = null;
       // Camera.Follow = null;
       Camera.transform.rotation = cameraAdjustData.DefaultTransform.rotation;
   }

   public void ZoomInCamera()
   {
       Camera.m_Lens.FieldOfView -= cameraAdjustData.ZoomInFOVModifier;
   }

   public void ZoomOutCamera()
   {
       Camera.m_Lens.FieldOfView += cameraAdjustData.ZoomOutFOVModifier;
   }

   public void DefaultCamera()
   {
       Camera.m_Lens.FieldOfView = cameraAdjustData.DefaultFOV;
       Camera.transform.position = cameraAdjustData.DefaultTransform.position;
       Camera.transform.rotation = cameraAdjustData.DefaultTransform.rotation;
   }

   public void ShakeCamera()
   {
       perlin.m_AmplitudeGain = cameraAdjustData.CameraShakeAmplitude;
       perlin.m_FrequencyGain = cameraAdjustData.CameraShakeFrequency;
   }

   public void StopShakeCamera()
   {
       perlin.m_AmplitudeGain = 0f;
       perlin.m_FrequencyGain = 0f;
   }
   
   
   
}

