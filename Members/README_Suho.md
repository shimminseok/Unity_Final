
## ManiMind_Suho 개발문서

---

## 주요개발목록

  - [1. 스킬시스템](#1.-스킬-시스템)
  - [2. 스킬연출](#2.-스킬연출)
  - [3. VFX](#3.-VFX시스템)
  - [4. SFX](#4.-SFX시스템)
  - [5. 적대치/적스킬사용로직](#5.-적대치/적스킬사용로직)

---

### 1. 스킬-시스템

#### 설계목적
- 스킬의 내부구조를 모르는 사람도 스킬데이터를 만들 수 있게 ScriptableObject의 필드를 지정.
- 다양한 스킬을 구현할 수 있도록 범용적인 스킬시스템 구현.
- 다른 팀원의 코드 구조에 맞게 스킬효과를 구현.
  
<hr>

#### 구현방식
- 스킬데이터를 ActiveSkillSO라는 ScriptableObject로 저장.
- 구조에 맞춰서 스킬의 효과를 데미지를 주는 부분과 버프/디버프/도트데미지와 같은 부가적인 효과를 주는 부분으로 나누어 적용.
- 유저가 선택한 스킬을 유닛에 있는 SkillManager에 저장하고, 이를 SkillController에서 배틀씬초기에 ActiveSkillSO를 실제 인게임내에서 사용하는 SKill객체를 만들어주는 SkillData클래스로 만들어 적용. 이후 SKillData는 자체적으로 스킬의 재사용횟수, 쿨타임등을 관리.
- 유저가 스킬을 선택하면, 유닛의 SkillController에서 CurrentSkill이 유저가 선택한 스킬로 바뀌고 CurrentSkill의 데이터를 읽어서 TargetSelect클래스를 사용해 SubTarget을 선택하고 Dictionary자료형으로 저장.
- 이후 유저가 배틀턴을 시작하면 스킬사용효과를 MainTarget과 SubTarget에 적용. 각각의 Target들에 다른 효과를 적용할 수 있도록 EffectData별로 Target과 사용효과를 정할 수 있음.


#### 근거리스킬과 원거리스킬 구현
- 근거리 스킬의 경우 ActiveSkillSO의 AnimationClip을 Play하며 애니메이션 트리거에 맞춰서 적유닛에 스킬효과를 부여.
- 원거리 스킬의 경우에는 애니메이션 트리거때 투사체가 발사. 해당 투사체는 ObjectPool을 사용하여 재사용가능.
- 투사체에 ActiveSkillSO데이터를 참조하여 투사체가 적과 충돌 시에 해당 스킬효과를 ACtiveSkillSO데이터를 참고하여 적용.
---
### 2. 스킬연출

#### 설계목적
- 턴제게임에서 유저에게 보여줄 수 있는 타격감, 액션을 지원.
- 높은 등급의 스킬에 액션을 넣어 유저가 높은 등급의 스킬을 원하는 욕구를 만들어 동기부여.

#### 구현방식
- [타임라인을 활용한 스킬컷씬 구현링크](https://velog.io/@suho1213/20250722TIL)
- 카메라 시네머신을 활용. 메인카메라는 움직이지않고 고정적인 위치, 넓은 FOV를 가짐. 타임라인에서 활용될 카메라는 스킬카메라로 FOV가 메인카메라보다 작고, 움직이면서 줌인하는 효과를 줌.
- 카메라들을 조절하기 쉽도록 VirtualCameraController클래스를 만들어 활용. 해당 카메라들의 처음 위치, 회전, FOV, Noise값을 저장하는 CameraAdjustData클래스로 값들을 저장하여 사용.
- 타임라인에서는 스킬이펙트가 발생하는 위치인 Effect와 VirualCamera의 이동을 애니메이션 트랙을 활용. VirtualCamera의 경우 MainCamera로 전환되는 것은 Cienmachine트랙을 활용하여 자연스럽게 원래의 구도로 카메라를 전환.
- TimeLine은 각각의 시작위치가 SceneOffset으로 되어있고, 시작위치를 기준으로 움직이기때문에 TimeLine이 재생되기전에 모든 시작위치를 캐릭터기준으로 초기화해줄 필요가 있기 때문에, TimeLineManager에서는 ActiveSkillSO에 매핑되어있는 TimeLine을 재생하는 역할을 한다. 런타임에서 애니메이션 컨트롤러, 시그널 리시버, 시네머신 트랙등을 바인딩하고 VirtualCamera와 Effect의 위치를 초기화 해준다.
- 스킬컷씬의 경우에는 스킬이펙트가 발생하고, 데미지가 들어가거나 카메라가 흔들리는 시점을 시그널을 통해서 제어한다.

#### 애니메이션 리깅
- 타임라인에서 사용되는 애니메이션은 [mixamo](mixamo.com)에서 스켈레톤 애니메이션으로 받아와 이를 유니티의 애니메이션 리깅(Animation Rigging) 패키지를 통해 수정하였다.
- [애니메이션 리깅을 활용한 애니메이션 수정 포스트 링크](https://velog.io/@suho1213/20250723TIL)
- 간단한 휴머노이드 애니메이션 정도는 수정할 수 있게 되었다.

#### 결과물


![2025-08-04 11-24-07](https://github.com/user-attachments/assets/0e488092-8573-42f7-9f5e-06e5a1501d24)
![2025-08-04 11-25-12](https://github.com/user-attachments/assets/437f43a4-fb56-4745-a513-9bd5ed6fb594)
![2025-08-04 11-24-38](https://github.com/user-attachments/assets/cb0181a3-1ae5-49f3-9837-4b95fd7b9f66)
![2025-08-04 11-25-52](https://github.com/user-attachments/assets/ef41b2ed-3563-4f04-b310-5da32a764f79)

이외에도 3가지 정도의 컷씬을 더 추가해서 만들었다.

#### 가벼운 연출구현
- 이렇게 장면이 전환되는 스킬컷씬 뿐만아니라 가볍게 카메라가 줌인/줌아웃되고 카메라가 흔들리는 정도의 연출이 발생하는 스킬도 구현하였다.
- 만약 컷씬형태로 재생되는 경우가 아니라면, 재생되기전에 Effect오브젝트와 VirtualCamera의 위치를 초기화 해줄 필요가 없기때문에 이를 고려하여 연출을 구현하였다.

#### 가벼운 연출 결과물

![2025-08-05 20-49-21](https://github.com/user-attachments/assets/8434a287-7b6f-4b11-9c11-53372965593e)

### 3. VFX시스템

#### 설계목적
- 다양한 VFX를 리소스낭비 없이 쉽게 사용할 수 있도록 구현.
- ObjectPool을 통하여 재사용가능한 형태로 구현.
- 내부구조를 모르더라도 VFX를 쉽게 적용가능한 형태로 구현.

#### 구현방식
- VFXData


