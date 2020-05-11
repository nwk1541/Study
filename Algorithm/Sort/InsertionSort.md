# InsertionSort

이 정렬은 내가 C#에서 자주 사용하는 List.Sort()에 대해 다시 공부하고 직접 구현해보기 위함이다.

[MS List<T>.Sort 문서](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.sort?view=netcore-3.1)

If comparison is provided, the elements of the List<T> are sorted using the method represented by the delegate.

If comparison is null, an ArgumentNullException is thrown.

This method uses Array.Sort, which applies the introspective sort as follows:

* If the partition size is less than or equal to 16 elements, it uses an `insertion sort algorithm`

* If the number of partitions exceeds 2 log n, where n is the range of the input array, it uses a Heapsort algorithm.

* Otherwise, it uses a Quicksort algorithm.

This implementation performs an unstable sort; that is, if two elements are equal, their order might not be preserved. In contrast, a stable sort preserves the order of elements that are equal.

On average, this method is an O(n log n) operation, where n is Count; in the worst case it is an O(n2) operation.

## 삽입 정렬이란?

삽입 정렬은 작은 리스트와 대부분 정렬된 리스트에 상대적으로 효율적인 알고리즘이며 더 복잡한 알고리즘의 일부분으로 사용되기도 한다. 자료구조의 크기가 클수록 효율이 떨어지지만 구현이 간단하다.

[삽입 정렬 - 위키백과](https://ko.wikipedia.org/wiki/%EC%82%BD%EC%9E%85_%EC%A0%95%EB%A0%AC)

삽입 정렬은 다음과 같은 순서로 동작한다. (오름차순 기준)

1. 자료구조의 두번째 요소부터 시작하여 모든 요소를 앞에서부터 차례대로 비교한다.
2. 만약 자기 자신보다 큰값이 앞에 위치해있다면 두 요소의 위치를 바꾸고 다음 요소에 대해 루프를 진행한다.

## Code

```cs
using System;
using System.Collections.Generic;

namespace ConsoleApp2
{
    class Program
    {
        class InsertionSort
        {
            public static void Sort(List<int> list)
            {
                for(int idx = 1; idx < list.Count; idx++)
                {
                    for(int secIdx = 0; secIdx < idx; secIdx++)
                    {
                        if (list[idx] > list[secIdx])
                            continue;

                        int tmp = list[idx];
                        list[idx] = list[secIdx];
                        list[secIdx] = tmp;
                    }
                }
            }

            public static void Print(List<int> list)
            {
                for (int idx = 0; idx < list.Count; idx++)
                    Console.WriteLine("Element : {0}, Idx : {1}", list[idx], idx);
            }
        }

        static void Main(string[] args)
        {
            List<int> data = new List<int>() { 2, 7, 88, 9, 11, 24, 66, 15, 3, 91, 83, 99, 100 };

            Console.WriteLine("--- Before Sort");
            InsertionSort.Print(data);

            InsertionSort.Sort(data);

            Console.WriteLine("--- After Sort");
            InsertionSort.Print(data);
        }
    }
}
```