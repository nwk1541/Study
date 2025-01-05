# 카메라 컬링에 대한 이해

## 목차

1. MVP Transformation
2. Frustum Culling
3. Spatial Culling
4. Occlusion Culling
5. Culling Layer

---

* `컬링`이란? 보이지 않는 것을 그리지 않는 것

그렇다면 물체를 화면에 그리기 위해 어떤 동작이 필요한지 간단하게 짚고 넘어간다.

### MVP Transformation

물체가 위치한 좌표를 화면상에 표현하기 위해 다음과 같은 좌표 변환을 거친다.

1. 모델 변환 (Model Transformation) : 객체를 `로컬 좌표계`에서 `월드 좌표계`로 변환 한다.

    > 객체를 화면상에 배치하고 이동, 크기, 회전을 하기 위해서는 절대적인 위치를 나타내는 `월드 좌표계`로 변환해야 함

2. 뷰 변환 (View Transformation) : `월드 좌표계`에서 `카메라 좌표계`로 변환 한다.

    > 절대적인 위치에 배치되어도 카메라를 기준으로 화면을 바라보기에 카메라의 위치와 방향을 기준으로 변환한다

3. 투영 변환 (Projection Transformation) : `카메라 좌표계`에서 `클립 좌표계`로 변환 한다.

    > 사람은 모니터(2D)를 통해서 보기에 3D 좌표를 특정 범위로 `정규화`한다. 이 과정에서 `원근 투영`을 할지 `직교 투영`을 할지 적용됨

최종적으로 클립 좌표를 `화면 공간 변환`을 통해 스크린 좌표로 최종 렌더링 된다.

### Frustum Culling

![https://learnopengl.com/Guest-Articles/2021/Scene/Frustum-Culling](Images/CameraCulling_1.png)

* 뷰 절두체(View Frustum)라고 하는 카메라의 시야를 정의하는 영역 외 바깥 공간은 렌더링 하지 않는것, 해당 영역 내부에 있어야 화면에 렌더링한다.