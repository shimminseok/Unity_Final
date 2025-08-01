using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour,IPointerEnterHandler
{

    // 마우스 진입 시 실행될 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();
    }


    // 호버 사운드 재생 함수
    public void PlayHoverSound()
    {
        AudioManager.Instance.PlaySFX(SFXName.HoverUISound.ToString());
    }

   
}