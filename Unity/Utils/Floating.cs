using UnityEngine;

public class Floating : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float moveThreshold = 0.2f;
    public float moveSpeed = 0.5f;

    private float baseY;

    private void Start()
    {
        baseY = transform.position.y;
        amplitude = Random.Range(-0.5f, 0.5f);
    }

    private void Update()
    {
        Vector3 tempPos = transform.position;
        if (tempPos.y > baseY + moveThreshold)
            SetAmplitude(false);
        else if (tempPos.y < baseY - moveThreshold)
            SetAmplitude(true);

        tempPos.y += Mathf.Sin(Time.deltaTime * Mathf.PI * moveSpeed) * amplitude;

        transform.position = tempPos;
    }

    private void SetAmplitude(bool sign)
    {
        amplitude = Random.Range(sign ? 0f : -0.5f, sign ? 0.5f : 0f);
    }
}
