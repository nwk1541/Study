using UnityEngine;

public class Cube : MonoBehaviour, IPooledObject
{
    public float moveSpeed = 3.5f;
    public float lifeTime = 5f;

    private Vector2 moveDir;
    private float currTime;

    private void Update()
    {
        currTime += Time.deltaTime;
        if (currTime >= lifeTime)
            ObjectPool.Instance.ReturnToPool(gameObject);

        Vector2 pos = transform.position;
        float speed = moveSpeed * Time.deltaTime;

        pos.x += moveDir.x * speed;
        pos.y += moveDir.y * speed;
        transform.position = pos;
    }

    public void OnSpawn()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        moveDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        currTime = 0f;
    }
}
