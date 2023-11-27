# MRid

> 노션에서 플레이 영상 등 더 자세한 내용 확인 가능합니다.  
> Notion: [Notion](https://www.notion.so/MRid-12b24ec49c8b49c6ba6b304f169e12c4?pvs=4)  
> GitHub: [GitHub](https://github.com/chocobubble/MRid-Demo)

<br>

## 프로젝트 소개
World of Warcraft와 Football Manager에서 아이디어를 가져왔습니다. 이 게임에서, 플레이어는 캐릭터들을 고용하고, 던전을 계속 반복해서 클리어하면서 캐릭터들을 성장시켜 최종적으로 마지막 던전을 클리어하는 것이 목표입니다. 전투는 AI로 진행됩니다. 던전에서 수집한 재화와 경험치를 이용해서 캐릭터들의 능력치를 업그레이드하며 더 강한 캐릭터를 고용하고 장비를 구매합니다.
- 게임 장르 : simulation, management  
- 제작 기간 : 2023. 4. ~ 2023. 5.
- 사용 엔진 : Unity
- 언어 : C#

<br><br>

## 주요 내용

### A* 알고리즘 (Pathfinding)
> [코드](https://github.com/chocobubble/MRid-Demo/blob/c740b89cce617a8ed84291c1c60390f7515a89d4/Assets/Scripts/Pathfinding.cs)

- 던전의 맵을 tile과 grid를 이용해 구현했는데, tile이 많아질수록 BFS를 통한 AI 경로찾기의 시간 복잡도가 매우 커져 더 효율적인 알고리즘은 A* 알고리즘을 이용해 AI 경로찾기를 구현했습니다.
- GameScene의 던전을 tile map과 grid를 이용하여 나누고 이동 가능한 노드들을 미리 세팅해 둔 후, 회피 길찾기 메소드와 공격 길찾기 메소드로 크게 나누었습니다.
- 회피 메소드는 해당 위치 까지 가는 최소 거리를 구하고 그 경로를 list로 반환합니다
- 공격 길찾기 메소드는 공격 타겟까지 가는 거리 중 공격 가능 범위에 들어오는 node 발견 시 해당 노드까지의 경로를 list로 반환합니다

### AI 로직 (AllyCtrl)
> [코드](https://github.com/chocobubble/MRid-Demo/blob/c740b89cce617a8ed84291c1c60390f7515a89d4/Assets/Scripts/AllyCtrl.cs)

- 캐릭터들의 상태는 STAY, ATTACK, MOVE 로 구분했습니다.
- 캐릭터들은 responseSpeed * 1초 만큼의 시간을 주기로 행동을 결정합니다.
    - 먼저 현재 위치가 범위스킬안에 포함되어 있는 지 확인하고, 그렇다면 범위 밖으로 이동할 곳을 찾아 이동합니다.
    - 범위스킬안에 포함되어 있지 않다면 공격 대상과의 거리를 계산합니다.
        - 대상과의 거리가 공격 가능 거리 이내라면 공격합니다
        - 그렇지 않다면 공격 가능 거리 이내까지 이동 합니다.
- 적은 공격 스킬 사용 시 모든 아군 캐릭터들에게 이벤트가 발생했음을 알립니다. 판단 코루틴 함수를 정지시키고 다시 판단 코루틴 함수를 호출해서 즉각 이벤트에 반응하게 합니다.
- 캐릭터는 이동 대상 위치 까지의 루트를 A* 알고리즘으로 부터 list로 받아 list를 탐색하며 움직입니다.
- 공격 상태에서는 스킬을 먼저 사용합니다. 스킬에는 시전시간이 있으며 이 시간동안 캐릭터는 움직일 수 없습니다. 쿨타임이라 스킬 사용이 가능하지 않다면 기본공격을 합니다.

<br><br>


## 게임 설명

### 캐릭터

- scriptable object(CharacterSO)와 prefab을 이용하여 캐릭터들을 구성하였습니다. 캐릭터의 scriptable object는 크게 두가지로 나누었습니다. 하나는, 캐릭터 직업별 베이스 스탯을 나타내는 scriptable object입니다. 다른 하나는, 게임 실행 중 scene 간의 이동에 있어 유지되어야 하는 데이터들을 보관하는 scriptable object로, 인게임 중에 scriptable object를 인스턴스화하여 사용합니다.
- prefab도 캐릭터 직업별 베이스를 구성하는데 사용하였습니다.
- 캐릭터를 구성하는 script는 두가지 입니다. 하나는 AllyCtrl 로, 전투 시 캐릭터의 움직임을 구사하며, 다른 하나는 ChracterStats로, 전투 시 일시적으로 변하는 스탯들을 보관합니다.

### 장비

- scriptable object(EquipmentSO)를 이용하였습니다

### UI

- Unity UI Toolkit 을 이용하여 제작하였습니다.

### Main Screen

- Main Screen은 Dungeon Screen, Shop Screen, Pub Screen, Inventory Screen, Prepare Screen 으로 구성되어 있으며, 화면 왼쪽의 TabBar를 통해 스크린 간의 전환이 이루어집니다.

### Dungeon Screen

- 던전 및 그 던전의 어려움 정도를 결정하는 화면입니다.
- start 버튼을 누르면 Prepare Screen UI가 팝업됩니다.
- 게임 진행도에 따라 표시되는 던전의 종류와 어려움 정도가 다릅니다. LevelSO scriptable object에 의해 결정됩니다.
 
    [<img width="500" alt="dungeon" src="./Images/dungeon.png">]

### Prepare Screen

- dungeon screen 에서 start 버튼을 누르면 이 screen이 팝업되어 나타납니다.
- 보유하고 있는 캐릭터 중 원하는 멤버를 골라 전투에 참여시킵니다.  
    [<img width="500" alt="prepare" src="./Images/prepare.png">]

### Shop Screen

- 장비는 무기와 갑옷으로 이루어져 있으며, scriptable object(EquipmentSO)로 다룹니다.
- 무기와 갑옷을 베이스로, 3종류의 레어도 모두 랜덤으로 결정되어 shop에 나타납니다.
- 구매한 장비는 GameManager에서 List로 관리합니다.

   [<img width="500" alt="shop" src="./Images/shop.png">]

### Pub Screen

- pub에서 용병들을 고용하고, 퀘스트를 받습니다.
- 고용한 용병들은 GameManager 에서 List로 관리합니다.
- 퀘스트 클리어에 따라 시도할 수 있는 던전 종류가 늘어납니다.

   [<img width="500" alt="pub" src="./Images/pub.png">]

### Inventory Screen

- 보유하고 있는 장비, 캐릭터 들과 캐릭터들의 스탯을 볼 수 있습니다.
- 캐릭터들의 장비 전환이 가능합니다.

   [<img width="500" alt="inventory" src="./Images/inventory.png">]

### Game Scene

- Prepare screen 에서 start 버튼을 누르면 이 scene 으로 이동합니다.
- 아군과 적의 hp를 큰 막대바로 보여주고, 가한 데미지를 수치로, 스킬 시전시 아래의 작은 막대바가 차오릅니다.
- 아군 캐릭터들은 적의 범위 공격 스킬이 자신의 바닥 아래에 깔리면 범위에서 벗어나는 위치를 찾고 그 위치로 이동합니다.
- 공격가능한 범위에서 벗어나면 공격할 수 있는 거리 까지 이동하여 공격합니다.  
   [<img width="500" alt="game" src="./Images/battle.png">]
