using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    public float rotationSpeed = 720f; // 회전 속도

    private Vector3 forwardDirection; // 캐릭터의 전방 방향

    private void Update()
    {
        // 입력 받기
        float horizontal = Input.GetAxis("Horizontal"); // A, D 또는 ←, →
        float vertical = Input.GetAxis("Vertical"); // W, S 또는 ↑, ↓

        // 회전: 좌우 키를 이용한 회전 (좌우는 회전만)
        if (horizontal != 0)
        {
            // 좌우 이동에 따른 회전
            Quaternion targetRotation = Quaternion.Euler(0, horizontal * rotationSpeed * Time.deltaTime, 0);
            transform.Rotate(targetRotation.eulerAngles);
        }

        // 직진/후진 (위아래)
        forwardDirection = transform.forward * vertical; // 캐릭터의 전방 방향을 따라 직진/후진

        // 위아래 방향으로 이동
        if (vertical != 0)
        {
            transform.Translate(forwardDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
