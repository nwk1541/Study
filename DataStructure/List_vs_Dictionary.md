# List vs Dictionary

개발을 하면서 `List`를 자주 사용했었는데 그 이유에 대해서는 생각을 해본적이 없는것 같다.
그리하여 `List`와 `Dictionary`에 대한 차이점, 성능비교에 대해 알아보고자 한다.

비교에 앞서 현재 설명하려는 것은 `ArrayList`와 `HashTable`의 일반화(제너릭)된 컬렉션인 `List<T>`와 `Dictionary<TKey, TVal>` 이다.
기존 컬렉션의 경우 `object` 형식을 기반으로 하다보니 컬렉션 요소에 접근할 때 마다 boxing/unboxing이 일어나 성능 문제를 일으키게 된다.

제너릭 컬렉션의 경우 컴파일할 때 사용할 형식이 정해지므로 형변환을 일으키지 않고, 잘못된 형식의 객체를 담을 위험도 없어진다.

## List

C#의 `List<T>` 는 `ArrayList`의 일반화(제네릭)된 컬렉션이다. 

조회, 추가, 삭제 등 데이터를 다루는 자료구조로서의 기본적인 기능을 다 갖추고 있으며 배열처럼 따로 크기를 지정하지 않아도 되고, 특정 크기를 넘어갈 걱정도 없다. 또한 `[num]`과 같이 특정 요소에 임의접근이 가능하다.

