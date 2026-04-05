// =============================================================================
// BulletPattern.cs
// -----------------------------------------------------------------------------
// 원형 탄막 패턴 발사
// =============================================================================

using UnityEngine;
using TMPro;

public class BulletPattern : MonoBehaviour
{
    [Header("=== 탄막 설정 ===")]
    [Range(3, 36)]
    [SerializeField] private int bulletCount = 12;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float fireInterval = 1f;
    [SerializeField] private GameObject bulletPrefab;

    [Header("=== 회전 설정 ===")]
    [SerializeField] private float autoRotationSpeed = 0f;
    [SerializeField] private float bulletLifetime = 5f;

    [Header("=== UI 텍스트 ===")]
    [SerializeField] private TextMeshProUGUI uiText;

    [Header("=== 디버그 정보 (읽기 전용) ===")]
    [SerializeField] private float angleSpacing;
    [SerializeField] private float nextFireTime;
    [SerializeField] private float currentRotationOffset;

    private void Start()
    {
        // TODO
        angleSpacing = 360f / bulletCount;
        nextFireTime = Time.time + fireInterval;
    }

    private void Update()
    {
        currentRotationOffset = (Time.time * autoRotationSpeed) % 360f;

        if (Time.time >= nextFireTime)
        {
            FireBulletPattern();
            nextFireTime = Time.time + fireInterval;
        }

        UpdateUI();
    }

    private void FireBulletPattern()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            // TODO
            // 총알 개수만큼의 간격에 시간에 따라 스포너의 회전 오프셋을 더함
            float angleDegree = (i * angleSpacing + currentRotationOffset) % 360;   

            // Mathf.Cos, Mathf.Sin 사용을 위해 Degree값을 Radian으로 변환
            float angleRadian = angleDegree * Mathf.Deg2Rad;

            // X Z 평면에서 단위원의 원리인 (Cos(θ), Sin(θ))로 y값에 0을 넣어 바닥과 수평인 정규화된 벡터를 얻음
            Vector3 direction = new Vector3(Mathf.Cos(angleRadian), 0f, Mathf.Sin(angleRadian)).normalized;

            // 총알 프리팹들을 스포너에서 생성
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            // 위에서 구한 방향에 속도를 곱해서 발사함
            rb.linearVelocity = direction * bulletSpeed;

            Destroy(bullet, bulletLifetime);
        }
    }

    private void UpdateUI()
    {
        if (uiText == null) return;

        uiText.text = $"<b>[원형 탄막 패턴]</b>\n" +
                     $"발사 개수: {bulletCount}\n" +
                     $"각도 간격: {angleSpacing:F1}°\n" +
                     $"총알 속도: {bulletSpeed}u/s\n" +
                     $"회전 오프셋: {currentRotationOffset:F1}°\n" +
                     $"다음 발사: {(nextFireTime - Time.time):F2}초";
    }

    private void OnDrawGizmos()
    {
        if (!enabled) return;

        if (!Application.isPlaying) return;

        float spacing = 360f / bulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            float angleDegrees = (i * spacing + currentRotationOffset) % 360f;
            float angleRadians = angleDegrees * Mathf.Deg2Rad;

            Vector3 direction = new Vector3(
                Mathf.Cos(angleRadians),
                0f,
                Mathf.Sin(angleRadians)
            ).normalized;

            VectorGizmoHelper.DrawArrow(
                transform.position,
                transform.position + direction * 2f,
                new Color(1f, i / (float)bulletCount, 0f, 1f)
            );
        }

        VectorGizmoHelper.DrawCircleXZ(transform.position, 2f, Color.gray, 32);
    }
}
