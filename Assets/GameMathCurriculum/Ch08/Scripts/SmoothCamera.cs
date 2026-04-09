using UnityEngine;

//3. 타겟 위치 + offset 방향 × 줌 거리로 카메라 목표 위치 계산
//4. `Vector3.SmoothDamp`로 카메라 위치를 부드럽게 이동
//5. `Quaternion.Slerp`로 카메라 회전을 목표 방향에 부드럽게 보간


public class SmoothCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 3f, -5f);
    [SerializeField] private float positionSmoothTime = 0.3f;
    [SerializeField] private float rotationSmoothSpeed = 5f;

    private Vector3 positionVelocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 targetPos = target.TransformPoint(offset);
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref positionVelocity,
            positionSmoothTime
            );

        Vector3 look = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(look);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSmoothSpeed * Time.deltaTime
            );
    }
}
