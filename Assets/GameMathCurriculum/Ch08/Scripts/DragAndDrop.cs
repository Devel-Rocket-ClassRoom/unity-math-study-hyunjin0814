using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
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
        if (isReturning) return;

        if (Input.GetMouseButtonDown(0)) TryPickObject();
        if (Input.GetMouseButton(0) && isDragging) DragObject();
        if (Input.GetMouseButtonUp(0) && isDragging) DropObject();

        //Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        //{
        //    if (hit.collider.CompareTag("Draggable"))
        //    {
        //        selectedObject = hit.collider.gameObject;
        //        Debug.Log(selectedObject);
        //    }
        //}
    }

    void TryPickObject()
    {

    }

    void DragObject()
    {

    }

    void DropObject()
    {

    }
}
