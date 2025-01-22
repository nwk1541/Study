using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public static int TargetFrame { get; private set; }

    public int targetFrame = 60;

    private void Awake()
    {
        TargetFrame = targetFrame;

        Application.targetFrameRate = targetFrame;
    }
}
