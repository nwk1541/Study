using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Profiling;
using Debug = UnityEngine.Debug;

public class TestSetActive : MonoBehaviour
{
    public GameObject targetObject;
    private const int ITERATIONS = 100000;

    void Start()
    {
        // 1. 공정한 테스트를 위해 JIT 컴파일 웜업 (초기 지연 방지)
        targetObject.SetActive(true);
        bool temp = targetObject.activeSelf;
    }

    private void TestStart()
    {
        Debug.Log("--- 테스트 시작 (10만 번 반복) ---");

        // 시나리오 A: 이미 켜져 있는 상태에서 계속 켜기 (상태 변경 없음)
        TestAlreadyActiveState();

        // 시나리오 B: 꺼져 있는 상태에서 켜기 (상태 변경 발생)
        TestStateChange();
    }

    private void TestAlreadyActiveState()
    {
        targetObject.SetActive(true); // 이미 켜진 상태로 고정

        // [1] if문 체크 방식
        Stopwatch sw1 = Stopwatch.StartNew();
        Profiler.BeginSample("[Same_State] If_Check");
        for (int i = 0; i < ITERATIONS; i++)
        {
            if (true != targetObject.activeSelf)
                targetObject.SetActive(true);
        }
        Profiler.EndSample();
        sw1.Stop();

        // [2] 직접 호출 방식
        Stopwatch sw2 = Stopwatch.StartNew();
        Profiler.BeginSample("[Same_State] Direct_Call");
        for (int i = 0; i < ITERATIONS; i++)
        {
            targetObject.SetActive(true);
        }
        Profiler.EndSample();
        sw2.Stop();

        Debug.Log($"[상태가 같을 때] if문 체크: {sw1.ElapsedMilliseconds} ms");
        Debug.Log($"[상태가 같을 때] 그냥 호출: {sw2.ElapsedMilliseconds} ms");
    }

    private void TestStateChange()
    {
        // 상태가 '다를 때'를 시뮬레이션 하기 위해 
        // 매번 강제로 상태를 끄고 켜는 비용을 제외한 순수 호출 횟수 논리를 반영

        Stopwatch sw1 = Stopwatch.StartNew();
        Profiler.BeginSample("[Diff_State] If_Check");
        for (int i = 0; i < ITERATIONS; i++)
        {
            targetObject.SetActive(false);

            if (true != targetObject.activeSelf)
                targetObject.SetActive(true);
        }
        Profiler.EndSample();
        sw1.Stop();

        Stopwatch sw2 = Stopwatch.StartNew();
        Profiler.BeginSample("[Diff_State] Direct_Call");
        for (int i = 0; i < ITERATIONS; i++)
        {
            targetObject.SetActive(false);

            targetObject.SetActive(true);
        }
        Profiler.EndSample();
        sw2.Stop();

        Debug.Log($"[상태가 다를 때] if문 체크: {sw1.ElapsedMilliseconds} ms");
        Debug.Log($"[상태가 다를 때] 그냥 호출: {sw2.ElapsedMilliseconds} ms");
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0f, Screen.height * 0.5f, Screen.width * 0.1f, Screen.height * 0.1f), "T"))
        {
            TestStart();
        }
    }
}
