using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [Header("이미지 / 코스트")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text cost;

    // 현재 버튼에 할당된 스킬 데이터
    private SkillData currentActive;
    private PassiveSO currentPassive;

    // 현재 버튼의 역할이 패시브인지 액티브인지
    private bool isPassive;



    // 액티브 스킬 버튼 데이터 세팅
    public void SetActiveSkill(SkillData active)
    {
        currentActive = active;
        isPassive = false;

        icon.sprite = active.skillIcon;
        cost.text = active.maxCost.ToString();
    }

    // 패시브 스킬 버튼 데이터 세팅
    public void SetPassiveSkill(PassiveSO passive)
    {
        currentPassive = passive;
        isPassive = true;

        // 현재 PassiveSO에는 스킬 아이콘이 없음! 추후 맞출 예정
        //icon.sprite = passive.skillIcon;
    }


    /// <summary>
    /// 이하 클릭 이벤트
    /// </summary>
    /// 

    public void OnClick()
    {
        if (isPassive)
        {
            DeckSelectManager.Instance.SelectPassiveSkill(currentPassive);
        }

        else
        {
            DeckSelectManager.Instance.SelectActiveSkill(currentActive);
        }
    }

}
