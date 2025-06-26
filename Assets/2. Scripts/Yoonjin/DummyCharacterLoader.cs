using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추후 변경될 임시 보유 캐릭터 풀
public class DummyCharacterLoader : MonoBehaviour
{
    [Header("보유 중인 캐릭터 목록")]
    [SerializeField] private List<PlayerUnitSO> ownedCharacters;

    public List<PlayerUnitSO> OwnedCharacters => ownedCharacters;
}
