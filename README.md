# D-Tower
> 타워 배치와 강화, 스킬 사용으로 30개 웨이브를 방어하는 Unity 2D 타워 디펜스 게임.

**[게임 다운로드 · itch.io](https://hong0723.itch.io/d-tower)** · **[핵심 소스 보기](https://github.com/Hong0723/D-Tower/tree/Main/Assets/Script)**

## 프로젝트 소개
제한된 골드를 어디에 사용할지 판단하며 적의 이동 경로를 방어하는 게임입니다. 타워의 공격 성능을 높이는 동시에 보스 웨이브에 대비해 스킬과 자원을 관리합니다.

| 항목 | 내용 |
| --- | --- |
| 장르 | 2D 타워 디펜스 |
| 개발 환경 | Unity 2022.3.62f2, C# |
| 주요 기술 | Physics2D, 코루틴, Unity UI, Firebase Authentication · Realtime Database |
| 프로젝트 형태 | 팀 프로젝트 |
| 담당 역할 | PM, 타워 로직 설계 및 구현 |

## 담당 역할
프로젝트 진행을 관리하고, 타워를 배치한 뒤 적을 탐색하고 공격하는 핵심 로직을 설계·구현했습니다. 아래 구현 설명은 프로젝트 전체의 구조를 함께 소개하며, 개인 담당 범위는 PM과 타워 로직입니다.

## 게임 흐름
1. 회원가입·로그인 후 게임을 시작합니다.
2. 맵의 배치 지점을 선택해 골드로 타워를 설치하거나 강화합니다.
3. 적을 처치하며 골드를 확보하고, 스킬로 위험한 구간을 넘깁니다.
4. 10·20·30번째 웨이브의 보스를 포함한 30개 웨이브를 방어합니다.
5. 클리어 후 점수와 닉네임을 통해 랭킹을 확인합니다.

기본 체력은 10입니다. 일반 적을 놓치면 체력이 1 감소하며, 보스를 놓치면 10 감소합니다.

## 주요 구현

### 타워 배치 · 강화 · 자동 공격
- 배치 노드와 선택 UI를 연결하고, 골드가 충분한지 확인한 뒤 타워를 생성합니다.
- 공격 범위 안의 적 중 가장 가까운 대상을 탐색해 자동으로 공격합니다.
- 1~5단계에 따라 공격 간격·범위·피해량과 시각 효과를 변경합니다.
- 발사체가 대상을 추적하고, 대상 소멸 또는 수명 만료 시 제거되도록 처리합니다.

**관련 코드:** [Tower.cs](Assets/Script/Towers/Tower.cs) · [Projectile.cs](Assets/Script/Towers/Projectile.cs) · [Node.cs](Assets/Script/Towers/Node.cs) · [TowerSelectUI.cs](Assets/Script/Core/TowerSelectUI.cs)

### 웨이브와 전투 진행
- 웨이브별 적 종류·수·생성 간격·체력·이동 속도를 데이터로 관리합니다.
- 준비 시간, 적 생성, 전투 종료 확인을 코루틴으로 연결합니다.
- 보스 웨이브의 BGM 변경과 최종 승리 씬 전환을 처리합니다.

**관련 코드:** [WaveManager.cs](Assets/Script/Enemies/WaveManager.cs) · [WaveData.cs](Assets/Script/Enemies/WaveData.cs)

### 스킬 · 자원 · 계정
- 번개·빙결·운석·통나무 스킬에 비용과 재사용 대기시간을 적용합니다.
- 골드와 점수 변경을 이벤트로 UI에 전달합니다.
- Firebase 기반 회원가입·로그인과 닉네임·랭킹 기능을 구성합니다.

**관련 코드:** [SkillManager.cs](Assets/Script/Core/SkillManager.cs) · [GoldBank.cs](Assets/Script/Core/GoldBank.cs) · [AuthManager.cs](Assets/Script/Login/AuthManager.cs) · [RankingManager.cs](Assets/Script/Core/RankingManager.cs)

## 조작 방법
| 조작 | 기능 |
| --- | --- |
| 배치 지점 클릭 | 타워 설치 UI 열기 |
| 설치된 타워의 지점 클릭 | 강화 UI 열기 |
| 스킬 버튼 및 대상 위치 선택 | 스킬 사용 |
| 배속 버튼 | 1·2·4·8·16배속 변경 |
| Skip | 준비 시간 넘기기 |
| Option | 일시정지 및 소리 설정 |

## 실행 방법
**플레이:** itch.io에서 배포 파일을 내려받아 압축 해제한 뒤 실행 파일을 실행합니다.

**소스:** 저장소를 내려받아 Unity Hub에서 프로젝트 루트를 추가하고 **Unity 2022.3.62f2**로 엽니다. Firebase를 사용하는 로그인·랭킹은 서비스 설정과 네트워크 연결 상태에 영향을 받습니다.

## 구조
- `Assets/Script/Towers`: 타워, 발사체, 배치 노드
- `Assets/Script/Enemies`: 적, 경로, 웨이브, 게임 속도
- `Assets/Script/Core`: 골드·점수·체력, 스킬, 게임 UI
- `Assets/Script/Login`: Firebase 초기화와 인증
- `Assets/Script/SceneMove`: 메뉴·로그인·패배 씬 이동

## 개선 방향
타깃 탐색 비용 측정과 탐색 주기 조정, 발사체 오브젝트 풀링, 타워 밸런스 데이터 분리를 후속 개선 과제로 고려할 수 있습니다.

프로젝트에 포함된 외부 에셋 및 라이브러리의 권리는 각 제작자에게 있습니다.

