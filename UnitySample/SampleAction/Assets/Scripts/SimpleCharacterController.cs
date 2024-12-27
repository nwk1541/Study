using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ�
    public float rotationSpeed = 720f; // ȸ�� �ӵ�

    private Vector3 forwardDirection; // ĳ������ ���� ����

    private void Update()
    {
        // �Է� �ޱ�
        float horizontal = Input.GetAxis("Horizontal"); // A, D �Ǵ� ��, ��
        float vertical = Input.GetAxis("Vertical"); // W, S �Ǵ� ��, ��

        // ȸ��: �¿� Ű�� �̿��� ȸ�� (�¿�� ȸ����)
        if (horizontal != 0)
        {
            // �¿� �̵��� ���� ȸ��
            Quaternion targetRotation = Quaternion.Euler(0, horizontal * rotationSpeed * Time.deltaTime, 0);
            transform.Rotate(targetRotation.eulerAngles);
        }

        // ����/���� (���Ʒ�)
        forwardDirection = transform.forward * vertical; // ĳ������ ���� ������ ���� ����/����

        // ���Ʒ� �������� �̵�
        if (vertical != 0)
        {
            transform.Translate(forwardDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
