## [모의고사](https://programmers.co.kr/learn/courses/30/lessons/42840)

## 문제

수포자는 수학을 포기한 사람의 준말입니다. 수포자 삼인방은 모의고사에 수학 문제를 전부 찍으려 합니다. 수포자는 1번 문제부터 마지막 문제까지 다음과 같이 찍습니다.

1번 수포자가 찍는 방식: 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, ...
2번 수포자가 찍는 방식: 2, 1, 2, 3, 2, 4, 2, 5, 2, 1, 2, 3, 2, 4, 2, 5, ...
3번 수포자가 찍는 방식: 3, 3, 1, 1, 2, 2, 4, 4, 5, 5, 3, 3, 1, 1, 2, 2, 4, 4, 5, 5, ...

1번 문제부터 마지막 문제까지의 정답이 순서대로 들은 배열 answers가 주어졌을 때, 가장 많은 문제를 맞힌 사람이 누구인지 배열에 담아 return 하도록 solution 함수를 작성해주세요.

## 제한 조건

* 시험은 최대 10,000 문제로 구성되어있습니다.
* 문제의 정답은 1, 2, 3, 4, 5중 하나입니다.
* 가장 높은 점수를 받은 사람이 여럿일 경우, return하는 값을 오름차순 정렬해주세요.

## 입출력 예

| answers | return |
|:--------|:-------|
| [1,2,3,4,5] | [1] |
| [1,3,2,4,2] | [1,2,3] |

## 풀이

문제의 핵심은 수포자 3명의 인덱스를 구하는데에 있다.

각 수포자들의 찍는 패턴이 각자 다 다르기 때문에 `%` 연산으로 인덱스를 따로 계산해줘야 한다.

```
수포자가 찍는 인덱스 = [현재 문제의 인덱스 % 수포자가 찍는 패턴의 총 수]
```

이렇게 하면 문제가 몇 문제가 나오던 수포자의 찍기 패턴과 동일하게 인덱스가 구해진다.

### Code

```cs
using System;
using System.Collections.Generic;

public class Solution {
    public int[] solution(int[] answers) {
        int[] studentA = new int[] { 1, 2, 3, 4, 5 };
        int[] studentB = new int[] { 2, 1, 2, 3, 2, 4, 2, 5 };
        int[] studentC = new int[] { 3, 3, 1, 1, 2, 2, 4, 4, 5, 5 };
        int aCnt = 0, bCnt = 0, cCnt = 0;
        
        for(int idx = 0; idx < answers.Length; idx++) {
            int number = answers[idx];
            
            if(studentA[idx % studentA.Length] == number)
                aCnt++;
            
            if(studentB[idx % studentB.Length] == number)
                bCnt++;
            
            if(studentC[idx % studentC.Length] == number)
                cCnt++;
        }
        
        List<int> answer = new List<int>();
        int max = aCnt > bCnt ? (aCnt > cCnt ? aCnt : cCnt) : (bCnt > cCnt ? bCnt : cCnt);
        
        if(max == aCnt)
            answer.Add(1);
        if(max == bCnt)
            answer.Add(2);
        if(max == cCnt)
            answer.Add(3);
            
        return answer.ToArray();
    }
}
```