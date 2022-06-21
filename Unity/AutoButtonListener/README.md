# 유니티 버튼 리스너 연결 테스트

## 배경

유니티 버튼 자동화 관련해서 이것저것 찾아보던 중 흥미로운 내용이 있어서 찾아보고 직접 테스트 해봄

* ref1 : [https://stackoverflow.com/questions/10313979/methodinfo-invoke-performance-issue](https://stackoverflow.com/questions/10313979/methodinfo-invoke-performance-issue)
* ref2 : [https://blogs.msmvps.com/jonskeet/2008/08/09/making-reflection-fly-and-exploring-delegates/](https://blogs.msmvps.com/jonskeet/2008/08/09/making-reflection-fly-and-exploring-delegates/)

원래 `MethodInfo.Invoke` 로 구현하려고 했었지만 해당 클래스가 생소하기도 하고, 궁금하기도 해서 샘플 프로젝트를 만들어 로그를 찍어봄

* Button1 : MethodInfo.Invoke
* Button2 : Delegate.CreateDelegate
* Button3 : 유니티 인스펙터에서 직접 이벤트 할당

## MonoBehaviour.Start() 에서 초기화시

* 1번 확인

| Invoke | CreateDelegate | 인스펙터 |
|---|:---:|---:|
| 0.0888 | 0.0269 | 확인불가 |

## Button.onClick(), 버튼 직접 클릭시

* 10번 확인

| Invoke | CreateDelegate | 인스펙터 |
|---|:---:|---:|
| 0.5333 | 0.281 | 0.2598 |
| 0.3273 | 0.2907 | 0.2958 |
| 0.3193 | 0.2871 | 0.2874 |
| 0.316 | 0.3079 | 0.2886 |
| 0.3097 | 0.2976 | 0.2949 |
| 0.3117 | 0.2888 | 0.2928 |
| 0.3205 | 0.2928 | 0.2965 |
| 0.3129 | 0.2954 | 0.2902 |
| 0.311 | 0.2945 | 0.2881 |
| 0.3069 | 0.2996 | 0.2861 |

------

* 평균 

| 0.33686 | 0.29354 | 0.28802 |
|---|:---:|---:|

## 결론

* 속도 자체는 인스펙터에서 이벤트를 직접 할당하는게 빠르다.
* 자동화 코드를 만들경우 Invoke를 사용하는 것 보다 CreateDelegate를 사용해서 연결하는게 빠르다.
* 평균 속도는 세 방식다 평이하지만 Invoke의 경우 첫 실행에서 굉장히(상대적으로) 느리다.
* 실 적용시 클래스의 메소드 목록을 모두 순회해야 하기 때문에 초기화 시 더 오래 걸릴 가능성이 높다.

## 예상 실제 코드

* 버튼 오브젝트의 이름이 함수 이름과 같거나 비슷해야 한다. 어쨋건 버튼을 참조해서 함수를 찾을 수 있어야함

```cs
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class AutoButtonListener : MonoBehaviour
{
    private void Start()
    {
        Button button = GetComponentInChildren<Button>(true);
        string compare = string.Format("OnClick{0}", button.name.Replace("Button", ""));

        Type type = typeof(AutoButtonListener);
        MethodInfo[] methodInfo = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        MethodInfo target = null;
        
        for(int idx = 0; idx < methodInfo.Length; idx++)
        {
            MethodInfo item = methodInfo[idx];
            if (item.Name == compare)
            {
                target = item;
                break;
            }
        }

        Action action = (Action)Delegate.CreateDelegate(typeof(Action), this, target);
        button.onClick.AddListener(() => action());
    }

    public void OnClickTest()
    {
        Debug.Log("OnClickTest");
    }
}
```