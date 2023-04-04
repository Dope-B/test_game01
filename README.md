# 플랫포머 충돌처리 테스트 게임

#### 제작 기간: 2019.01.14~2019.02.20
#### 시용 기술: Unity(2020.1.61f 버전업)
#### 제작 의도
- 플랫포머 게임에서 일반 콜라이더를 사용했을 시 기울기가 있는 땅을 지날 때 어색함
- 특정 환경에서 충돌 예외처리의 필요성을 느낌
<p align="center">
<img width="30%" src="https://user-images.githubusercontent.com/33209821/229854651-6a095a63-4293-43b9-ab07-a697328e392f.png"/>
<img width="30%" src="https://user-images.githubusercontent.com/33209821/229854654-42743719-04b8-4841-8472-b345f1ff11fb.png"/>
<img width="30%" src="https://user-images.githubusercontent.com/33209821/229854655-156c94da-7103-4d1a-b6ae-cd71d169a40a.png"/>
</p>
<br/>

#### 설명
- 플레이어의 바닥 판정은 ground_check()함수에서 이뤄진다.
- 총 6개의 레이캐스트로 판별하며 예시는 아래와 같다.

<img width="25%" src="https://user-images.githubusercontent.com/33209821/229854642-dfc69813-6ef6-4155-a474-908158b43baa.png"/>

- 플레이어를 기준으로 왼쪽, 가운데, 오른쪽에 각각 2개의 레이캐스트를 사용하고 1개는 짧게(빨간색), 나머지 하나는 길게(파란색) 설정한다.
- 플레이어의 가운데만 레이캐스트를 한다면 가장자리 충돌처리를 못 하기때문에 3곳에서 레이캐스트를 한다.
- 땅에 착지했을때의 판정은 다음과 같다.
  - y축 속도가 0 이하 + 세 위치 중 하나라 긴 선은 true, 짧은선은 false를 리턴=> 긴 선은 땅에 닿고 짧은 선은 땅에 안 닿음  
- 착지 상태일 때 키입력이 없고 외부 힘을 받지 않는다면 플레이어 벡터를 0으로 한다.
  ```C#
  if ((ground && !ground2) || (ground3 && !ground4)||(ground5&&!ground6))// ground는 레이캐스트 결과를 의미
        {
            if (rigid.velocity.y < 0f) {on_ground = true;}
        }
  ```
- 긴 3개의 선이 모두 땅에 안 닿았을 때만 공중 판정을 한다.
- 타일의 크기는 고정되어 있으니 플레이어의 y축 위치는 true값을 리턴하는 레이캐스트의 y좌표에 일정 값을 더해서 결정된다.<br/> 따라서 땅에 경사가 있더라도 일정한 간격유지가 가능하다.

```C#
 if (ground5)// 가운데 선이 땅에 닿았다면
 {
      transform.position = new Vector3(transform.position.x, ground5.point.y + 1.9f, 0);// 가운데 땅을 기준으로 플레이어의 y축 위치 결정
  }
  else if (!ground5 && ground)// 오른쪽 선이 땅에 닿았다면
  {
      transform.position = new Vector3(transform.position.x, ground.point.y + 1.9f, 0);// 오른쪽 땅을 기준으로 플레이어의 y축 위치 결정
   }
   else if(!ground5 && ground3){// 왼쪽 선이 땅에 닿았다면
      transform.position = new Vector3(transform.position.x, ground3.point.y + 1.9f, 0);// 왼쪽 땅을 기준으로 플레이어의 y축 위치 결정
   }

```
- 수직방향으로의 벽 판정도 원리는 바닥 판정 판별과 같다.
- 몬스터는 이동 중 절벽에서는 방향을 바꿔야하기 때문에 이동 방향 기준 아래로 레이캐스트를 한다.
<img width="25%" src="https://user-images.githubusercontent.com/33209821/229854648-970f5f7b-a692-4a01-a71b-50dbb9e655ef.png"/>

- 절벽 감지 레이캐스트(빨간색)가 false값을 출력한다면 이동 방향을 바꾼다.
