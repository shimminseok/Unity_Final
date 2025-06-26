using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeSkillSO", menuName = "ScriptableObjects/SKillType/Range", order = 0)]

public class RangeSkillSO : SkillTypeSO
{
    public string poolID;
    public override void UseSkill(BaseSkillController skillController)
    {
        GameObject projectile = ObjectPoolManager.Instance.GetObject(poolID);
        

    }
}
