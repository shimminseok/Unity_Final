using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterGachaSlotUI : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI characterTierText; // 나중에 프레임으로 변경

    public void Initialize(PlayerUnitSO character)
    {
        characterImage.sprite = character.UnitIcon;
        characterNameText.text = character.UnitName;
        characterTierText.text = $"{character.Tier}";
    }
}
