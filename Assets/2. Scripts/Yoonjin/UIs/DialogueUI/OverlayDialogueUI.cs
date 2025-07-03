using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverlayDialogueUI : UIBase
{
    [Header("대사창 구성")]
    [SerializeField] private TMP_Text nameText;     // 캐릭터 이름
    [SerializeField] private TMP_Text dialogueText; // 대사 텍스트
    [SerializeField] private Image portraitImage;   // 초상화 이미지

    public void Show (DialogueLine line)
    {
        nameText.text = line.characterName;
        dialogueText.text = line.dialogue;

        var sprite = DialogueResourceLoader.LoadPortrait(line.portraitKey);

        if (sprite == null)
            Debug.LogWarning($"초상화 스프라이트를 찾을 수 없습니다: {line.portraitKey}");

        portraitImage.sprite = sprite;
        portraitImage.gameObject.SetActive(sprite != null);
        Open();
    }

    public void OnClickNext()
    {
        DialogueController.Instance.Next();
    }
}
