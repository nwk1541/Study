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

`IEnumerator`는 특정한 시퀀스에 대한 커서처럼 작동한다.
또한 인터페이스여서 다음과 같은 세가지를 구현하도록 하고 있다.

```cs
public Object Current { get; }
public bool MoveNext();
public void Reset();
```

* Current : 현재 시퀀스에 대한 요소를 가지는 프로퍼티
* MoveNext() : 다음 시퀀스를 진행할 수 있는지의 여부

MoveNext()가 호출이 되면 해당하는 로직을 수행한 후 결과는 Current 프로퍼티에 저장이 된다. 그리고 진행할 것이 없다면 false를 리턴하게 된다.

C# 에서는 몇가지 룰만 따르면 컴파일러가 자동으로 해당 IEnumerator를 상속받은 클래스 구현체를 생성해준다. 이것을 `Iterator block` 라고 하는것 같다.

[Iterator Block](https://codeblog.jonskeet.uk/2011/01/18/gotcha-around-iterator-blocks/)

`Iterator block`에는 다음과 같은 룰이 존재한다.

1. IEnumerator를 리턴할 것
2. yield 키워드를 사용할 것

그렇다면 yield 키워드는 무엇인가?

## yield 키워드

[MS Docs yield](https://docs.microsoft.com/ko-kr/dotnet/csharp/language-reference/keywords/yield)

`yield` 키워드는 `IEnumerator` 객체에 값을 전달하거나 반복의 종료를 알리기 위해 사용한다.

```cs
yield statement or Instruction();
yield return object; // 값을 전달
yield break; // 시퀀스 종료
```

그렇다면 아래 예제코드를 통해 `yield`, `IEnumerator`의 동작을 살펴보자

```cs
private void Start()
{
    IEnumerator et = CoEx();
    while(et.MoveNext())
    {
        Debug.Log(et.Current);
    }
}

IEnumerator CoEx()
{
    yield return 3;
    yield return 5;
    yield return 7;
}
```

1. while문 안에서 MoveNext()가 호출되면 CoEx()의 첫 yield문인 `yield return 3`을 만날때까지 실행
2. 리턴되는 값이 존재하므로 MoveNext()는 `true`가 되고, Current 에는 리턴된 값인 3이 할당됨
3. 다시 반복을 돌아 MoveNext()가 호출되면 이전에 실행됐던 yield 문의 바로 다음줄부터 재실행이됨, 그러므로 `yield return 5`를 수행하고 위 작업 반복

![](./Img/out.PNG)

실제로 유니티 코루틴은 매개변수로 IEnumerator를 받는다. 바로 코루틴의 함수가 실행되는게 아닌 IEnumerator의 포인터를 받는셈이다.

> public Coroutine StartCoroutine(IEnumerator routine);

또한 내부에서 `yield`를 만나게 되는 순간 `IEnumerator`의 구현체로 넘어가서 다음 반복을 진행할지 말지를 결정하고 그 다음 코드를 실행하게 된다. `yield` 키워드를 만나기 전까지는 코루틴은 완벽하게 일반함수와 동일하게 동작한다.