```cs
public class List<T> : ...

var a = List[3];
```
[MS C# List Ref](https://docs.microsoft.com/ko-kr/dotnet/api/system.collections.generic.list-1?view=netframework-4.8)

## Dictionary

`Dictionary<TKey, TVal>` 는 `HashTable`의 일반화된 컬렉션이다. 키와 값이 쌍으로 이루어진 데이터를 다룰때 사용하며 리스트처럼 조회, 추가, 삭제 같은 기능을 갖추고 있으며 `[key]` 임의접근이 가능하지만 인덱스 대신 키를 사용한다.

```cs
public class Dictionary<TKey,TValue> : ...

var a = Dictionary[key];
```

[MS C# Dictionary Ref](https://docs.microsoft.com/ko-kr/dotnet/api/system.collections.generic.dictionary-2?view=netframework-4.8)

## 비교

위에만 보았을때 형식 매개변수가 다른것 말고는 크게 차이가 없어보인다. 두 자료구조의 핵심적인 차이는 내부 구현에 있다.

먼저 조회, 추가, 삭제 같은 기능들의 성능을 비교해본다.

### Add

```cs
static void Main(string[] args) {
    const int n_max = 100000;
    const int n_count = 10;
    var d_dic = new Dictionary<int, int>();
    var l_list = new List<int>();
    double f_dtime = 0f, f_ltime = 0f;

    for(int n_loop = 0; n_loop < n_count; n_loop++) {
        // Dictionary Add함수 체크
        Stopwatch dtime = new Stopwatch();
        dtime.Start();

        for (int n_i = 0; n_i < n_max; n_i++) {
            d_dic.Add(n_i, n_i);
        }

        dtime.Stop();
        f_dtime += dtime.Elapsed.TotalMilliseconds;

        // List Add함수 체크
        Stopwatch ltime = new Stopwatch();
        ltime.Start();

        for (int n_i = 0; n_i < n_max; n_i++) {
            l_list.Add(n_i);
        }

        ltime.Stop();
        f_ltime += ltime.Elapsed.TotalMilliseconds;

        d_dic.Clear();
        l_list.Clear();
    }

    Console.WriteLine("Dictionary, " + f_dtime / n_count);
    Console.WriteLine("List, " + f_ltime / n_count);
}
```
```
단위 : ms

Dictionary, 1.65035
List, 0.35018
```

총 10번을 돌려서 평균을 내었다.

C#의 레퍼런스 문서를 보면 List와 Dictionary 둘다 O(1), Capacity를 넘어갈시 재할당이 일어나서 O(n)의 시간복잡도를 가진다고 한다. 그런데 요소 추가의 경우 Dictionary가 시간상 더 걸렸다.

중단점을 찍어서 확인해보면 Dictionary의 경우 시간 편차가 굉장히 큰 반면 리스트는 거의 동일했다.

### Remove

```cs
static void Main(string[] args) {
    const int n_max = 100000;
    const int n_count = 10;
    var d_dic = new Dictionary<int, int>();
    var l_list = new List<int>();
    double f_dtime = 0f, f_ltime = 0f;

    for(int n_loop = 0; n_loop < n_count; n_loop++) {
        for (int n_i = 0; n_i < n_max; n_i++) {
            d_dic.Add(n_i, n_i);
            l_list.Add(n_i);
        }

        // Dictionary Remove함수 체크
        Stopwatch dtime = new Stopwatch();
        dtime.Start();

        d_dic.Remove(1000);

        dtime.Stop();
        f_dtime += dtime.Elapsed.TotalMilliseconds;

        // List Remove함수 체크
        Stopwatch ltime = new Stopwatch();
        ltime.Start();

        l_list.Remove(1000);

        ltime.Stop();
        f_ltime += ltime.Elapsed.TotalMilliseconds;

        d_dic.Clear();
        l_list.Clear();
    }

    Console.WriteLine("Dictionary, " + f_dtime / n_count);
    Console.WriteLine("List, " + f_ltime / n_count);
}
```
```
Dictionary, 0.00167
List, 0.01786
```

Dictionary가 빠르다. 이건 C# 문서를 확인해도 List의 경우 선형탐색을 하기때문에 O(n)이고, Dictionary의 경우 O(1) 이다.

이건 내부 구현상의 차이이므로 나중에 설명하겠다.

### Loop

```cs
static void Main(string[] args) {
    const int n_max = 100000;
    const int n_count = 10;
    var d_dic = new Dictionary<int, int>();
    var l_list = new List<int>();
    double f_dtime = 0f, f_ltime = 0f;

    for(int n_loop = 0; n_loop < n_count; n_loop++) {
        for (int n_i = 0; n_i < n_max; n_i++) {
            d_dic.Add(n_i, n_i);
            l_list.Add(n_i);
        }

        // Dictionary 순회 체크
        Stopwatch dtime = new Stopwatch();
        dtime.Start();

        for(int n_i = 0; n_i < n_max; n_i++) {
            if (d_dic[n_i] == n_max)
                throw new Exception();
        }

        dtime.Stop();
        f_dtime += dtime.Elapsed.TotalMilliseconds;

        // List 순회 체크
        Stopwatch ltime = new Stopwatch();
        ltime.Start();

        for (int n_i = 0; n_i < n_max; n_i++) {
            if (l_list[n_i] == n_max)
                throw new Exception();
        }

        ltime.Stop();
        f_ltime += ltime.Elapsed.TotalMilliseconds;

        d_dic.Clear();
        l_list.Clear();
    }

    Console.WriteLine("Dictionary, " + f_dtime / n_count);
    Console.WriteLine("List, " + f_ltime / n_count);
}
```
```
Dictionary, 1.19467
List, 0.37306
```

각 컬렉션을 끝까지 단순순회후 시간을 측정해보았다. 보다시피 List가 훨씬 더 빨랏다.

### Find

```cs
static void Main(string[] args) {
    const int n_max = 100000;
    const int n_count = 10;
    var d_dic = new Dictionary<int, int>();
    var l_list = new List<int>();
    double f_dtime = 0f, f_ltime = 0f;

    for(int n_loop = 0; n_loop < n_count; n_loop++) {
        for (int n_i = 0; n_i < n_max; n_i++) {
            d_dic.Add(n_i, n_i);
            l_list.Add(n_i);
        }

        // Dictionary Contains함수 체크
        Stopwatch dtime = new Stopwatch();
        dtime.Start();

        for(int n_i = 0; n_i < n_max; n_i++) {
            if (d_dic.ContainsKey(n_max - 1)) // 1, 1000, n_max - 1
                break;
        }

        dtime.Stop();
        f_dtime += dtime.Elapsed.TotalMilliseconds;

        // List Contains함수 체크
        Stopwatch ltime = new Stopwatch();
        ltime.Start();

        for (int n_i = 0; n_i < n_max; n_i++) {
            if (l_list.Contains(n_max - 1)) // 1, 1000, n_max - 1
                break;
        }

        ltime.Stop();
        f_ltime += ltime.Elapsed.TotalMilliseconds;

        d_dic.Clear();
        l_list.Clear();
    }

    Console.WriteLine("Dictionary, " + f_dtime / n_count);
    Console.WriteLine("List, " + f_ltime / n_count);
}
```
```
// '1' 을 찾는 경우
Dictionary, 0.00166
List, 0.00061

// '1000` 을 찾는 경우
Dictionary, 0.00208
List, 0.00269

// 'n_max - 1` 을 찾는 경우
Dictionary, 0.00181
List, 0.2084
```

두 컬렉션의 `Contains` 함수를 이용, 시간을 측정해 보았다. 처음 추가된 요소는 List가 빠르지만 끝의 요소일수록 Dictionary가 더 빨랏다.

Contains 함수를 사용하지 않은 조건문에서의 찾기의 경우
> ex ) if(d_dic[n_i] == 1000)

모든 경우에서 List가 더 빨랐다. Contains 함수를 써야만 Dictionary의 속도가 빨라진다.

C#의 문서에 보면 List.Contains는 O(n)이다. Dictionary.ContainsKey는 O(1)이다.

## 결론

대부분의 경우 List의 속도가 우세했다. 일단은 데이터의 갯수가 엄청 많은 경우가 아닌이상 List를 사용하는게 좋을것같다. 

하지만 많은 데이터를 가지고 있는 컬렉션에서 특정 데이터 하나만을 찾고 싶다면 Dictionary가 훨씬 더 좋다.

이후에는 Dictionary의 내부 구조 HashTable에 대해서 알아본다.