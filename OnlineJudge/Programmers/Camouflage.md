## [위장](https://programmers.co.kr/learn/courses/30/lessons/42578?language=csharp)

## 문제

스파이들은 매일 다른 옷을 조합하여 입어 자신을 위장합니다.

예를 들어 스파이가 가진 옷이 아래와 같고 오늘 스파이가 동그란 안경, 긴 코트, 파란색 티셔츠를 입었다면 다음날은 청바지를 추가로 입거나 동그란 안경 대신 검정 선글라스를 착용하거나 해야 합니다.

| 종류 | 이름 |
|:-----|:-----|
| 얼굴 | 동그란 안경, 검정 선글라스 |
| 상의 | 파란색 티셔츠 |
| 하의 | 청바지 |
| 겉옷 | 긴 코트 |

스파이가 가진 의상들이 담긴 2차원 배열 clothes가 주어질 때 서로 다른 옷의 조합의 수를 return 하도록 solution 함수를 작성해주세요.

## 제한사항

* clothes의 각 행은 [의상의 이름, 의상의 종류]로 이루어져 있습니다.
* 스파이가 가진 의상의 수는 1개 이상 30개 이하입니다.
* 같은 이름을 가진 의상은 존재하지 않습니다.
* clothes의 모든 원소는 문자열로 이루어져 있습니다.
* 모든 문자열의 길이는 1 이상 20 이하인 자연수이고 알파벳 
* 소문자 또는 '_' 로만 이루어져 있습니다.
* 스파이는 하루에 최소 한 개의 의상은 입습니다.

## 입출력 예

입출력 예
| clothes | return |
|:--------|:-------|
| [[yellow_hat, headgear], [blue_sunglasses, eyewear], [green_turban, headgear]] | 5 |
| [[crow_mask, face], [blue_sunglasses, face], [smoky_makeup, face]] | 3 |

## 풀이

경우의 수를 구하는 문제라 구하는 법을 따로 검색해보았다.

경우의 수를 구하는 방법엔 `합의 법칙`, `곱의 법칙` 이 있는데 이번 경우는 곱의 법칙을 사용하면 된다.

곱의 법칙이란 어떤 경우의 수를 구할 때 두 경우가 모두 일어날 수 있을 경우이다. 구하는 법은 간단하다.

```
사과 a, b, c가 있고, 배 d, e가 있다고 가정할 경우
사과와 배를 하나씩 고르는 경우의 수는 3 * 2 = 6(가지)
```

처음에는 몰랐지만 위 문제의 경우 headgear만 입을수도 eyewear만 입을수도 있기 때문에 각 가짓수에 +1을 해주고

아무것도 고르지 않는 경우는 없기 때문에 마지막에 -1을 해준다.

### Code

```cs
using System;
using System.Collections.Generic;

public class Solution {
    public int solution(string[,] clothes) {
        int answer = 1;
        Dictionary<string, int> itemSet = new Dictionary<string, int>();
        for(int idx = 0; idx < clothes.GetLength(0); idx++)
        {
            string itemType = clothes[idx, 1];
            if (itemSet.ContainsKey(itemType))
                itemSet[itemType]++;
            else
                itemSet.Add(itemType, 2);
        }

        foreach (var item in itemSet)
            answer *= item.Value;
        
        return answer - 1;
    }
}
```