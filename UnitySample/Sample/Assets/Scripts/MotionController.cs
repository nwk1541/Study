using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionController : MonoBehaviour
{
    public void PerformBodySlam(Transform targetTransform, ICombatAction action, float slamDistance = 2f)
    {
        float fps = (float)Environment.TargetFrame;
        float startupTime = action.GetStartupFrames() / fps;
        float activeTime = action.GetActiveFrames() / fps;
        float recoveryTime = action.GetRecoveryFrames() / fps;

        // Startup 단계: 약간 뒤로 준비
        targetTransform.DOLocalMoveZ(-0.5f, startupTime).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // Active 단계: 앞으로 돌진 (박치기)
            targetTransform.DOLocalMoveZ(slamDistance, activeTime).SetEase(Ease.InQuad).OnComplete(() =>
            {
                // Recovery 단계: 원래 위치로 복귀
                targetTransform.DOLocalMoveZ(0f, recoveryTime).SetEase(Ease.OutQuad);
            });
        });
    }
}
