using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스킬을 적용할 타겟을 정하는 역할을 해주는 클래스
public class TargetSelect
{

    public List<IDamageable> ConedShapeTargets(List<IDamageable> enemise, IDamageable target) // 리스트에서 타겟이 정해지면, 부채꼴 형태로 타겟들을 지정
    {
        List<IDamageable> targets = new List<IDamageable>();
        //List가 1차원일지 2차원 일지에 따라 로직이 바뀐다.
        foreach (IDamageable enemy in enemise)
        {
            
        }
        
        return targets;
        
    }
    
    



}
