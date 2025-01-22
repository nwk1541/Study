using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/AttackBase")]
public class AttackBase : ScriptableObject, ICombatAction
{
    [SerializeField]
    private int startupFrames = 15;
    [SerializeField]
    private int activeFrames = 5;
    [SerializeField]
    private int recoveryFrames = 15;
    [SerializeField]
    private int hitboxId = 0;

    public int GetStartupFrames()
    {
        return startupFrames;
    }

    public int GetActiveFrames()
    {
        return activeFrames;
    }

    public int GetRecoveryFrames()
    {
        return recoveryFrames;
    }

    public int GetHitboxId()
    {
        return hitboxId;
    }
}
