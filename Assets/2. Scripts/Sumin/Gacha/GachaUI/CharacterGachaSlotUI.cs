using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterGachaSlotUI : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private List<GameObject> unitTierStar;

    public void Initialize(PlayerUnitSO character)
    {
        characterImage.sprite = character.UnitIcon;
        characterNameText.text = character.UnitName;
        for (int i = 0; i < unitTierStar.Count; i++)
        {
            unitTierStar[i].SetActive(i <= (int)character.Tier);
        }
    }
}
