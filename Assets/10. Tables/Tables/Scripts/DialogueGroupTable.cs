using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueGroupTable", menuName = "Table/DialogueGroupTable", order = 1)]
public class DialogueGroupTable : BaseTable<string, DialogueGroupSO>
{
    // 이 경로에 위치한 DialogueGroupSO들을 자동으로 찾아 등록
    protected override string[] DataPath => new[] { "Assets/10. Tables/Dialogue" };

    // 테이블 초기화: 데이터 리스트를 Dictionary로 정리
    public override void CreateTable()
    {
        Type = GetType();
        foreach (DialogueGroupSO data in dataList)
        {
            if (!DataDic.ContainsKey(data.groupKey))
                DataDic[data.groupKey] = data;
        }
    }
}

