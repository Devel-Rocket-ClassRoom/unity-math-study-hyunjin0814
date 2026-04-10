using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private float yOffset = 2.5f;     
    [SerializeField] private float returnSpeed = 5f;  
    [SerializeField] private LayerMask groundLayer;   
    [SerializeField] private string dropZoneTag = "DropZone";

    private Camera cam;
    private GameObject selectedObject;
    private Vector3 startPosition;

    private bool isReturning;
    private bool isDragging;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (isReturning)
        {
            HandleReturning();
            return;
        }

        if (Input.GetMouseButtonDown(0)) TryPickObject();
        if (Input.GetMouseButton(0) && isDragging) DragObject();
        if (Input.GetMouseButtonUp(0) && isDragging) DropObject();
    }

    // 태그로 드래그할 게임 오브젝트를 찾음
    void TryPickObject()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider.CompareTag("Draggable"))
            {
                selectedObject = hit.collider.gameObject;
                startPosition = selectedObject.transform.position;
                isDragging = true;
            }
        }
    }

    // 드래그중인 게임 오브젝트를 움직임
    void DragObject()
    {
        if (selectedObject == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        // 레이어를 통해서 바닥을 감지
        if (Physics.Raycast(ray, out RaycastHit hit, 200f, groundLayer))
        {
            Vector3 targetPos = hit.point;
            targetPos.y += yOffset; 
            selectedObject.transform.position = targetPos;
        }
    }

    void DropObject()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // 태그로 드롭존을 감지
            if (hit.collider.CompareTag(dropZoneTag))
            {
                selectedObject.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + yOffset, hit.transform.position.z);
                isDragging = false;
                selectedObject = null;
            }
            else
            {
                isReturning = true;
                isDragging = false;
            }
        }
    }

    void HandleReturning()
    { 
        Vector3 nextPos = Vector3.Lerp(
            selectedObject.transform.position,
            startPosition,
            Time.deltaTime * returnSpeed
        );

        // 돌아가야할 다음 좌표의 위에서 수직으로 Ray를 쏴서 Terrain의 굴곡에 맞게 y좌표 조정
        Vector3 rayOrigin = new Vector3(nextPos.x, 200f, nextPos.z);
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 200f, groundLayer))
        {
            nextPos.y = hit.point.y + yOffset;
        }

        selectedObject.transform.position = nextPos;

        // 도착했을 때 y좌표 차이로 인해 거리를 이용한 도착 여부 판단이 어려워 XZ 평면에서의 거리 차이로 계산
        // 기존 오브젝트의 위치를 정확하게 조정하면 단순하게 거리 차이만 계산해도 되지만 버그 방지
        Vector2 currentXZ = new Vector2(selectedObject.transform.position.x, selectedObject.transform.position.z);
        Vector2 targetXZ = new Vector2(startPosition.x, startPosition.z);

        if (Vector2.Distance(currentXZ, targetXZ) < 0.05f)
        {
            selectedObject.transform.position = startPosition;
            isReturning = false;
            selectedObject = null;
        }
    }
}

/* 최초 클릭에 큐브인지 체크
 * 클릭하면 드래그 상태로 변경, 돌아갈 위치 저장
 * 클릭 중에 그라운드를 raycast 해서 ground를 따라다니게 함
 * 드래그가 끝났을 때, 드롭존인지 ray를 통해 판단
 */ 