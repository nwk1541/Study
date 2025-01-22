using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    public MotionController motionController;
    public HitboxController hitboxController;
    public AttackBase[] attackActions;

    private YieldInstruction waitForEndOfFrame = new WaitForEndOfFrame();
    private bool isAttacking = false;
    private int currentFrame = 0;

    #region MonoBehaviour
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isAttacking)
        {
            PerformAction(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && !isAttacking)
        {
            PerformAction(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && !isAttacking)
        {
            PerformAction(2);
        }
    }
    #endregion

    private void PerformAction(int actionIndex)
    {
        if (isAttacking)
            return;

        currentFrame = 0;
        ICombatAction action = attackActions[actionIndex];

        StartCoroutine(CoExecute(action));
    }

    private IEnumerator CoExecute(ICombatAction action)
    {
        isAttacking = true;
        Debug.Log($"Attack Started, frame:{currentFrame}");

        // startup
        motionController.PerformBodySlam(transform, action);
        for (int idx = 0; idx < action.GetStartupFrames(); idx++)
        {
            currentFrame++;
            yield return waitForEndOfFrame;
        }

        // active
        hitboxController.EnableHitbox(action.GetHitboxId());
        for (int idx = 0; idx < action.GetActiveFrames(); idx++)
        {
            currentFrame++;
            yield return waitForEndOfFrame;
        }
        hitboxController.DisableHitbox(action.GetHitboxId());

        for (int idx = 0; idx < action.GetRecoveryFrames(); idx++)
        {
            currentFrame++;
            yield return waitForEndOfFrame;
        }

        isAttacking = false;
        Debug.Log($"Attack Finished, frame:{currentFrame}");
    }
}
