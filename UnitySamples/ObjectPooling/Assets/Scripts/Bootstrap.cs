using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private void Awake()
    {
        PoolMgr.Inst.Open();
    }
}
