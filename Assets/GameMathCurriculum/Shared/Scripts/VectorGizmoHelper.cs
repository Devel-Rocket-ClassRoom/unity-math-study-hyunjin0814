// =============================================================================
// VectorGizmoHelper.cs
// -----------------------------------------------------------------------------
// 목적: Gizmos로 화살표, 부채꼴, 원 등을 그리는 공용 유틸리티 클래스
// 사용법: VectorGizmoHelper.DrawArrow(start, end, Color.red); 처럼 정적 메서드 호출
// 참고: 이후 챕터(Ch02~Ch08)에서도 계속 재사용하는 헬퍼입니다.
// =============================================================================

using UnityEngine;

/// <summary>
/// 벡터 시각화를 위한 Gizmos 헬퍼 클래스.
/// OnDrawGizmos() 또는 OnDrawGizmosSelected() 안에서 호출하세요.
/// </summary>
public static class VectorGizmoHelper
{
    // =========================================================================
    // 화살표 그리기
    // =========================================================================

    /// <summary>
    /// 시작점에서 끝점까지 화살표를 그립니다.
    /// </summary>
    /// <param name="start">화살표 시작 위치</param>
    /// <param name="end">화살표 끝 위치 (화살촉)</param>
    /// <param name="color">화살표 색상</param>
    /// <param name="arrowHeadLength">화살촉 길이 (기본값 0.25)</param>
    /// <param name="arrowHeadAngle">화살촉 벌어짐 각도 (기본값 20도)</param>
    public static void DrawArrow(Vector3 start, Vector3 end, Color color,
        float arrowHeadLength = 0.25f, float arrowHeadAngle = 20f)
    {
        // 이전 색상 백업 후 복원
        Color prevColor = Gizmos.color;
        Gizmos.color = color;

        // 몸통 직선
        Gizmos.DrawLine(start, end);

        // 화살촉 계산
        // 방향 벡터를 구한 뒤, 좌우로 일정 각도 회전시켜 두 개의 날개를 그린다
        Vector3 direction = (end - start).normalized;
        if (direction == Vector3.zero) { Gizmos.color = prevColor; return; }

        // 화살촉의 오른쪽 날개
        Vector3 right = Quaternion.LookRotation(direction)
            * Quaternion.Euler(0, 180 + arrowHeadAngle, 0)
            * Vector3.forward;
        // 화살촉의 왼쪽 날개
        Vector3 left = Quaternion.LookRotation(direction)
            * Quaternion.Euler(0, 180 - arrowHeadAngle, 0)
            * Vector3.forward;

        Gizmos.DrawLine(end, end + right * arrowHeadLength);
        Gizmos.DrawLine(end, end + left * arrowHeadLength);

        Gizmos.color = prevColor;
    }

    /// <summary>
    /// 특정 위치에서 방향 벡터를 화살표로 그립니다.
    /// </summary>
    /// <param name="origin">시작 위치</param>
    /// <param name="direction">방향 벡터 (크기가 화살표 길이)</param>
    /// <param name="color">색상</param>
    public static void DrawArrowFromDirection(Vector3 origin, Vector3 direction, Color color,
        float arrowHeadLength = 0.25f, float arrowHeadAngle = 20f)
    {
        DrawArrow(origin, origin + direction, color, arrowHeadLength, arrowHeadAngle);
    }

    // =========================================================================
    // 부채꼴(시야 범위) 그리기
    // =========================================================================

    /// <summary>
    /// XZ 평면 기준으로 부채꼴(시야 범위)을 그립니다.
    /// </summary>
    /// <param name="origin">부채꼴 중심</param>
    /// <param name="forward">정면 방향</param>
    /// <param name="halfAngle">반각(도) — 전체 시야각의 절반</param>
    /// <param name="radius">부채꼴 반지름</param>
    /// <param name="color">색상</param>
    /// <param name="segments">곡선 분할 수 (기본 20)</param>
    public static void DrawFOV(Vector3 origin, Vector3 forward, float halfAngle,
        float radius, Color color, int segments = 20)
    {
        Color prevColor = Gizmos.color;
        Gizmos.color = color;

        forward.y = 0f;
        if (forward == Vector3.zero) { Gizmos.color = prevColor; return; }
        forward.Normalize();

        // 시작 각도와 끝 각도
        float startAngle = -halfAngle;
        float endAngle = halfAngle;
        float step = (endAngle - startAngle) / segments;

        Vector3 prevPoint = origin + Quaternion.Euler(0, startAngle, 0) * forward * radius;

        // 왼쪽 경계선
        Gizmos.DrawLine(origin, prevPoint);

        // 호(arc) 그리기
        for (int i = 1; i <= segments; i++)
        {
            float angle = startAngle + step * i;
            Vector3 nextPoint = origin + Quaternion.Euler(0, angle, 0) * forward * radius;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }

        // 오른쪽 경계선
        Gizmos.DrawLine(origin, prevPoint);

        Gizmos.color = prevColor;
    }

    // =========================================================================
    // 원 그리기
    // =========================================================================

    /// <summary>
    /// XZ 평면에 원을 그립니다.
    /// </summary>
    public static void DrawCircleXZ(Vector3 center, float radius, Color color, int segments = 32)
    {
        Color prevColor = Gizmos.color;
        Gizmos.color = color;

        float step = 360f / segments;
        Vector3 prevPoint = center + new Vector3(radius, 0f, 0f);

        for (int i = 1; i <= segments; i++)
        {
            float angle = step * i * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }

        Gizmos.color = prevColor;
    }

    //public static void DrawCircleXZ(Vector3 center, float radius, Color color, int segments = 32)
    //{
    //    Color prevColor = Gizmos.color;
    //    Gizmos.color = color;

    //    float step = 360f / segments;
    //    Vector3 prevPoint = center + new Vector3(radius, 0f, 0f);

    //    for (int i = 1; i <= segments; i++)
    //    {
    //        float angle = step * i * Mathf.Deg2Rad;
    //        Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
    //        Gizmos.DrawLine(prevPoint, nextPoint);
    //        prevPoint = nextPoint;
    //    }

    //    Gizmos.color = prevColor;
    //}

    // =========================================================================
    // 텍스트 라벨 (씬 뷰 전용)
    // =========================================================================

#if UNITY_EDITOR
    /// <summary>
    /// 씬 뷰에서 특정 위치에 텍스트 라벨을 표시합니다. (에디터 전용)
    /// </summary>
    public static void DrawLabel(Vector3 position, string text, Color color)
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = color;
        style.fontSize = 12;
        style.fontStyle = FontStyle.Bold;
        UnityEditor.Handles.Label(position, text, style);
    }
#endif
}
