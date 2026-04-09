using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 3f;
    private float rotateSpeed = 90f;
    private Rigidbody playerRigidBody;
    private PlayerInput playerInput;

    void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        float angle = playerInput.Rotate * rotateSpeed * Time.deltaTime;
        playerRigidBody.MoveRotation(playerRigidBody.rotation * Quaternion.Euler(0f, angle, 0f));

        Vector3 forward = transform.forward * playerInput.MoveX;
        Vector3 right = transform.right * playerInput.MoveZ;
        Vector3 direction = forward + right;
        if (direction.magnitude > 1f)
        {
            direction.Normalize();
        }
        playerRigidBody.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
    }
}
