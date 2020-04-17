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

## IEnumerator 인터페이스

## yield 키워드

`yield` 키워드는 `IEnumerator` 객체게 값을 전달하거나 반복의 종료를 알리기 위해 사용한다.

```cs
yield statement or Instruction();
yield return object;
yield break;
```