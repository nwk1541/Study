using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;
    public float lineDistance = 2f;
    public bool showForwardLine = true;

    private LineRenderer lineRenderer;

    void Start()
    {
        // LineRenderer 컴포넌트 가져오기
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRenderer 설정
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2; // 시작점과 끝점
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 기본 머티리얼
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    private void Update()
    {
        // 앞뒤 이동 (W, S)
        float moveInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * moveInput * moveSpeed * Time.deltaTime;
        transform.Translate(moveDirection, Space.World);

        // 좌우 회전 (A, D)
        float rotationInput = Input.GetAxis("Horizontal");
        float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotationAmount, 0f);

        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        if (!showForwardLine)
        {
            lineRenderer.enabled = false;
            return;
        }

        lineRenderer.enabled = true;

        // 선의 시작점과 끝점을 설정
        Vector3 startPoint = transform.position;
        Vector3 endPoint = startPoint + transform.forward * lineDistance;

        // LineRenderer로 선 그리기
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }
}
