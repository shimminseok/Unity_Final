using System;
using UnityEngine;

[RequireComponent(typeof(SkillManager))]
public class SkillAnimationListener : MonoBehaviour
{
    public BaseSkillController skillController;
    public SkillData skillData;

    private void Awake()
    {
        if (skillController == null)
        {
            skillController = GetComponent<BaseSkillController>();
        }
    }

    //투사체 발사, 데미지 넣기, 디버프 넣기가 실제로 실행되는 이벤트
    public void UseSkillEvent()
    {
        // skillController.CurrentSkillData.skillType.UseSkill(skillController.sk);
    }

    public void EndSkillEvent()
    {
        skillController.EndTurn();
    }
}