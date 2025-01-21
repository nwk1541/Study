using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // 따라갈 대상
    public Vector3 offset = new Vector3(0, 10, -10); // 타겟과의 상대적인 위치
    public float followSpeed = 5f; // 카메라 이동 속도
    public float rotationSpeed = 5f; // 카메라 회전 속도

    private void LateUpdate()
    {
        if (target == null) return;

        // 카메라 위치를 타겟 위치 + 오프셋으로 설정
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // 카메라가 타겟을 바라보도록 회전
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}
