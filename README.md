# FPS 게임

### 프로젝트 소개
FPS게임의 기본적인 구성요소들을 가지고 있는 게임입니다.

### 프로젝트 동기
1) 이전까지 배운 다양한 기능들을 구현
2) 3D게임의 제작 경험
3) 시네머신을 이용한 프로젝트 제작

### 프로젝트 언어 및 환경
  프로그래밍 언어: C#  
  게임엔진 : Unity
  
---
## 실습 코드
### Manager  
- [GameManager.cs](https://github.com/Songhosub/FPS/blob/main/FPS/Assets/Scripts/PlayScene/GameManager.cs)
- 
### Battle  
- [PlayerController.cs](https://github.com/Songhosub/FPS/blob/main/FPS/Assets/Scripts/PlayScene/Player/PlayerController.cs)  
- [EnemyController.cs](https://github.com/Songhosub/FPS/blob/main/FPS/Assets/Scripts/PlayScene/Enemy/EnemyController.cs)

### Login  
- [LoginManager.cs](https://github.com/Songhosub/FPS/blob/main/FPS/Assets/Scripts/MainScene/LoginManager.cs)

Etc  

---
## 주요 기능

### 전투
![캐릭터 이동]()  
방향키를 통하여 이동, 스페이스키를 통하여 점프
단 점프는 바닥에 있을 때 1회에 한하여 가능
마우스 좌클릭을 통하여 공격
단 공격은 탄창이 남아 있을 때에만 가능

![Zoom IN, Zoom OUT]()  
특정 무기(스나이퍼)에 한하여 Tap키를 누르는 동안만 fieldOfView를 변경하여 Zoom IN 실행

![무기 변경]()  
숫자키를 눌러 해당 숫자에 따른 무기로 변경

![적 AI]()  
FSM을 이용하여 상태에 따른 행동을 실행
NavMashAgant를 이용하여 자애물을 피하여 맵을 이동

### 비전투
![로그인]()  
PlayerPrefs를 이용한 로그인 시스템 구성
비로그인을 클릭 시 바로 전투로 진입

![시네머신]()  
시네머신 기능을 이용한 인트로 영상 구성
1회에 한해 볼 수 있도록 설정

### 잔체적인 게임의 흐름
- 전체 흐름도  
![전체 흐름도](https://github.com/user-attachments/assets/80971668-1a23-431f-aecf-bdebdaee80af)


