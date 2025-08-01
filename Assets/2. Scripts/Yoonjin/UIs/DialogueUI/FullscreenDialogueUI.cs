using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FullscreenDialogueUI : MonoBehaviour
{
    [Header("대사씬 구성")]
    [SerializeField] private TMP_Text nameText;         // 캐릭터 이름
    [SerializeField] private TMP_Text dialogueText;     // 대사 텍스트
    [SerializeField] private Image leftPortraitImage;   // 초상화 왼쪽 이미지
    [SerializeField] private Image rightPortraitImage;  // 초상화 오른쪽 이미지
    [SerializeField] private Image backgroundImage;     // 배경 이미지
    [SerializeField] private CanvasGroup leftPortraitGroup;
    [SerializeField] private CanvasGroup rightPortraitGroup;

    // 한 줄의 대사를 받아 화면에 출력한다
    public void Show(DialogueLine line)
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
        leftPortraitImage.DOColor(isLeftSpeaking ? Color.white : new Color(0.3f, 0.3f, 0.3f), 0.25f);
        rightPortraitImage.DOColor(isRightSpeaking ? Color.white : new Color(0.3f, 0.3f, 0.3f), 0.25f);

        // 배경 설정
        var background = DialogueResourceLoader.LoadBackground(line.backgroundKey);
        backgroundImage.sprite = background;
        backgroundImage.gameObject.SetActive(background != null);

        gameObject.SetActive(true);
    }


    public void OnClickNext()
    {
        AudioManager.Instance.PlaySFX(SFXName.SwipeDialogueSound.ToString());
        DialogueController.Instance.Next();
    }

    public void Skip()
    {
        AudioManager.Instance.PlaySFX(SFXName.SwipeDialogueSound.ToString());
        DialogueController.Instance.EndDialogue();
    }
}
