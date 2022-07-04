using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float interval = 0.5f;
    private float currTime = 0f;

    private void Update()
    {
        currTime += Time.deltaTime;

        if (currTime >= interval)
        {
            currTime = 0f;
            ObjectPool.Instance.SpawnFromPool<Cube>("Cube");
        }
    }
}
