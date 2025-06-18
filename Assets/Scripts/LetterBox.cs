using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LetterBox : MonoBehaviour
{
    public RectTransform topBar;
    public RectTransform bottomBar;

    public float animationDuration = 0.5f;
    public float targetOffset = 100f; // 레터박스가 들어올 거리
    public static bool IsLetterBoxOut = false;

    void Start()
    {
        // 초기 위치: 화면 밖으로 이동
        topBar.anchoredPosition = new Vector2(0, targetOffset);
        bottomBar.anchoredPosition = new Vector2(0, -targetOffset);

        // 애니메이션으로 원래 자리로 이동 (anchoredPosition이 (0,0)일 경우)
        topBar.DOAnchorPosY(0, animationDuration).SetEase(Ease.OutQuad);
        bottomBar.DOAnchorPosY(0, animationDuration).SetEase(Ease.OutQuad);
    }

    void Update()
    {
        if (IsLetterBoxOut)
        {
            IsLetterBoxOut = false;
            HideLetterbox();
        }
    }

    void HideLetterbox()
    {
        float animationDuration = 0.5f;
        float targetOffset = 100f;

        // 위쪽 레터박스는 위로 이동
        topBar.DOAnchorPosY(targetOffset, animationDuration).SetEase(Ease.InQuad);

        // 아래쪽 레터박스는 아래로 이동
        bottomBar.DOAnchorPosY(-targetOffset, animationDuration).SetEase(Ease.InQuad);
        IsLetterBoxOut = false;
    }

}
