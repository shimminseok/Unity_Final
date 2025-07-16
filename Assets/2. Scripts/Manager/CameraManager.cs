using Cinemachine;
using UnityEngine;

public class CameraManager : SceneOnlySingleton<CameraManager>
{
    [SerializeField]public CinemachineBrain cinemachineBrain;
    [SerializeField]public VirtualCameraController mainCameraController;
    [SerializeField]public VirtualCameraController skillCameraController;

    public void ChangeFollowTarget(IEffectProvider target)
    {
        skillCameraController.Target = target.Collider.transform;
        skillCameraController.FocusOnUnit();
    }
    
}
