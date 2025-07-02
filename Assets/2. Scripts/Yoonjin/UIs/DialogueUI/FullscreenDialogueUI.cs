using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenDialogueUI : MonoBehaviour
{
    [Header("대사씬 구성")]
    [SerializeField] private TMP_Text nameText;         // 캐릭터 이름
    [SerializeField] private TMP_Text dialogueText;     // 대사 텍스트
    [SerializeField] private Image portraitImage;       // 초상화 이미지
    [SerializeField] private Image backgroundImage;     // 배경 이미지

    // 한 줄의 대사를 받아 화면에 출력한다
    public void Show (DialogueLine line)
    {
        nameText.text = line.characterName;
        dialogueText.text = line.dialogue;

        // TODO: 리소스 로드 방식에 따라 초상화/배경 이미지 설정
        // portraitImage.sprite = LoadPortrait(line.portraitKey);
        // backgroundImage.sprite = LoadBackground(line.backgroundKey);
    }

    public void OnClickNext()
    {
        DialogueController.Instance.Next();
    }
}
