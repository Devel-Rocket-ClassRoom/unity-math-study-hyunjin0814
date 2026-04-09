// =============================================================================
// Assignment_SplineConveyor.cs
// -----------------------------------------------------------------------------
// Catmull-Rom 스플라인 위를 여러 박스가 일정 간격으로 순환 이동하는 컨베이어.
//       AnimationCurve 로 구간별 속도를 조절한다.
// =============================================================================

using UnityEngine;
using TMPro;

public class Assignment_SplineConveyor : MonoBehaviour
{
    [Header("=== 스플라인 경로 ===")]
    [SerializeField] private Transform[] waypoints;

    [Header("=== 컨베이어 박스 ===")]
    [SerializeField] private Transform[] boxes;

    [Header("=== 속도 프로파일 ===")]
    [Tooltip("x축: globalT(0~1), y축: 속도 배율. 기본 Linear는 일정 속도")]
    [SerializeField] private AnimationCurve speedCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

    [Range(1f, 20f)] [SerializeField] private float cycleDuration = 6f;

    [Header("=== 시각화 ===")]
    [Range(10, 100)] [SerializeField] private int splineResolution = 50;

    [Header("=== UI 연결 ===")]
    [SerializeField] private TMP_Text uiInfoText;

    [Header("=== 디버그 정보 (읽기 전용) ===")]
    [SerializeField] private float globalT;
    [SerializeField] private float currentSpeedMultiplier;

    private void Update()
    {
        // TODO

        // 현재 진행률에 맞는 speedCurve 가중치를 가져옴
        // speed가 0에 수렴할 때 멈추는 현상 발생 가능성 있음
        float speed = speedCurve.Evaluate(globalT);
        globalT += (1f / cycleDuration) * speed * Time.deltaTime;
        globalT = Mathf.Repeat(globalT, 1f);

        for (int i = 0; i < boxes.Length; i++)
        {
            float spacing = 1f / boxes.Length;
            float boxT = globalT + (i * spacing);
            boxT = Mathf.Repeat(boxT, 1f);

            Vector3 targetPos = EvaluateSpline(waypoints, boxT);
            boxes[i].position = targetPos;
        }

        currentSpeedMultiplier = speedCurve.Evaluate(globalT);
        UpdateUI();
    }

    private Vector3 EvaluateSpline(Transform[] pts, float t)
    {
        // 웨이포인트가 5개면 구간(길)은 4개
        int segmentCount = pts.Length - 1;

        // 현재 진행률이 어느 구간에 속하는지 계산 (예: 1.5는 두 번째 구간을 지나는 중임. 0~1: 첫 번째 구간, 1~2: 두 번째 구간)
        float scaledT = t * segmentCount;

        // 현재 구간에서 소수점을 버린 값 (예: 1.5의 값을 1로 변환)
        int segment = Mathf.Clamp((int)scaledT, 0, segmentCount - 1);

        // (1.5 - 1 = 0.5, 현재 두 번째 구간을 지나는데, 그 구간에서의 진행률이 0.5임)
        float localT = scaledT - segment;

        Vector3 p0 = pts[Mathf.Max(0, segment - 1)].position;                   // 현재 구간 이전의 점
        Vector3 p1 = pts[segment].position;                                     // 현재 구간 시작점
        Vector3 p2 = pts[Mathf.Min(pts.Length - 1, segment + 1)].position;      // 현재 구간의 끝점
        Vector3 p3 = pts[Mathf.Min(pts.Length - 1, segment + 2)].position;      // 다음 구간의 끝점

        return CatmullRom(p0, p1, p2, p3, localT);
    }

    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t, t3 = t2 * t;
        return 0.5f * (
            2f * p1 +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3
        );
    }

    private void UpdateUI()
    {
        if (uiInfoText == null) return;

        int boxCount = boxes != null ? boxes.Length : 0;
        int wpCount = waypoints != null ? waypoints.Length : 0;

        uiInfoText.text = $"[Assignment_SplineConveyor] 스플라인 컨베이어\n"
                        + $"웨이포인트: {wpCount}개 / 박스: {boxCount}개\n"
                        + $"globalT: {globalT:F2}\n"
                        + $"속도 배율: {currentSpeedMultiplier:F2}×\n"
                        + $"주기: {cycleDuration:F1}초\n"
                        + $"\n<color=yellow>AnimationCurve로 구간별 속도 조절</color>";
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!enabled) return;
        if (waypoints == null || waypoints.Length < 2) return;

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(waypoints[i].position, 0.2f);
        }

        Gizmos.color = Color.cyan;
        Vector3 prev = EvaluateSpline(waypoints, 0f);
        for (int i = 1; i <= splineResolution; i++)
        {
            float t = i / (float)splineResolution;
            Vector3 curr = EvaluateSpline(waypoints, t);
            Gizmos.DrawLine(prev, curr);
            prev = curr;
        }
    }
#endif
}
