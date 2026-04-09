using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string moveXAxis = "Vertical";
    public static readonly string moveZAxis = "Horizontal";
    public static readonly string RotateAxis = "Rotation";
    public float MoveX { get; private set; }
    public float MoveZ { get; private set; }
    public float Rotate { get; private set; }

    void Update()
    {
        MoveX = Input.GetAxis(moveXAxis);
        MoveZ = Input.GetAxis(moveZAxis);
        Rotate = Input.GetAxis(RotateAxis);
    }
}
