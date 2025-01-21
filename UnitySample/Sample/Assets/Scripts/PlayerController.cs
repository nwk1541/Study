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
        // LineRenderer ������Ʈ ��������
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRenderer ����
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2; // �������� ����
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // �⺻ ��Ƽ����
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    private void Update()
    {
        // �յ� �̵� (W, S)
        float moveInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * moveInput * moveSpeed * Time.deltaTime;
        transform.Translate(moveDirection, Space.World);

        // �¿� ȸ�� (A, D)
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

        // ���� �������� ������ ����
        Vector3 startPoint = transform.position;
        Vector3 endPoint = startPoint + transform.forward * lineDistance;

        // LineRenderer�� �� �׸���
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }
}
