# JavaScript

자바 스크립트는 객체 지향의 스크립트 프로그래밍 언어이다. 주로 웹 브라우저 내에서 사용하며 Node.js와 같이 서버측 언어로도 사용되고 있다.

`CloudScript` 관련해서 사용할 일이 있어 기본적인 문법을 여기에 정리해둔다.

# 개발 환경

* Visual Studio Code
    * VS Code Extension, Code Runner
    * VS Code Extension, ESLint
* Node.js 14.15.1

주 언어가 아닌 서브 언어를 사용할 때 VS Code를 주로 사용했었고, 에디터 자체가 프로그래밍 관련해서 범용적인 부분을 다루기 때문에 공부하기에도 좋다.

웹 브라우저에서 JavaScript를 다루기 위해 공부하는 것이 아니고, 로직 작성을 위한 것이며 또 잘 모르기도 해서 그냥 Node.js를 설치해서 사용한다.

## 기본 문법, 변수, 자료형

JavaScript는 대소문자를 구별하며 유니코드 문자셋을 이용한다.
따라서 다음과 같은 코드가 유효한다.

```js
var 갑을 = "병정";

var value = "12345";
var Value = "ABCDE";
```

하지만 `value`와 `Value`는 다르다. 대소문자를 구분하기 때문

JavaScript 에서는 명령을 `명령문(Statement)` 라고 부르며, 세미콜론(`;`) 으로 구분한다.

JavaScript의 스크립트 소스는 왼쪽에서 오른쪽으로 탐색하면서 토큰, 제어 문자, 줄바꿈 문자, 주석이나 공백으로 이루어진 입력 element의 시퀀스로 변환된다. 스페이스, 탭, 줄바꿈 문자는 공백으로 간주 된다.

### 주석

주석은 C++ 및 기타 다른 언어와 똑같다.

```js
// 한 줄 주석
/* 긴 주석
 * 여러 줄 주석
*/
```

주석은 공백처럼 판단되고, 스크립트 실행 시 버려진다.

### 선언

JavaScript의 선언에는 3가지 방법이 존재한다.

> var
* 변수를 선언과 동시에 값을 초기화

>let
* 블록 범위(scope) 지역 변수를 선언과 동시에 값을 초기화

>const
* 블록 범위 읽기 전용 상수 선언

### 변수

특정 값에 상징적인 이름으로 변수를 사용한다. 변수 명은 `식별자(Identifier)`라고 불리며 특정 규칙을 따른다.

네이밍의 경우 기호에 따라 달라지지만 보통 `Number_hits`, `temp99`, `$credit`, `_name` 등으로 쓰여진다.

### 변수 선언

변수 선언은 3가지의 방법이 있다.

```js
var x = 42 // 이 구문은 지역 및 전역 변수를 선언하는데 모두 사용 될 수 있다.
let y = 13 // 이 구문은 블록 범위 지역 변수를 선언하는데 사용 될 수 있다.
x = 42 // 이 구문은 선언되지 않는 전역변수를 만든다. 하지만 사용을 권고하진 않는다.
```

### 변수 할당

지정된 초기값 없이 `var` 혹은 `let` 문을 사용해서 선언된 변수를 `undefined` 값을 갖는다.

선언되지 않는 변수에 접근을 시도하는 경우 `ReferenceError` 예외가 발생한다.

```js
var a;
console.log("a 값은 " + a);

console.log("b 값은 " + b);
var b;

console.log("c 값은 " + c);

let x;
console.log("x 값은 " + x);

console.log("y 값은 " + y);
let y;
```

* 결과

![](./Imgs/val_init_1.PNG)

![](./Imgs/val_init_2.PNG)

`undefined`를 사용하여 변수값이 있는지 확인할 수 있다. 아래 코드에서 `input` 변수는 값이 할당되지 않았고, `if`문은 `true`로 평가된다.

```js
var input;
if(input == undefined)
    console.log("input is undefined");
else
    console.log("input has a value");
```

* 결과

![](./Imgs/val_undefined.PNG)

`undefined` 값은 `boolean` 문맥(context)에서 사용될 때 `false`로 동작한다.

```js
var tempArr = [];
if(!tempArr[0])
    console.log("Hello World");
```

* 결과

![](./Imgs/val_undefined_2.PNG)

`undefined` 값은 수치 문맥에서 사용될 때 `NaN`으로 변환된다.

```js
var a;
console.log(a + 2);
```

* 결과

![](./Imgs/val_undefined_3.PNG)

`null` 값을 평가할 때, 수치 문맥에서는 `0` 으로 `boolean` 문맥에서는 `false`로 동작한다.

```js
var n = null;
console.log(n * 32);

if(!n)
    console.log("n is null");
```

* 결과

![](./Imgs/val_undefined_4.PNG)