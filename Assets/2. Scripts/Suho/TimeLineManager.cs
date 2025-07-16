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
    public VirtualCameraController CurrentCameraController{get;set;}

    protected override void Awake()
    {
        base.Awake();
        director = GetComponent<PlayableDirector>();
        receiver = GetComponent<SignalReceiver>();
        director.stopped += StopTimeLine;
    }

    public void PlayTimeLine(CinemachineBrain brain,VirtualCameraController vCamController, IAttackable attacker)
    {
        director.playableAsset = attacker.SkillController.CurrentSkillData?.skillSo.skillTimeLine;
        if (director.playableAsset == null) return;
        isPlaying = true;
        CurrentCameraController = vCamController;
        CurrentCameraController.Camera.m_Priority = 11;
        var timelineAsset = director.playableAsset as TimelineAsset;
        foreach (var track in timelineAsset.GetOutputTracks())
        {
            if (track is CinemachineTrack)
            {
                director.SetGenericBinding(track, CurrentCameraController.Camera);
                var output = timelineAsset.outputs.FirstOrDefault(
                    o => o.outputTargetType == typeof(CinemachineBrain)
                );
                director.SetGenericBinding(output.sourceObject, brain);
            }
            
            if (track is SignalTrack)
            {
                director.SetGenericBinding(track, receiver);
            }
        }
        director.Play();
    }

    public void StopTimeLine(PlayableDirector pd)
    {
        director.Stop();
        isPlaying = false;
        if(CurrentCameraController != null)
        CurrentCameraController.Camera.m_Priority = 9;
        CurrentCameraController = null;
        director.playableAsset = null;
        
    }
    
    
    
    
}
