# Coroutine

코루틴이란 함수의 실행을 중지하여 Unity에게 제어권을 돌려주고, 계속할 때는 다음 프레임에서 중지한 부분 부터 실행을 계속할 수 있는 함수이다.

[Unity Coroutine Documentation](https://docs.unity3d.com/kr/530/Manual/Coroutines.html)

유니티 문서에서는 절차적 애니메이션이나 시간의 경과와 함께 천천히 변경이 있는 동작에서 코루틴을 사용하는 예시를 들고있다.

그럼 도대체 코루틴이 뭐길래 그런 동작이 가능한걸까? 현재 페이지의 작성 이유는 코루틴에대해 더 자세히 알아보고 내부동작에 대해 이해하기 위해 작성한다.

```cs
public class CoroutineEx : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(CoEx());
    }

    IEnumerator CoEx()
    {
        Debug.Log("Start Coroutine");
        yield return new WaitForSeconds(1f);
        Debug.Log("Wait One Frame");
        yield return null;
        Debug.Log("End Coroutine");
    }
}
```

위의 코드는 간단하게 코루틴에 대해 알아보고자 유니티에서 작성한 스크립트다.

코루틴은 다음과 같은 특성을 가진다.
1. 특정 작업을 단계적으로 발생하게 한다.
2. 시간이 흐름에 따라 발생하는 루틴을 작성할 수 있다.
3. 다른 연산이 완료될때까지 기다리는 루틴을 작성할 수 있다.

또한 명확하게 해야하는 부분이 코루틴은 쓰레드가 아니다. 즉 비동기가 아니라는 뜻이다. (동시에 실행하지 않는다) 코루틴은 `yield` 키워드를 실행하기 전까지 완전하게 통제가 가능하다.

코루틴의 주요 동작은 다음 두 가지에 의해 이루어진다.

1. IEnumerator 인터페이스
2. yeild 키워드

## IEnumerator 인터페이스

[MS Docs IEnumerator 인터페이스](https://docs.microsoft.com/ko-kr/dotnet/api/system.collections.ienumerator?view=netframework-4.8)

```cs
public interface IEnumerator
```

`IEnumerator`는 일종의 루프에 대한 커서처럼 작동한다. (C# 에서는 컬렉션) 이 인터페이스는 다음과 같은 세가지를 구현하도록 하고 있다.

```cs
public Object Current { get; }
public bool MoveNext();
public void Reset(); // 꼭 구현할 필요 없음
```

`Current`가 현재 루프에 대한 요소를 가진 프로퍼티이고, `MoveNext()`는 현재 루프에서 더 진행할 것이 있는지 확인하는 함수다.

MoveNext()가 호출이 되면 해당하는 로직을 수행한 후 결과는 Current 프로퍼티에 저장이 된다. 그리고 진행할 것이 없다면 false를 리턴하게 된다.

원래대로라면 IEnumerator를 상속받은 클래스를 구현해서 인터페이스를 다 구현해줘야 하는데, C# 에서는 몇가지 룰만 따르면 컴파일러가 자동으로 해당 IEnumerator를 상속받은 클래스 구현체를 생성해준다. 이것을 `Iterator block` 이라고 한다.

`Iterator block`에는 다음과 같은 룰이 존재한다.

1. IEnumerator를 리턴할 것
2. yield 키워드를 사용할 것

그렇다면 yield 키워드는 무엇인가?

## yield 키워드

[MS Docs yield](https://docs.microsoft.com/ko-kr/dotnet/csharp/language-reference/keywords/yield)

`yield` 키워드는 `IEnumerator` 객체에 값을 전달하거나 반복의 종료를 알리기 위해 사용한다.

```cs
yield statement or Instruction();
yield return object;
yield break;
```

즉, `yield return ~`은 MoveNext()를 true로 리턴하도록 하고 Current는 ~로 할당되도록 한다. `yield return break`는 MoveNext()가 false를 반환하도록 한다.

# 결론

정리하자면 코루틴은 C#의 Iterator block을 이용해서 IEnumerator 인터페이스의 구현체를 한번 래핑해놓은 것이다.