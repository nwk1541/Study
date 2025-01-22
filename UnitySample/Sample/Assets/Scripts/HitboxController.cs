using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HitboxData
{
    public int id;
    public Collider collider;
}

public class HitboxController : MonoBehaviour
{
    public HitboxData[] hitboxes = null;

    public void EnableHitbox(int id)
    {
        if (id < 0)
            return;

        hitboxes[id].collider.enabled = true;
    }

    public void DisableHitbox(int id)
    {
        if (id < 0)
            return;

        hitboxes[id].collider.enabled = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log($"hit:{other.name}");

            CharacterBase charBase = other.GetComponent<CharacterBase>();
            if (charBase != null)
                charBase.Hit(1);
        }
    }
}
