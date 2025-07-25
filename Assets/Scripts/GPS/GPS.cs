using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour
{
    public GameObject car;
    public GameObject minimapMark;
    public GameObject indicatorMark;

    public Transform player;              // 내비게이션 중심 (플레이어)
    public Transform target;              // 타겟 오브젝트
    public RectTransform indicator;       // 방향 아이콘 (UI)
    public RectTransform minimapRect;     // 미니맵 UI Rect
    public Camera minimapCam;

    private bool IsIndicate = false;

    void Update()
    {
        GPSFollow();
        if (IsIndicate)
        {
            SetIndicator();
        }
    }

    void SetIndicator()
    {
        Vector3 viewportPos = minimapCam.WorldToViewportPoint(target.position);
        // 타겟이 미니맵 카메라 안에 있음
        bool Isinview = viewportPos.z > 0 &&
                        viewportPos.x >= 0 && viewportPos.x <= 1 &&
                        viewportPos.y >= 0 && viewportPos.y <= 1;
        if (Isinview)
        {
            indicatorMark.SetActive(false);
            IsIndicate = false;
            CarController.IsEndingStart = true;
        }
        else indicatorMark.SetActive(true);

        Vector3 offset = target.position - player.position;
        Vector2 dir = new Vector2(offset.x, offset.z).normalized;

        float halfWidth = minimapRect.rect.width / 2f;
        float halfHeight = minimapRect.rect.height / 2f;

        Vector2 rawPos = new Vector2(offset.x, offset.z);

        if (Mathf.Abs(rawPos.x) > halfWidth || Mathf.Abs(rawPos.y) > halfHeight)
        {
            Vector2 pos = GetEdgePosition(dir, halfWidth, halfHeight);
            indicator.anchoredPosition = pos;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            indicator.localRotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }
    Vector2 GetEdgePosition(Vector2 dir, float halfWidth, float halfHeight)
    {
        float slope = dir.y / dir.x;
        Vector2 pos;

        if (Mathf.Abs(dir.x) * halfHeight <= Mathf.Abs(dir.y) * halfWidth)
        {
            // y 경계 우선
            pos.y = Mathf.Sign(dir.y) * halfHeight;
            pos.x = pos.y / slope;
        }
        else
        {
            // x 경계 우선
            pos.x = Mathf.Sign(dir.x) * halfWidth;
            pos.y = pos.x * slope;
        }

        return pos;
    }
    void GPSFollow()
    {
        minimapMark.transform.SetParent(car.transform);
        minimapMark.transform.position = new Vector3(car.transform.position.x, car.transform.position.y + 20f, car.transform.position.z);
        minimapMark.transform.rotation = car.transform.rotation;
    }

    public void TurnIndicatorOn()
    {
        IsIndicate = true;
    }
}
