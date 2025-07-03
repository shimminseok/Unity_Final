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
        Debug.Log("풀스크린");
        nameText.text = line.characterName;
        dialogueText.text = line.dialogue;

        var portrait = DialogueResourceLoader.LoadPortrait(line.portraitKey);
        portraitImage.sprite = portrait;
        portraitImage.gameObject.SetActive(portrait != null);

        var background = DialogueResourceLoader.LoadBackground(line.backgroundKey);
        backgroundImage.sprite = background;
        backgroundImage.gameObject.SetActive(background != null);
        Debug.Log(line.backgroundKey);
        Debug.Log(background);

        gameObject.SetActive(true);
    }

    public void OnClickNext()
    {
        DialogueController.Instance.Next();
    }
}
