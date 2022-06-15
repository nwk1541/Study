#if UNITY_EDITOR
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DrawBoxCollider2D : MonoBehaviour
{
    // LineRenderer « ø‰
    // Loop : true
    // Positions - Size : 4
    // Width : √Î«‚≤Ø
    // Color : √Î«‚≤Ø
    // Materials : Default-Line
    private LineRenderer lineRenderer;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        //lineRenderer = Instantiate(Resources.Load<LineRenderer>("LineRenderer"));
        //lineRenderer.transform.SetParent(transform);
        //lineRenderer.transform.localPosition = Vector3.zero;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Vector3[] positions = new Vector3[4];

        positions[0] = transform.TransformPoint(new Vector3((boxCollider.size.x / 2.0f) + boxCollider.offset.x, (boxCollider.size.y / 2.0f) + boxCollider.offset.y, 0));
        positions[1] = transform.TransformPoint(new Vector3((-boxCollider.size.x / 2.0f) + boxCollider.offset.x, boxCollider.size.y / 2.0f + boxCollider.offset.y, 0));
        positions[2] = transform.TransformPoint(new Vector3((-boxCollider.size.x / 2.0f) + boxCollider.offset.x, -boxCollider.size.y / 2.0f + boxCollider.offset.y, 0));
        positions[3] = transform.TransformPoint(new Vector3((boxCollider.size.x / 2.0f) + boxCollider.offset.x, -boxCollider.size.y / 2.0f + boxCollider.offset.y, 0));

        //lineRenderer.SetPositions(positions);
    }
}
#endif