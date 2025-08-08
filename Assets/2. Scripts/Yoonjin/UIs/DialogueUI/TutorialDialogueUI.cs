using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDialogueUI : UIBase
{
    [Header("대사창 구성")]
    [SerializeField] private TMP_Text dialogueText; // 대사 텍스트

    public void SetDialogue(DialogueLine line)
    {
        dialogueText.text = line.dialogue;
    }

    public void OnClickNext()
    {
        DialogueController.Instance.Next();
    }
}