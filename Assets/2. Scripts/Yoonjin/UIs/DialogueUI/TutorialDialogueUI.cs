using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDialogueUI : UIBase
{
    [Header("대사창 구성")]
    [SerializeField] private TMP_Text dialogueText; // 대사 텍스트

    public void Show(DialogueLine line)
    {
        Debug.Log("TutorialDialogueUI: Show");
        dialogueText.text = line.dialogue;

        Open();
    }

    public void OnClickNext()
    {
        DialogueController.Instance.Next();
    }
}
