using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour
{
    // 카메라 위치랑 상관없이 UI로 띄우는 위치를 변경
    // 타겟이 스크린에 들어왔는지만 체크
    [SerializeField] private Transform target;
    private Camera cam;
    private Image uiImage;
    private RectTransform rectTransform;

    private int screenWidth;
    private int screenHeight;
    [SerializeField] private float margin = 30f;

    void Start()
    {
        cam = Camera.main;
        uiImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        Vector3 targetPos = cam.WorldToScreenPoint(target.position);

        bool inScreen = targetPos.z > 0 && targetPos.x >= 0 && targetPos.x <= screenWidth && targetPos.y >= 0 && targetPos.y <= screenHeight;

        if (inScreen)
        {
            // UI 이미지 비활성화
            uiImage.enabled = false;
        }
        else
        {
            // UI 이미지 활성화
            if (!uiImage.enabled)
            {
                uiImage.enabled = true;

            }
            // 뒤쪽 타겟이면 좌표 반전 처리
            if (targetPos.z < 0)
            {
                targetPos *= -1f;
            }
            // Mathf.Clamp로 화면 테두리에 고정
            float x = Mathf.Clamp(targetPos.x, margin, screenWidth - margin);
            float y = Mathf.Clamp(targetPos.y, margin, screenHeight - margin);
            // RectTransform 위치 업데이트
            rectTransform.position = new Vector3(x, y, 0f);
        }
    }
}
