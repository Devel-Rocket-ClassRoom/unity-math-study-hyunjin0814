using UnityEngine;

public class DragAndDropManager : MonoBehaviour
{
    private Camera cam;
    public LayerMask ground;
    public LayerMask dragObject;
    public LayerMask dropZone;
    private bool isDraging = false;
    private DragObject dragginObject;

    void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, dragObject))
            {
                Debug.Log("Drag Start");
                isDraging = true;
                dragginObject = hitInfo.collider.GetComponent<DragObject>();
                dragginObject.DragStart();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isDraging)
            {
                if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, dropZone))
                {
                    dragginObject.DragEnd();
                }
                else
                {
                    dragginObject.Return();
                }

                isDraging = false;
                dragginObject = null;
            }
        }
        else if (isDraging)
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, ground))
            {
                dragginObject.transform.position = hitInfo.point;
            }
        }
        
    }
}
