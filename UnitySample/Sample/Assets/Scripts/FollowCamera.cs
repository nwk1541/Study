using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // ���� ���
    public Vector3 offset = new Vector3(0, 10, -10); // Ÿ�ٰ��� ������� ��ġ
    public float followSpeed = 5f; // ī�޶� �̵� �ӵ�
    public float rotationSpeed = 5f; // ī�޶� ȸ�� �ӵ�

    private void LateUpdate()
    {
        if (target == null) return;

        // ī�޶� ��ġ�� Ÿ�� ��ġ + ���������� ����
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // ī�޶� Ÿ���� �ٶ󺸵��� ȸ��
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}
