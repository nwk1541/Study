# SetActive() 호출 시 activeSelf 상태 체크가 필요한가?

## 요약

`if (activeSelf != value) SetActive(value)` 패턴은 **불필요하다.** Unity 내부에서 이미 같은 상태에 대한 early-return 처리를 하고 있으며, 프로파일링 결과 성능 차이는 무의미한 수준이다.

---

## 1. 배경

Unity 개발 시 `SetActive()` 호출 전에 `activeSelf`로 상태를 확인하는 패턴이 관행적으로 사용된다.

```csharp
// 관행적 패턴: 상태 체크 후 호출
if (value != obj.activeSelf)
    obj.SetActive(value);

// 직접 호출
obj.SetActive(value);
```

이 패턴의 의도는 "이미 같은 상태일 때 불필요한 SetActive 호출을 피하자"이지만, 실제로 성능 이점이 있는지 검증한다.

---

## 2. Unity C# Reference 분석

**출처**: [UnityCsReference - GameObject.bindings.cs](https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Scripting/GameObject.bindings.cs)

```csharp
public extern bool activeSelf
{
    [NativeMethod(Name = "IsSelfActive")]
    get;
}

[NativeMethod(Name = "SetSelfActive")]
public extern void SetActive(bool value);
```

- **둘 다 `extern` 선언**으로, C# 측에는 구현이 없다.
- `activeSelf` → C++ 네이티브 메서드 `IsSelfActive` 호출
- `SetActive()` → C++ 네이티브 메서드 `SetSelfActive` 호출
- 즉 `activeSelf` 읽기도 managed → native 브릿지를 거치는 네이티브 호출이다.

### SetActive의 내부 동작

