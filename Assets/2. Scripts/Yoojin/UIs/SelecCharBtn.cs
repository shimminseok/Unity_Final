using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelecCharBtn : MonoBehaviour
{
    [SerializeField]
    private PlayerUnitSO characterSO;

    public void OnCharacterClick()
    {
        // ĳ���� ����
        DeckSelectManager.Instance.SelectCharacter(characterSO);
    }
}
