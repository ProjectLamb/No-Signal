using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LetterBox : MonoBehaviour
{
    public RectTransform topBar;
    public RectTransform bottomBar;

    private Vector2 orgTopVec;
    private Vector2 orgBotVec;

    public float animationDuration = 0.5f;
    public static bool IsLetterBoxOut = false;

    void Start()
    {
        // 초기 위치 저장
        orgTopVec = topBar.anchoredPosition;
        orgBotVec = bottomBar.anchoredPosition;

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
        topBar.DOAnchorPos(orgTopVec, animationDuration).SetEase(Ease.InQuad);

        // 아래쪽 레터박스는 아래로 이동
        bottomBar.DOAnchorPos(orgBotVec, animationDuration).SetEase(Ease.InQuad);
        IsLetterBoxOut = false;
    }

}
