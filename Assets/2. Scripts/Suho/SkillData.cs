
using System.Collections.Generic;
using UnityEngine;


/* 스킬 데이터 ScriptableObject
 * targets : 스킬을 적용받을 대상
 * mainAffect : 주 공격 대상이 받는 효과 => 리스트형태이므로 여러가지 효과를 동시에 줄 수 있음.
 * subAffect : 주 공격 대상 외에 유닛이 받는 효과 => ,,
 * reuseNum : 재사용가능횟수
 * cost : 쿨타임
 * skillIcon : 스킬아이콘
 * skillVFX : 시각적 스킬효과 ( 임시로 파티클 시스템으로 만들어 놓음 )
 * AnimationClip : 해당 스킬의 애니메이션 클립
 */
[CreateAssetMenu (fileName = "New SkillData", menuName = "ScriptableObjects/New SkillData")]
public class SkillData : ScriptableObject
{
    public SelectTargetType selectType;
    public List<StatusEffectData> mainAffect;
    public List<StatusEffectData> subAffect;
    public JobType jobType;
    public int reuseNumber;
    public int cost;
    public Sprite skillIcon;
    public ParticleSystem.Particle SkillVFX;
    public AnimationClip skillAnimation;
    
}
