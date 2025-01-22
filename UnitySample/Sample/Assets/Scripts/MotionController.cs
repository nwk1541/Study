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

        // Startup �ܰ�: �ణ �ڷ� �غ�
        targetTransform.DOLocalMoveZ(-0.5f, startupTime).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // Active �ܰ�: ������ ���� (��ġ��)
            targetTransform.DOLocalMoveZ(slamDistance, activeTime).SetEase(Ease.InQuad).OnComplete(() =>
            {
                // Recovery �ܰ�: ���� ��ġ�� ����
                targetTransform.DOLocalMoveZ(0f, recoveryTime).SetEase(Ease.OutQuad);
            });
        });
    }
}
