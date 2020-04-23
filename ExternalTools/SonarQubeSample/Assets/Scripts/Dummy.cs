using UnityEngine;

public class Dummy : MonoBehaviour
{
    public float speed = 5f;
    public float directionChangeInterval = 1f;
    public float maxHeadingChange = 30f;
    public float elasedTimeMax = 0.1f;

    CharacterController controller;
    Vector3 targetRotation;
    float heading;
    float elapsed;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        heading = Random.Range(0, 360f);
        transform.eulerAngles = new Vector3(0f, heading, 0f);
    }

    private void Update()
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
        Vector3 dir = transform.TransformDirection(Vector3.forward);
        controller.SimpleMove(dir * speed);

        elapsed += Time.deltaTime;
        if(elapsed > elasedTimeMax)
        {
            NewRoute();
            elapsed = 0f;
        }
    }

    void NewRoute()
    {
        float floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
        float ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
        heading = Random.Range(floor, ceil);
        targetRotation = new Vector3(0f, heading, 0f);
    }
}