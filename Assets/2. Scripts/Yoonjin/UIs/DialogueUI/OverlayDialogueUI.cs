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
        // TODO: Resources나 Addressable에서 portraitKey로 스프라이트 로드
        // portraitImage.sprite = LoadPortrait(line.portraitKey);
        Open();
    }

    public void OnClickNext()
    {
        DialogueController.Instance.Next();
    }
}