[Unity 공식 문서](https://docs.unity3d.com/ScriptReference/GameObject.SetActive.html)에 따르면:

> *"If the call to SetActive changes the value of GameObject.activeInHierarchy, this triggers MonoBehaviour.OnEnable or MonoBehaviour.OnDisable on all attached MonoBehaviour scripts."*

핵심은 **"changes the value"** 조건이다. 상태가 실제로 변경될 때만 다음이 발생한다:
- `OnEnable()` / `OnDisable()` 콜백 실행
- 렌더러, 콜라이더, 리지드바디 등 컴포넌트의 활성화/비활성화
- 하위 오브젝트 계층에 재귀적 전파 (`ActivateAwakeRecursively`)

**같은 상태로 호출 시에는 내부적으로 early-return 처리된다.**

---

## 3. 테스트 코드

`Assets/Scripts/TestSetActive.cs`:

```csharp
public class TestSetActive : MonoBehaviour
{
    public GameObject targetObject;
    private const int ITERATIONS = 1000000;

    // 시나리오 A: 이미 active인 상태에서 SetActive(true) 호출
    private void TestAlreadyActiveState()
    {
        targetObject.SetActive(true); // 이미 활성 상태로 설정

        // [1] if문 체크 방식
        Profiler.BeginSample("[Same_State] If_Check");
        for (int i = 0; i < ITERATIONS; i++)
        {
            if (true != targetObject.activeSelf)
                targetObject.SetActive(true);
        }
        Profiler.EndSample();

        // [2] 직접 호출 방식
        Profiler.BeginSample("[Same_State] Direct_Call");
        for (int i = 0; i < ITERATIONS; i++)
        {
            targetObject.SetActive(true);
        }
        Profiler.EndSample();
    }

    // 시나리오 B: 매번 상태가 바뀌는 경우
    private void TestStateChange()
    {
        // [1] if문 체크 방식
        Profiler.BeginSample("[Diff_State] If_Check");
        for (int i = 0; i < ITERATIONS; i++)
        {
            targetObject.SetActive(false);
            if (true != targetObject.activeSelf)
                targetObject.SetActive(true);
        }
        Profiler.EndSample();

        // [2] 직접 호출 방식
        Profiler.BeginSample("[Diff_State] Direct_Call");
        for (int i = 0; i < ITERATIONS; i++)
        {
            targetObject.SetActive(false);
            targetObject.SetActive(true);
        }
        Profiler.EndSample();
    }
}
```

---

## 4. 프로파일링 결과

환경: Unity 2022.3.62f2, 에디터 플레이모드

### 시나리오 A: 같은 상태 (100,000회)

이미 `active=true`인 오브젝트에 `SetActive(true)` 호출.

| 방식 | Time ms | Self ms | 내부 호출 |
|---|---|---|---|
| **[Same_State] If_Check** | 11.35 | 4.40 | `get_activeSelf()` 6.95ms (100,000 calls) |
| **[Same_State] Direct_Call** | 11.30 | 4.04 | `SetActive()` 7.25ms (100,000 calls) |

**차이: 0.05ms (100,000회 기준) — 무의미**

- if문 체크: `IsSelfActive` 네이티브 호출 1회 → 조건 불일치로 `SetActive` 스킵
- 직접 호출: `SetSelfActive` 네이티브 호출 1회 → 내부 early-return
- 네이티브 호출 1회 비용: `activeSelf` ≈ 0.0000695ms, `SetActive(early-return)` ≈ 0.0000725ms

### 시나리오 B: 다른 상태 (100,000회)

매번 `SetActive(false)` → `SetActive(true)` 로 실제 상태 변경.

| 방식 | Time ms | Self ms | Calls (SetActive) | Calls (get_activeSelf) |
|---|---|---|---|---|
| **[Diff_State] If_Check** | 292.69 | 15.35 | 200,000 | 100,000 |
| **[Diff_State] Direct_Call** | 286.70 | 10.58 | 200,000 | 0 |

**차이: 약 6ms — if문 체크가 오히려 손해**

if문 체크 방식은 `get_activeSelf`(7.72ms)를 추가로 호출하며, 상태가 항상 다르므로 어차피 `SetActive`도 호출된다. 네이티브 호출만 1회 늘어난 셈.

### 실제 비용의 소재

프로파일러가 보여주는 `SetActive()`의 실제 비용은 상태 비교가 아니라 **상태 변경 시 발생하는 내부 처리**에 있다:

| 내부 함수 | Time ms | 설명 |
|---|---|---|
| `GameObject.ActivateAwakeRecursively` | ~130ms | 컴포넌트 활성화, 재귀적 전파 |
| `GameObject.Deactivate` | ~107ms | 컴포넌트 비활성화 |
| `SortingGroup.UpdateRenderer` | ~4.5ms | 렌더러 갱신 |
| `Component.SetGameObject` | ~0ms | 컴포넌트 참조 설정 |

---

## 5. 결론

### if문 체크 패턴은 불필요하다

| 상황 | if문 체크 | 직접 호출 | 판정 |
|---|---|---|---|
| 같은 상태 반복 호출 | 네이티브 1회 (`IsSelfActive`) | 네이티브 1회 (`SetSelfActive` + early-return) | **동일** |
| 상태가 다른 경우 | 네이티브 2회 (`IsSelfActive` + `SetSelfActive`) | 네이티브 1회 (`SetSelfActive`) | **직접 호출 유리** |

- Unity 내부에서 이미 같은 상태에 대한 early-return을 처리하므로, `activeSelf`로 미리 체크하는 것은 **같은 일을 두 번 하는 것**이다.
- `activeSelf`도 네이티브 브릿지를 거치는 호출이므로 "가벼운 체크"라는 전제 자체가 틀렸다.
- 상태가 다른 경우에는 if문 체크가 네이티브 호출을 추가하므로 오히려 손해다.

### 권장

```csharp
// 이렇게 쓸 필요 없다
if (value != obj.activeSelf)
    obj.SetActive(value);

// 이렇게 쓰면 된다
obj.SetActive(value);
```

성능 최적화가 필요한 상황이라면 `SetActive` 호출 자체의 조건 체크보다 **호출 빈도를 줄이는 설계**(오브젝트 풀링, `CanvasGroup.alpha`, 카메라 밖 컬링 등)가 효과적이다.
