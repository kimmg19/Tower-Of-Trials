using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale; // 버튼의 원래 크기를 저장할 변수
    public float scaleFactor = 1.2f; // 커질 크기 비율
    public float scaleDuration = 0.2f; // 커지거나 작아지는 애니메이션 시간

    void Start()
    {
        // 시작할 때 원래 크기를 저장해둠
        originalScale = transform.localScale;
    }

    // 마우스 포인터가 버튼 위로 갔을 때 호출되는 메서드
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 버튼을 커지게 하는 애니메이션
        LeanTween.scale(gameObject, originalScale * scaleFactor, scaleDuration).setEaseOutBack();
    }

    // 마우스 포인터가 버튼을 벗어났을 때 호출되는 메서드
    public void OnPointerExit(PointerEventData eventData)
    {
        // 버튼을 원래 크기로 돌리는 애니메이션
        LeanTween.scale(gameObject, originalScale, scaleDuration).setEaseOutBack();
    }
}
