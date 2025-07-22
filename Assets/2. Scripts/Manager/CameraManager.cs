using Cinemachine;
using UnityEngine;

public class CameraManager : SceneOnlySingleton<CameraManager>
{
    [SerializeField]public CinemachineBrain cinemachineBrain;
    [SerializeField]public VirtualCameraController mainCameraController;
    [SerializeField]public VirtualCameraController skillCameraController;

    public bool followNextIEffectProvider = true;
    public void ChangeFollowTarget(IEffectProvider target)
    {
        if (followNextIEffectProvider)
        {
            skillCameraController.Target = target.Collider.transform;
            skillCameraController.FocusOnUnit();
        }
        else
        {
            followNextIEffectProvider = true;
        }
    }

    public void ChangeMainCamera()
    {
        mainCameraController.ChangeCamera();
        skillCameraController.ThrowCamera();
        TimeLineManager.Instance.CurrentCameraController = mainCameraController;
    }
    
    public void ChangeSkillCamera()
    {
        skillCameraController.ChangeCamera();
        mainCameraController.ThrowCamera();     
        TimeLineManager.Instance.CurrentCameraController = skillCameraController;
    }
    
}
