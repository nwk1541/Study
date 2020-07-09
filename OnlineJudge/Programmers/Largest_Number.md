## [가장 큰 수](https://programmers.co.kr/learn/courses/30/lessons/42746)

## 문제

0 또는 양의 정수가 주어졌을 때, 정수를 이어 붙여 만들 수 있는 가장 큰 수를 알아내 주세요.

예를 들어, 주어진 정수가 [6, 10, 2]라면 [6102, 6210, 1062, 1026, 2610, 2106]를 만들 수 있고, 이중 가장 큰 수는 6210입니다.

0 또는 양의 정수가 담긴 배열 numbers가 매개변수로 주어질 때, 순서를 재배치하여 만들 수 있는 가장 큰 수를 문자열로 바꾸어 return 하도록 solution 함수를 작성해주세요.

## 제한사항

* numbers의 길이는 1 이상 100,000 이하입니다.
* numbers의 원소는 0 이상 1,000 이하입니다.
* 정답이 너무 클 수 있으니 문자열로 바꾸어 return 합니다.

## 입출력 예

| numbers | return |
|:--------|:-------|
| [6, 10, 2] | 6210 |
| [3, 30, 34, 5, 9] | 9534330 |

## 풀이

여러번 재시도 했던 문제였다. 그만큼 어려웠다.

return 값이 string 인 이유는 숫자로 풀 생각을 하면 안되고 문자열 처리로 풀어야 해서 그렇다.

문제는 정렬인데 문자열 정렬의 경우 숫자와는 비교기준이 다르다. 아마 사전순? 정렬로 알고 있다.

```
input : "1", "2", "4", "10", "11", "21", "100"
sort : "1", "10", "100", "11", "2", "21", "4"
```

Sort 함수에 보면 IComparer를 매개변수로 넘겨서 커스텀한 정렬 기준을 세울 수 있다.

저렇게 정렬을 하는 이유는

```
input : "3", "34"
sort : "3", "34"

answer : "343"
```

위와 같이 3과 34가 있을 경우 가장 큰 수는 `343` 이지만 정렬을 하면 `334`가 되어버리기 때문에 커스텀하게 정렬 기준을 세워 두 문자열을 합친후에 대소 비교를 한다.

위 정렬은 질문게시판에서 힌트를 보고 해결한것이다.

### Code

```cs
using System;
using System.Text;

public class Solution {
    public string solution(int[] numbers) {
        StringBuilder answer = new StringBuilder();
        int length = numbers.Length;
        string[] strNums = new string[length];
        
        bool isZero = true;
        for(int idx = 0; idx < strNums.Length; idx++) {
            int currNum = numbers[idx];
            isZero &= currNum == 0;
            strNums[idx] = currNum.ToString();
        }
        
        if(isZero)
            return "0";
            
        Array.Sort(strNums, (lhs, rhs) => { return (rhs + lhs).CompareTo(lhs + rhs); });
        
        for(int idx = 0; idx < strNums.Length; idx++)
            answer.Append(strNums[idx].ToString());
        
        return answer.ToString();
    }
}
```