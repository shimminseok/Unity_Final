using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStageSO", menuName = "ScriptableObjects/Stage/StageSO", order = 0)]
public class StageSO : ScriptableObject
{
    public int ID;
    public int MonsterLevel;
    public List<EnemyUnitSO> Monsters;
    public MonsterIncreaseSO MonsterIncrease;

    [Header("스테이지 전/후에 출력되는 대사")]
    public string beforeDialogueKey;
    public string afterDialogueKey;

    public bool HasBeforeDialogue => !string.IsNullOrEmpty(beforeDialogueKey);
    public bool HasAfterDialogue => !string.IsNullOrEmpty(afterDialogueKey);
}