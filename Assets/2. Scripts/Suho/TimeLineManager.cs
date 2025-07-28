using Cinemachine;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimeLineManager : SceneOnlySingleton<TimeLineManager>
{
    public PlayableDirector director;
    public SignalReceiver receiver;
    public bool isPlaying = false;
    public GameObject effectObject;
    private Animator effectAnimator;
    private IAttackable attacker;
    public VirtualCameraController CurrentCameraController{get;set;}

    protected override void Awake()
    {
        base.Awake();
        director = GetComponent<PlayableDirector>();
        effectAnimator = effectObject.GetComponent<Animator>();
        receiver = GetComponent<SignalReceiver>();
        director.stopped += StopTimeLine;
    }


    // private void Update()
    // {
    //     Debug.Log(effectObject.transform.position);
    // }

    public void StartVFXOnEffectObject()
    {
        foreach (var data in attacker.SkillController.CurrentSkillData.skillSo.effect.skillEffectDatas)
        {
            VFXController.VFXListPlayOnTransform(data.skillVFX,VFXType.Start,effectObject);
        }
    }
    
    public void OnAttackVFXOnEffectObject()
    {
        foreach (var data in attacker.SkillController.CurrentSkillData.skillSo.effect.skillEffectDatas)
        {
            VFXController.VFXListPlayOnTransform(data.skillVFX,VFXType.Hit,effectObject);
        }
    }

    public void OnAttackVFX()
    {
        var type = attacker.SkillController.CurrentSkillData.skillSo.skillType;
        type.PlayVFX(attacker, attacker.Target);
        
    }
    
    public void AffectSkillInTimeline()
    {
        attacker.SkillController.UseSkill();
    }

    public void ShakeCurrentCamera()
    {
        CurrentCameraController.ShakeCamera();
    }

    public void StopShakeCurrentCamera()
    {
        CurrentCameraController.StopShakeCamera();
    }

    public void InitializeTimeline()
    {
        Unit attackerUnit = attacker as Unit;
        Transform unitTransform = attackerUnit.transform;
        unitTransform.LookAt(attackerUnit.Target.Collider.bounds.center);
        unitTransform.eulerAngles = new Vector3(0,unitTransform.eulerAngles.y,0);
        var pos = unitTransform.position;
        var rot = unitTransform.rotation;
        CurrentCameraController.Unfocus();
        CameraManager.Instance.followNextIEffectProvider = false;
        CurrentCameraController.transform.position = pos;
        CurrentCameraController.transform.rotation = rot;
        effectObject.transform.position = pos;
        effectObject.transform.rotation = rot;
    }

    public void PlayTimeLine(CinemachineBrain brain,VirtualCameraController vCamController, IAttackable user)
    {
        attacker = user;
        director.playableAsset = attacker.SkillController.CurrentSkillData?.skillSo.skillTimeLine;
        isPlaying = true;
        if (director.playableAsset == null) return;
        CurrentCameraController = vCamController;
        CurrentCameraController.ChangeCamera();
        Unit attackerUnit = attacker as Unit;
        var timelineAsset = director.playableAsset as TimelineAsset;
        if (attacker.SkillController.CurrentSkillData.skillSo.isSkillScene)
        {
            InitializeTimeline();
        }
        foreach (var track in timelineAsset.GetOutputTracks())
        {
            if (track is CinemachineTrack cinemachineTrack)
            {
                var clips = cinemachineTrack.GetClips();
                foreach (var clip in clips)
                {
                    var shot = clip.asset as CinemachineShot;
                    if (shot == null)
                    {
                         continue;
                    }
                    if (shot.DisplayName == "SkillCamera")
                    {
                        director.SetReferenceValue(shot.VirtualCamera.exposedName, CameraManager.Instance.skillCameraController.vCam);
                    }
                    else if (shot.DisplayName == "MainCamera")
                    {
                        director.SetReferenceValue(shot.VirtualCamera.exposedName, CameraManager.Instance.mainCameraController.vCam);
                    }
                }

                // 뇌 (CinemachineBrain) 바인딩은 여전히 필요
                var output = timelineAsset.outputs.FirstOrDefault(
                    o => o.outputTargetType == typeof(CinemachineBrain)
                );
                if (output.sourceObject != null)
                {
                    director.SetGenericBinding(output.sourceObject, brain);
                }
            }
            
            if (track is SignalTrack)
            {
                director.SetGenericBinding(track, receiver);
            }
            
            
            if (track is AnimationTrack animationTrack)
            {
                // if (animationTrack.trackOffset == TrackOffset.ApplyTransformOffsets)
                // {
                //     foreach (var clip in animationTrack.GetClips())
                //     {
                //         var animPlayableAsset = clip.asset as AnimationPlayableAsset;
                //         if (animPlayableAsset != null)
                //         {
                //             animPlayableAsset.position = pos;
                //             animPlayableAsset.rotation = rot;
                //         }
                //     }
                // }

                if (animationTrack.name == "AttackerTrack")
                {

                    director.SetGenericBinding(animationTrack, attackerUnit?.Animator);

                }
                else if (animationTrack.name == "EffectTrack")
                {
                    director.SetGenericBinding(animationTrack, effectAnimator);
                }
                else if (animationTrack.name == "CameraAnimationTrack")
                {
                    director.SetGenericBinding(animationTrack, CurrentCameraController.CameraAnimator);
                }
                


            }
        }

        director.time = 0f;
        director.Evaluate();
        director.Play();
    }

    public void StopTimeLine(PlayableDirector pd)
    {
        director.Stop();
        isPlaying = false;
        if (CurrentCameraController != null)
        {
            CurrentCameraController.DefaultCamera();
        }
        CurrentCameraController = null;
        director.playableAsset = null;
        
    }
    
    
    
    
}
