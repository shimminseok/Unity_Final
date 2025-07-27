using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverlayDialogueUI : UIBase
{
    [Header("대사창 구성")]
    [SerializeField] private TMP_Text nameText;     // 캐릭터 이름
    [SerializeField] private TMP_Text dialogueText; // 대사 텍스트
    [SerializeField] private Image leftPortraitImage;   // 초상화 왼쪽 이미지
    [SerializeField] private Image rightPortraitImage;  // 초상화 오른쪽 이미지
    [SerializeField] private CanvasGroup leftPortraitGroup;
    [SerializeField] private CanvasGroup rightPortraitGroup;

    public void Show (DialogueLine line)
    {
        nameText.text = line.characterName;
        dialogueText.text = line.dialogue;

        // 초상화 불러오기
        var leftSprite = DialogueResourceLoader.LoadPortrait(line.portraitLeft);
        var rightSprite = DialogueResourceLoader.LoadPortrait(line.portraitRight);

        leftPortraitImage.sprite = leftSprite;
        rightPortraitImage.sprite = rightSprite;

        leftPortraitImage.gameObject.SetActive(leftSprite != null);
        rightPortraitImage.gameObject.SetActive(rightSprite != null);

        // 말하는 캐릭터가 왼쪽인지 오른쪽인지 판별
        bool isLeftSpeaking = line.portraitLeft == line.portraitKey;
        bool isRightSpeaking = line.portraitRight == line.portraitKey;

        // 밝기 조절
        leftPortraitGroup.DOFade(isLeftSpeaking ? 1f : 0.5f, 0.25f);
        rightPortraitGroup.DOFade(isRightSpeaking ? 1f : 0.5f, 0.25f);

        Open();
    }

    public void OnClickNext()
    {
        DialogueController.Instance.Next();
    }

    public void Skip()
    {
        DialogueController.Instance.EndDialogue();
    }
}
